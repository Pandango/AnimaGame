var io;
var gameSocket;
var currentUser;
var app = require('./app');

exports.initGame = function (sio, socket){
    io = sio;
    gameSocket = socket;

    socket.on("login", createJoinedUser);
    socket.on("current_client", updateClient);

    socket.on("disconnet", onDisconnect);
}

function createJoinedUser(userdata) {
    var newUser = {
        'username' : data.username,
        'state' : data.state
    }

    currentUser = newUser;

    app.clients.push(newUser);
    io.sockets.emit("CURRENT_CLIENT", {clients});
    console.log("Username " + newUser.username + " is connected");
}

function updateClient() {
    socket.emit("current_client", {client});
}

function updateUserState(userData) {
    client[userData.role].state = data.state;

    io.sockets.emit("current_client", {client})
}

function onUpdateUsetState(data){
     //role 
        clients[data.role].state = data.state;

        io.sockets.emit("CURRENT_CLIENT", {clients});
        console.log("Username " + clients[data.role].username + " is " + clients[data.role].state);
}

function onDisconnect(){
    socket.broadcast.emit("USER_DISCONNECTED", currentUser);
    for(var userCouter = 0; userCouter < clients.length; userCouter++){
        if(clients[userCouter].username === currentUser.username){
            console.log("User " + clients[userCouter].username + " disconnected");
            clients.splice(userCouter, 1);
        }
    }
}