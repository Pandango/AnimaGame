var express = require('express');
var bodyParser = require('body-parser');
var path = require('path');
var app = express();

// var gameRouters = require('./gameRouters');

var server = require("http").createServer(app);
var io = require('socket.io')(server);

app.set('port', process.env.PORT || 8080);
app.get('/', function(req, res){
    res.sendfile(__dirname + '/index.html');
});

app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true }));

var calculateCardUsed = require('./CalculateGame');

app.post('/calculate', function(req, res){
    var cardUsagedResData = req.body;
    var new_game_resource = calculateCardUsed.getUsagedGameResource(cardUsagedResData, game_resource);
    res.send(new_game_resource);
})

var calculateEndTurn = require('./CalculateEndTurn');
app.post('/endturn', function(req, res){
    var currentResource = req.body;
    var calculatedResource = calculateEndTurn.calResourceAfterEndingTurn(currentResource);
    res.send(calculatedResource);
});

var calculateEnvironmentEvent = require('./CalculateEnvironmentEvent');
app.post('/endround', function(req, res){
    var currentResource = req.body;
    var calculateEnvironmentEventObj = new calculateEnvironmentEvent();

    var newResource = calculateEnvironmentEventObj.calculate(currentResource);
    res.send(newResource);
});

var clients = [];
var ingame_clients = [];
var currentUser;

var game_resource;
var gameOverData;

var isGameStart = false;

io.on('connection', function (socket){
    
    socket.on("login", function(data){
        if(clients.length < 4 && isGameStart === false){
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
    socket.on("create_game", function(gameObjective){
        isGameStart = true;

        game_resource = {
            'populationFoodBalanced' : {
                'population' : 20,
                'food' : 80
            },
            'sharingResource' : {
                'wood' : 1000,
                'stone': 1000
            },
            'buildingResource' : {
                'woodCutterExp': 3,
                'mineExp' : 3,
                'farmExp' : 3,
                'townExp' : 3
            },
            'naturalResource' :{
                'waterExp' : 15,
                'forestExp' : 3
            }
        }
        io.sockets.emit("set_gameObjective", gameObjective);
        io.sockets.emit("create_game", game_resource);    
    });

    socket.on("update_game_resource", function(currentResource){
        game_resource = currentResource;
        io.sockets.emit("update_game_resource", game_resource);
    });

    socket.on("current_ingame_clients",function(){
        socket.emit("current_ingame_clients", {ingame_clients});
    })

    socket.on("join_game", function(playerdata){
         var current_ingame_user = {
                'username' : playerdata.username,
                'role': playerdata.role,
                'score': 0
            }
        
        ingame_clients.push(current_ingame_user);
        io.sockets.emit("current_ingame_clients", {ingame_clients});

        console.log("user " + current_ingame_user.username + " join game already");   
    });

    socket.on("sort_player_turn", function(){
        var sortItems = ingame_clients // sort by score
        sortItems.sort(function(a, b){
            return b.score - a.score;
        })
        
        //Update role
        for (var index = 0; index < sortItems.length; index++) {
            sortItems[index].role = index;
        }
        ingame_clients = sortItems;
        io.sockets.emit("sorted_players", {ingame_clients});

        gameTurnData = {
            "turnNo" : 1,
            "playerNameInCurrentTurn" : ingame_clients[0].username
        }
        io.sockets.emit("update_game_turn", {gameTurnData});
    })

    socket.on("update_game_turn", function(newGameTurn){
        var turnNo = parseInt(newGameTurn.turnNo) + 1;
        newGameTurn.turnNo = turnNo;
        gameTurnData = newGameTurn;
        io.sockets.emit("update_game_turn", {gameTurnData});
    });

    socket.on("game_over", function(reqGameOver){
        gameOverData = reqGameOver;
        isGameStart = false;
        io.sockets.emit("game_over", gameOverData);
    });

    socket.on("update_newround_resource", function(resource){
        var newResource = resource;
        io.sockets.emit("update_newround_resource", newResource);
    });

    socket.on("disconnect", function (){
        for(var userCouter = 0; userCouter < clients.length; userCouter++){
            if(clients[userCouter].username === currentUser.username){
                console.log("User " + clients[userCouter].username + " disconnected from lobby");
                clients.splice(userCouter, 1);
            }
                            
            if(ingame_clients.length > 0){ //after run game will have ingame_client clear when disconnect
                if(ingame_clients[userCouter].username === currentUser.username){
                    console.log("User " + ingame_clients[userCouter].username + " disconnected form game");
                    ingame_clients.splice(userCouter, 1 );
                }                  
            }
        }

        if(ingame_clients.length <= 0){
            isGameStart = false;
        }
        io.sockets.emit("current_ingame_clients", {ingame_clients});
        io.sockets.emit("current_client", {clients});
    })
});

server.listen(app.get('port'), function () {
    console.log("---------------Server is running : " + app.get('port') + "---------------");
})