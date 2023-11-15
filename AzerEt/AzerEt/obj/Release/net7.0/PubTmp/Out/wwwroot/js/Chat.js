"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable the send button until connection is established.
document.getElementById("sendButton").disabled = true;








connection.on("ReceiveMessage", function (user, message) {
    var li = document.createElement("li");
    document.getElementById("messagesList").appendChild(li);
    // We can assign user-supplied strings to an element's textContent because it
    // is not interpreted as markup. If you're assigning in any other way, you 
    // should be aware of possible script injection concerns.
    li.textContent = `${user} says ${message}`;
});





connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

//Messagee

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
//document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (senderid, message) {

    var reciverid = document.getElementById("reciverid").value;
    if (reciverid == senderid) {
        var div = document.createElement("div");
        div.classList.add("reciver");
        div.textContent = message;
        document.getElementsByClassName("message")[0].appendChild(div);
        var div = document.getElementsByClassName("message")[0];
        div.scrollTop = div.scrollHeight;
    }
});


connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendMessage").addEventListener("click", function (event) {
    var reciverid = document.getElementById("reciverid").value;
    var senderid = document.getElementById("senderid").value;
    var message = document.getElementById("message").value;
    if (message !== "" && reciverid !== "" && senderid !== "") {

        connection.invoke("SendPrivate", reciverid, senderid, message).catch(function (err) {
            return console.error(err.toString());
        });
        event.preventDefault();

        var div = document.createElement("div");
        div.classList.add("sender")
        div.textContent = message;



        document.getElementsByClassName("message")[0].appendChild(div);
        document.getElementById("message").value = "";

        var div = document.getElementsByClassName("message")[0];
        div.scrollTop = div.scrollHeight;
    }
});

var input = document.getElementById("message");

// Execute a function when the user releases a key on the keyboard
input.addEventListener("keypress", function (event) {
    // Number 13 is the "Enter" key on the keyboard

    if (event.keyCode === 13) {
        // Cancel the default action, if needed
        event.preventDefault();
        // Trigger the button element with a click
        //document.getElementById("sendMessage").click();
        var reciverid = document.getElementById("reciverid").value;
        var senderid = document.getElementById("senderid").value;
        var message = document.getElementById("message").value;
        if (message !== "" && reciverid !== "" && senderid !== "") {

            connection.invoke("SendPrivate", reciverid, senderid, message).catch(function (err) {
                return console.error(err.toString());
            });
            event.preventDefault();

            var div = document.createElement("div");
            div.classList.add("sender")
            div.textContent = message;



            document.getElementsByClassName("message")[0].appendChild(div);
            document.getElementById("message").value = "";

            var div = document.getElementsByClassName("message")[0];
            div.scrollTop = div.scrollHeight;
        }

    }

});

var div = document.getElementsByClassName("message")[0];
div.scrollTop = div.scrollHeight;
