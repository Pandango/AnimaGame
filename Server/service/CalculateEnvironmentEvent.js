var CalculateEnvironmentEvent = function(){
};

CalculateEnvironmentEvent.prototype.updatedResource_ = null;
CalculateEnvironmentEvent.prototype.eventName_ = null;

CalculateEnvironmentEvent.prototype.calculate = function(currentResource){
    this.updatedResource_ = currentResource;

    var randomEvent = getRandomIntInclusive(1, 10);
    if( randomEvent == 1){
        var randomRareDisater = getRandomIntInclusive(1, 2);
        this.onRareDisater(randomRareDisater);
    }else if(randomEvent <= 3){
        var randomDisater = getRandomIntInclusive(1, 4);
        this.onDisater(randomDisater);
    }else if(randomEvent <= 6){
        var randomNaturalEvent = getRandomIntInclusive(1, 4);
        this.onNatural(randomNaturalEvent);
    }else{
        this.onNothing();
    }
    var newResource = this.generateNewResource();
    
    return newResource;
};


CalculateEnvironmentEvent.prototype.generateNewResource = function (){
    var generateNewResource = this.updatedResource_;
    generateNewResource["gameEvent"] = this.eventName_;
    return generateNewResource;
};

CalculateEnvironmentEvent.prototype.onNothing = function(){
    this.eventName_ = EventNameCollection.NOTHING;
};

CalculateEnvironmentEvent.prototype.onRareDisater = function(eventNo){
     if(eventNo === 1){
        this.onEarthquake();
        this.eventName_  = EventNameCollection.EARTHQUAKE;
     }else{
        this.onTyphoon();
        this.eventName_ = EventNameCollection.TYPHOON;
     }
};

CalculateEnvironmentEvent.prototype.onDisater = function(eventNo){
    if(eventNo === 1){
        this.onWildfire();
        this.eventName_ = EventNameCollection.WILDFIRE;
    }else if (eventNo === 2){
        this.onFlood();
        this.eventName_ = EventNameCollection.FLOOD;
    }else if (eventNo === 3){
        this.eventName_ = EventNameCollection.GROUNDCOLLAPSE;
        this.onGroundCollapsation();
    }else{
        this.eventName_ = EventNameCollection.DESOLATION;
        this.onDesolation();
    }
};

CalculateEnvironmentEvent.prototype.onNatural = function(eventNo){
    if(eventNo === 1){
        this.eventName_ = EventNameCollection.RAIN;
        this.calWaterExp(3);
    }else if(eventNo === 2){
        this.eventName_ = EventNameCollection.FOOD;
        this.calFood(110);
    }else if(eventNo === 3){
        this.eventName_ = EventNameCollection.POPULATION;
        this.calPopulation(110);
    }else{
        this.eventName_ = EventNameCollection.TREE;
        this.calForestExp(3);
    }
};

CalculateEnvironmentEvent.prototype.onWildfire = function(){
    this.calForestExp(-3);
};

CalculateEnvironmentEvent.prototype.onFlood = function(){
    this.calFood(70);
};

CalculateEnvironmentEvent.prototype.onGroundCollapsation = function(){
    var randomDecreaseContructEvent = getRandomIntInclusive(1, 5);

    if(randomDecreaseContructEvent == 1){
        this.calForestExp(-3);
    }else if(randomDecreaseContructEvent == 2){
        this.calWoodCutterExp(-3);
    }else if(randomDecreaseContructEvent == 3){
        this.calMineExp(-3);
    }else if(randomDecreaseContructEvent == 4){
        this.calFarmExp(-3);
    }else if(randomDecreaseContructEvent == 5){
        this.calTownExp(-3);
    }
};

CalculateEnvironmentEvent.prototype.onDesolation = function(){
    this.calWaterExp(-6);
};

