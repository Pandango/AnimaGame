exports.calResourceAfterEndingTurn = function(currentResource){
    var townLv = calBuildingLv(currentResource.buildingResource.townExp);
    var farmLv = calBuildingLv(currentResource.buildingResource.farmExp);
    var woodCutterLv = calBuildingLv(currentResource.buildingResource.woodCutterExp);
    var mineLv = calBuildingLv(currentResource.buildingResource.mineExp);

    var updatedPopulation = calculatePopulation(townLv, currentResource.populationFoodBalanced.population);
    var updatedFood = calculateFood(farmLv, currentResource.populationFoodBalanced.food);
    var updatedWood = calculateWood(woodCutterLv, currentResource.sharingResource.wood);
    var updatedStone = calculateStone(mineLv, currentResource.sharingResource.stone);

    currentResource.populationFoodBalanced.food = updatedFood;
    currentResource.populationFoodBalanced.population = checkPopFoodBalanced(updatedPopulation, updatedFood);
    currentResource.sharingResource.wood = updatedWood;
    currentResource.sharingResource.stone = updatedStone;
    
    return currentResource;
};

function checkPopFoodBalanced (populationUnit, foodUnit){
    var currentPopulationUnit = populationUnit;
    var balancePercent = calPopFoodBalance(populationUnit, foodUnit);

    if(balancePercent < 60 && balancePercent >= 40){
        currentPopulationUnit = getResetPopulation(foodUnit);
    }else if(balancePercent < 40){
        currentPopulationUnit = 0
    }
    
    return currentPopulationUnit;
}

function calculatePopulation(townLv, populationUnit){
    var currentPopulation = populationUnit + (populationUnit * townLv);
    return currentPopulation;
};

function calculateFood(farmLv, FoodUnit){
    var currentFood = FoodUnit + (FoodUnit * 1.5 * farmLv);
    return currentFood;
};

function calculateWood(woodCutterLv, woodUnit){
    var currentWoodUnit = woodUnit + (woodCutterLv * 300);
    return currentWoodUnit;
};

function calculateStone(mineLv, stoneUnit){
    var currentStoneUnit = stoneUnit + (mineLv * 300);
    return currentStoneUnit;
};

function calPopFoodBalance(populationUnit, foodUnit){
    var balancePercent = Math.floor((foodUnit / (populationUnit * 3)) * 100);
    return balancePercent;
};

function calBuildingLv(exp){
    return Math.floor(exp / 3);
};

function getResetPopulation(foodUnit){
    var newPopulation = Math.floor(foodUnit / 3);
    return newPopulation;
};

