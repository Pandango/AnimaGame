exports.calResourceAfterEndingTurn = function(currentResource){
    var townLv = calBuildingLv(currentResource.buildingResource.townExp);
    var farmLv = calBuildingLv(currentResource.buildingResource.farmExp);
    var woodCutterLv = calBuildingLv(currentResource.buildingResource.woodCutterExp);
    var mineLv = calBuildingLv(currentResource.buildingResource.mineExp);

    var updatedPopulation = calculatePopulation(townLv, currentResource.populationFoodBalanced.population);
    var updatedFood = calculateFood(farmLv, currentResource.populationFoodBalanced.food);
    var updatedWood = calculateWood(woodCutterLv, currentResource.sharingResource.wood);
    var updatedStone = calculateStone(mineLv, currentResource.sharingResource.stone);

    currentResource.populationFoodBalanced.population = updatedPopulation;
    currentResource.populationFoodBalanced.food = updatedFood;
    currentResource.sharingResource.wood = updatedWood;
    currentResource.sharingResource.stone = updatedStone;
    
    return currentResource;
};

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

function calBuildingLv(exp){
    return Math.floor(exp / 3);
};