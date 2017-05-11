exports.getUsagedGameResource = function(cardUsagedResData, game_resource){
    var new_game_resource = {
            'populationFoodBalanced' : {
                'population' : game_resource.populationFoodBalanced.population,
                'food' : game_resource.populationFoodBalanced.food
            },
            'sharingResource' : {
                'wood' : game_resource.sharingResource.wood + cardUsagedResData.getResource.woodUnit,
                'stone': game_resource.sharingResource.stone + cardUsagedResData.getResource.stoneUnit
            },
            'buildingResource' : {
                'woodCutterExp': game_resource.buildingResource.woodCutterExp + cardUsagedResData.getBuildingExp.woodCutterGetExp,
                'mineExp' : game_resource.buildingResource.mineExp + cardUsagedResData.getBuildingExp.mineGetExp,
                'farmExp' : game_resource.buildingResource.farmExp + cardUsagedResData.getBuildingExp.farmGetExp,
                'townExp' : game_resource.buildingResource.townExp + cardUsagedResData.getBuildingExp.townGetExp
            },
            'naturalResource' :{
                'waterExp' : game_resource.naturalResource.waterExp + cardUsagedResData.getNaturalExp.waterGetExp,
                'forestExp' : game_resource.naturalResource.forestExp + cardUsagedResData.getNaturalExp.forestGetExp
            }
    }
    return new_game_resource
}