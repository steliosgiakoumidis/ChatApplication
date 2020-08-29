# Chat Application using Nats server

## General Info

In order to be able to execute the application, first step would be to download the nats server. You can download it from https://nats.io/download/nats-io/nats-server/.

Unzip the downloaded file and execute the .exe file of the folder. Nats server will start running.

## How to run
Unzip the solution and browse to the folder containing the .sln file through your cmd window. Run the command  
```
dotnet build
```
(The above command is not mandatory, as the project would build directly by executing the dotnet run command, but it is a good practice to build the solution before we start)

Then, while having open two command line interfaces, browse to the folders containing the ChatApplication.ConsoleMessaging and ChatApplication.Api csproj files. When you are in the right folders execute to both of them
```
dotnet run
```
This will start the console messaging and the api application. The order the projects will be executed is not essential for testing purposes but it will affect the messages received because the application processes only the messages arriving after subscription is established. 


## How to use
The console application is set up so the user sends messages to himself. The user types a message and receives the text typed together with a timestamp and the user who sent it (in our case that would the the user itself). The application has hardcoded username "User1".

After executing the ChatApplication.Api (and while the MessagingConsole is still running) project you can browse to https://localhost/swagger where there are two options available.

- Get all messages received by a user. You need to pass the username whose messages will be harvested ("User1" in our case) and the response will be a list of messages.
- Send a message to a user. The user need to pass a text message, from whom this message is coming and to whom this message will be send. As an example you can use "User1" in "To" and "Api" in"From" properties. The message will very soon appear in the console.

## Api Documentation
For a detailed API Documentation please run the application and browse to the swagger page ( https://localhost/swagger)

## Support
In case questions contact me at stelios.giakoumidis[at]gmail.com