using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatApplication.Api.Models;
using CommonLibraries;
using Microsoft.AspNetCore.Mvc;

namespace ChatApplication.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private InMemoryStorage _inMemoryStorage;
        private INatsIntegration _natsIntegration;

        public ChatController(InMemoryStorage inMemoryStorage, INatsIntegration natsIntegration)
        {
            _inMemoryStorage = inMemoryStorage;
            _natsIntegration = natsIntegration;
        }

        /// <summary>
        /// Returns messages received by a user/subject
        /// </summary>
        /// <response code="200">Returns all received messages</response> 
        /// <response code="400">Missing username</response>  
        /// <response code="404">Message not found for this user/subject</response>  
        /// <response code="500">An internal error occured</response>
        [HttpGet]
        [Route("v1/username/{username}")]
        public async Task<IActionResult> GetReceivedMessagesForUser(string username)
        {
            if (String.IsNullOrEmpty(username))
                return BadRequest("Username is missing");

            var messages = _inMemoryStorage.GetMessagesByUserName(username);
            if (messages.Count == 0)
                return NotFound("User not found or has no messages");

            return Ok(messages);
        }


        /// <summary>
        /// Sends message to a user/subject
        /// </summary>
        /// <response code="200">Message is published</response> 
        /// <response code="400">Request form is incorrect</response>  
        /// <response code="500">An internal error occured</response>
        [HttpPost]
        public async Task<IActionResult> SendMessageToUser([FromBody] ChatMessage chatMessage)
        {
            if (chatMessage == null
                || String.IsNullOrEmpty(chatMessage.From)
                || String.IsNullOrEmpty(chatMessage.Message)
                || String.IsNullOrEmpty(chatMessage.To))
                return BadRequest("Request object is incorrect");

            if (!_natsIntegration.PublishMessage(chatMessage.Message, chatMessage.To, chatMessage.From))
                return StatusCode(500);

            return Ok();
        }
    }
}