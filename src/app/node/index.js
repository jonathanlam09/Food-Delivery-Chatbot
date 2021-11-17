var express = require('express');
var http = require('http');
var socketio = require('socket.io');

const app = express();
app.get('/',function(req,res) {
    res.sendFile('index.html');
  });
const server = http.createServer(app);
const server_port = 8000 || process.env.server_port;
const ws = socketio(server);

ws.on("connection", (socket)=>{
    console.log("A client has connected to the server...\n");

    socket.emit("test event", "noob tho");

    socket.on("disconnect", ()=>{
        console.log("Client left...\n");
    }) 
})



server.listen(server_port, ()=> console.log(`Websocket server has started on port ${server_port}.`));

