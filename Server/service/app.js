var express = require('express');
var path = require('path');
var app = express();

// var gameRouters = require('./gameRouters');

var server = require("http").createServer(app);
var io = require('socket.io')(server);
var sockets;

app.set('port', process.env.PORT || 8080);

var clients = [];
var ingame_client = [];

io.on('connection', function (socket){
    var currentUser;

    socket.on("login", function(data){
        if(clients.length < 4){
            currentUser = {
                'username' : data.username,
                'state': data.state
            }
            //add user in server
            clients.push(currentUser);
            
            socket.emit("user_connected", currentUser);
            io.sockets.emit("current_client", {clients});
            console.log("Username " + currentUser.username + " is connected");
        }else{
            message = "Room is full!"
            socket.emit("user_unconnected", message);
        }
    });

    socket.on("current_client", function() {
        socket.emit("current_client", {clients});
    });
    
    socket.on("update_userstate", function(data){
        //role 
        clients[data.role].state = data.state;

        io.sockets.emit("current_client", {clients});
        console.log("Username " + clients[data.role].username + " is " + clients[data.role].state);
    });

    //game play socket
    socket.on("create_game", function(){
        var game_resource = {
            'populationfoodBalanced' : {
                'population' : 20,
                'food' : 80
            },
            'sharingResource' : {
                'wood' : 1000,
                'stone': 1000
            },
            'buildingResource' : {
                'woodCutterExp': 0,
                'mineExp' : 0,
                'farmExp' : 0,
                'TownExp' : 0
            },
            'naturalResource' :{
                'waterLv' : 0,
                'forestExp' : 0
            }
        }
        io.sockets.emit("create_game",game_resource);
    });

    socket.on("disconnect", function (){
        socket.broadcast.emit("user_disconnect", currentUser);      
        for(var userCouter = 0; userCouter < clients.length; userCouter++){
            if(clients[userCouter].username === currentUser.username){
                console.log("User " + clients[userCouter].username + " disconnected");
                clients.splice(userCouter, 1);
            }
        }
        io.sockets.emit("current_client", {clients});
    })
});

server.listen(app.get('port'), function () {
    console.log("---------------Server is running : " + app.get('port') + "---------------");
})