var express = require('express');
var path = require('path');
var app = express();

var gameRouters = require('./gameRouters');

var server = require("http").createServer(app);
var io = require('socket.io')(server);

app.set('port', process.env.PORT || 8080);

var clients = [];

io.sockets.on('connection', function (socket){
    gameRouters.initGame(io, socket);

    
});

server.listen(app.get('port'), function () {
    console.log("---------------Server is running : " + app.get('port') + "---------------");
})