CalculateEnvironmentEvent.prototype.onEarthquake = function(){
    this.calForestExp(-1)
    this.calWaterExp(-1)

    this.calFarmExp(-1);
    this.calMineExp(-1);
    this.calWoodCutterExp(-1);
    this.calTownExp(-1);
    
    this.calPopulation(75);
};

CalculateEnvironmentEvent.prototype.onTyphoon = function(){
    this.calFarmExp(-3);
    this.calMineExp(-3);
    this.calWoodCutterExp(-3);
    this.calTownExp(-3);
};

CalculateEnvironmentEvent.prototype.manageForestResource = function(resourceExpValue){
    if (resourceExpValue < 0){
        return 0;
    }
};

CalculateEnvironmentEvent.prototype.manageBuildingResource = function(resourceExpValue){
    if (resourceExpValue < 3){
        return 3;
    }
};

CalculateEnvironmentEvent.prototype.calForestExp = function(unitValue){
    var calculatedResource = (this.updatedResource_.naturalResource.forestExp + unitValue);
    this.updatedResource_.naturalResource.forestExp = this.manageForestResource(calculatedResource);
};

CalculateEnvironmentEvent.prototype.calWaterExp = function(unitValue){
    this.updatedResource_.naturalResource.waterExp += unitValue;
};

CalculateEnvironmentEvent.prototype.calWoodCutterExp = function(unitValue){
    var calculatedResource = (this.updatedResource_.buildingResource.woodCutterExp + unitValue);
    this.updatedResource_.buildingResource.woodCutterExp = this.manageBuildingResource(calculatedResource);
};

CalculateEnvironmentEvent.prototype.calMineExp = function(unitValue){
    var calculatedResource = (this.updatedResource_.buildingResource.mineExp + unitValue)
    this.updatedResource_.buildingResource.mineExp = this.manageBuildingResource(calculatedResource);
};

CalculateEnvironmentEvent.prototype.calFarmExp = function(unitValue){
    var calculatedResource = (this.updatedResource_.buildingResource.farmExp + unitValue)
    this.updatedResource_.buildingResource.farmExp = this.manageBuildingResource(calculatedResource);
};

CalculateEnvironmentEvent.prototype.calTownExp = function(unitValue){
     var calculatedResource = (this.updatedResource_.buildingResource.townExp + unitValue)
    this.updatedResource_.buildingResource.townExp = this.manageBuildingResource(calculatedResource);
};

CalculateEnvironmentEvent.prototype.calWoodUnit = function(unitValue){
    this.updatedResource_.sharingResource.wood += unitValue;
};

CalculateEnvironmentEvent.prototype.calStoneUnit = function(unitValue){
    this.updatedResource_.sharingResource.stone += unitValue;
};

CalculateEnvironmentEvent.prototype.calPopulation = function(percent){
   
    var currentPopulation = this.updatedResource_.populationFoodBalanced.population;
    var result = Math.floor((currentPopulation * percent) / 100);
    this.updatedResource_.populationFoodBalanced.population = result;
};

CalculateEnvironmentEvent.prototype.calFood = function(percent){
    var currentFood = this.updatedResource_.populationFoodBalanced.food;
    var result = Math.floor((currentFood * percent) / 100);
    this.updatedResource_.populationFoodBalanced.food = result;
};

function getRandomIntInclusive(min, max) {
  min = Math.ceil(min);
  max = Math.floor(max);
  return Math.floor(Math.random() * (max - min + 1)) + min;
}

EventNameCollection = {
    RAIN : "RAIN",
    FOOD : "FOOD",
    POPULATION : "POPULATION",
    TREE : "TREE",
    WILDFIRE : "WILDFIRE",
    FLOOD : "FLOOD",
    GROUNDCOLLAPSE : "GROUNDCOLLAPSE",
    DESOLATION : "DESOLATION",
    EARTHQUAKE : "EARTHQUAKE",
    TYPHOON : "TYPHOON",
    NOTHING : "NOTHING"
}


module.exports = CalculateEnvironmentEvent;