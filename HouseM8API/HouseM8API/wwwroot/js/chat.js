"use strict";

let data = sessionStorage.getItem('access_token');
var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub", { accessTokenFactory: () => data }).build();

function AddMessage(message) {
    var li = document.createElement("li");
    li.textContent = message;
    document.getElementById("messagesList").appendChild(li);
}

function GetString(id) {
    document.getElementById(id).value;
}

connection.on("ReceivePrivateMessage", function (message, userId) {
    AddMessage("User " + userId + ": " + message);
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var sender = parseInt(document.getElementById("senderIdInput").value);
    var target = parseInt(document.getElementById("targetIdInput").value);
    var message = document.getElementById("messageInput").value;
    var connectionId = document.getElementById("groupInput").value;
    var d = new Date();

    connection.invoke("SendPrivateMessage", user, connectionId, message, d, target, sender).catch(function (err) {
        return console.error(err.toString());
    });

    event.preventDefault();
    document.getElementById("messageInput").value = "";
});