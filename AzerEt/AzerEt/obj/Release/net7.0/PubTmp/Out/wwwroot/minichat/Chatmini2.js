"use strict";



var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.on("Bildirim", function (senderid, message) {
    const audio = new Audio();
    audio.src = "../music/message.mp3";
    audio.play();
  

});

//connection.on("ReceiveMessage", function (senderid, message) {
//    var reciverid = document.getElementById("reciverid").value;


//    console.log()
    
//});

connection.on("ReceiveMessage", function (senderid, message) {
      var reciverid = document.getElementById("reciverid").value;
    if (reciverid == senderid) {

        var currentdate = new Date();
        var datetime =
      + currentdate.getHours() + ":"
      + currentdate.getMinutes()

        console.log(datetime);
        //var strr = "";
        //    strr += "          <span class=\"msg-avatar\">";
        //    strr += datetime;
        //strr += "          <\/span>";
          var div = document.createElement("div");
          div.classList.add("chat-msg");
          div.classList.add("user");

          var div2 = document.createElement("div");
        div2.classList.add("cm-msg-text");
        var p = document.createElement("span");
        p.textContent = message;

        var dt = document.createElement("span");
        dt.classList.add("dt-span");
        dt.textContent = datetime;
        div2.appendChild(p);
        div2.appendChild(dt);

        document.getElementsByClassName("chat-logs")[0].appendChild(div).appendChild(div2);
          var div = document.getElementsByClassName("chat-logs")[0];
          div.scrollTop = div.scrollHeight;
      }
  });


  connection.start().then(function () {
      document.getElementById("sendButton").disabled = false;
  }).catch(function (err) {
      return console.error(err.toString());
  });

  document.getElementById("chat-submit").addEventListener("click", function (event) {
console.log("cliccck");
      var reciverid = document.getElementById("reciverid").value;
      var senderid = document.getElementById("senderid").value;
      var message = document.getElementById("chat-input").value;
      if (message !== "" && reciverid !== "" && senderid !== "") {

          connection.invoke("SendPrivate", reciverid, senderid, message).catch(function (err) {
              return console.error(err.toString());
          });
          event.preventDefault();

          var currentdate = new Date();
          var datetime =
              //currentdate.getDate() + "/"
              //+ (currentdate.getMonth() + 1) + "/"
              //+ currentdate.getFullYear() + "  "
              + currentdate.getHours() + ":"
              + currentdate.getMinutes()
          console.log(datetime);
          var div = document.createElement("div");
          div.classList.add("chat-msg");
          div.classList.add("self");
          var div2 = document.createElement("div");
          div2.classList.add("cm-msg-text");
          var dt = document.createElement("span");
          dt.classList.add("dt-span");
          dt.textContent = datetime;
          var p = document.createElement("span");
          p.textContent = message;
          div2.appendChild(p);
          div2.appendChild(dt);


          console.log(reciverid);
          console.log(senderid);


          document.getElementsByClassName("chat-logs")[0].appendChild(div).appendChild(div2);
          //document.getElementsByClassName("self").appendChild(div2);
          document.getElementById("chat-input").value = "";

          var div = document.getElementsByClassName("chat-logs")[0];
          div.scrollTop = div.scrollHeight;
      }
  });


 //var input = document.getElementById("message");

 //// Execute a function when the user releases a key on the keyboard
 //input.addEventListener("keypress", function (event) {
 //    // Number 13 is the "Enter" key on the keyboard

 //    if (event.keyCode === 13) {
 //        // Cancel the default action, if needed
 //        event.preventDefault();
 //        // Trigger the button element with a click
 //        //document.getElementById("sendMessage").click();
 //        var reciverid = document.getElementById("reciverid").value;
 //        var senderid = document.getElementById("senderid").value;
 //        var message = document.getElementById("message").value;
 //        if (message !== "" && reciverid !== "" && senderid !== "") {

 //            connection.invoke("SendPrivate", reciverid, senderid, message).catch(function (err) {
 //                return console.error(err.toString());
 //            });
 //            event.preventDefault();

 //            var div = document.createElement("div");
 //            div.classList.add("sender")
 //            div.textContent = message;



 //            document.getElementsByClassName("message")[0].appendChild(div);
 //            document.getElementById("message").value = "";

 //            var div = document.getElementsByClassName("message")[0];
 //            div.scrollTop = div.scrollHeight;
 //        }

 //    }

 //});

var div = document.getElementsByClassName("chat-logs")[0];
 div.scrollTop = div.scrollHeight;