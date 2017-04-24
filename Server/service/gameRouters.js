var io;
var gameSocket;
var currentUser;
var clients;

var gameRouters = module.exports = {};

gameRouters.initGame = function (sio, socket, client){
    io = sio;
    gameSocket = socket;
    clients = client;

    socket.on("login", createJoinedUser);
    socket.on("current_client", updateClient);

    socket.on("disconnect", onDisconnect);
}

function createJoinedUser(data) {
    var newUser = {
        'username' : data.username,
        'state' : data.state
    }

    currentUser = newUser;

    clients.push(newUser);
    gameSocket.emit(currentUser);
    // io.sockets.emit("current_client", app.clients);
    console.log("Username " + newUser.username + " is connected");
}

function updateClient() {
    gameSocket.emit("current_client", {client});
}

function updateUserState(userData) {
    client[userData.role].state = data.state;

    io.sockets.emit("current_client", {client})
}

function onUpdateUsetState(data){
     //role 
        clients[data.role].state = data.state;

        io.sockets.emit("current_client", {clients});
        console.log("Username " + clients[data.role].username + " is " + clients[data.role].state);
}

function onDisconnect(){
    gameSocket.broadcast.emit("user_disconnected", currentUser);
    for(var userCouter = 0; userCouter < clients.length; userCouter++){
        if(clients[userCouter].username === currentUser.username){
            console.log("User " + clients[userCouter].username + " disconnected");
            clients.splice(userCouter, 1);
        }
    }
}