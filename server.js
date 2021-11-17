var express = require('express');
var http = require('http');
var socketio = require('socket.io');
const app = express();
const server = http.createServer(app);
const server_port = 8000 || process.env.server_port;
const ws = socketio(server);


ws.on("connection", (socket)=>{
    console.log("A client has connected to the server...\n");

    socket.on("disconnect", ()=>{
        console.log("Client left...\n");
    }) 

    socket.on('toAdmin', function(toAdmin){
        console.log("//From Client Side//")
        socket.broadcast.emit('toAdmin', toAdmin);
        console.log(toAdmin);
    })
})

server.listen(server_port, ()=> console.log(`Websocket server has started on port ${server_port}.`));

