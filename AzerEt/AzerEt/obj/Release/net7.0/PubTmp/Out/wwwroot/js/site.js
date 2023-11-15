"use strict";

 
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

connection.on("Bildirim", function  (senderid, message) {
    console.log(senderid);
    //console.log(message);
    const audio = new Audio();
    audio.src = "../music/message.mp3";
    audio.play();
    alert("yeni mesaj var" );

    var dattaa = {
        senderId: senderid
    };
    console.log(dattaa);

    var controllerName = "Wishlist";
    var actionName = "ChatCount";
    console.log(dattaa);
    var urll = `/${controllerName}/${actionName}`;
    $.ajax({
        type: "POST",
        url: urll,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(senderid),
        // data: JSON.stringify({ menuId: MenUId, userId: UseRId }),
        dataType: "json",
        success: function (response) {

            console.log("İstek başarıyla gönderildi:", response.message);
        },
        error: function (error) {
            console.error("İstek gönderme hatası:", error);
        }
    });
   
    audio.play();
    var msgid = $(".msgid");
    var msgsp = $(".msgsp");



    //var count = 0;
    for (var i = 0; i < msgid.length; i++) {
        if (msgid[i].dataset.msgid == senderid) {
            console.log("duzdu");
            var email = msgid[i].dataset.msgnm;
            alert("yeni mesaj var" + " " + email);
            var currentValue = parseInt(msgsp[i].textContent, 10);
            var newValue = currentValue + 1; 
            var countt = msgsp[i].dataset.msgcnt;
            console.log(newValue);
            msgsp[i].textContent = countt;
           
           

         
            
        } else {
            console.log("beraber deyl");
        }
        
    }
 
   

});

//function yenileSpan() {
//    // <span> elementini buluyoruz
//    var spanElement = $(".msgsp");

//    // İçeriği değiştiriyoruz
//    spanElement.textContent = 3;
//}

//// Her 2 saniyede bir <span> elementini güncelleyecek olan zamanlayıcıyı başlatma
//var zamanlayici = setInterval(yenileSpan, 2000);


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
        if (message!== "" && reciverid !== "" && senderid !== "") {
            
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

 
