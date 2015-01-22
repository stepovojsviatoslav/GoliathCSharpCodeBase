local Class = require 'utils.hump.class'
local UnityExistsEntity = require 'unity/unityentity'
local RecipeList = require 'components.inventory.recipelist'

local UIBenchController = Class({__includes=UnityExistsEntity, init=function(self, gameObject)            
      UnityExistsEntity.init(self, gameObject)            
      self.benchController = gameObject:GetComponent("UIBenchController") 
      self.benchController:SetupLuaController(self)
      self.luaMapper:SetAlwaysVisible(true)
      GameController.eventSystem:AddListener("BENCH_PANEL_ACTIVATE", self)      
      self.recipeList = RecipeList('data/config/recipes_bench.dat')   
      self:AddRecipes(self.recipeList:GetNames())
      self.benchController:Toggle(false)        
      self.benchController:SetCallback(self.Callback)
end})

function UIBenchController:Update()
  UnityExistsEntity.Update(self)  
  if GameController.inputService:RightButtonWasPressed("bench") then  
    self:OnHide()
  end
end

function UIBenchController:OnShow()
  self.benchController:Toggle(true)
  local mainInterface = GameController.ui.mainInterface
  mainInterface:AddContainer("bench", self.benchEntity)
  mainInterface.unityInterface:ToggleInventory(true)
  GameController.inputService:PushFrame("bench")
end

function UIBenchController:OnHide()
  self.benchController:Toggle(false)
  local mainInterface = GameController.ui.mainInterface
  mainInterface:RemoveContainer("bench")
  mainInterface.unityInterface:ToggleInventory(false)
  GameController.inputService:PopFrame()
end

function UIBenchController:OnEvent(event, source)
  if event == "BENCH_PANEL_ACTIVATE" then    
    self:Initiate(source)
    self:OnShow()    
  end
end

function UIBenchController:GetCraftRecipe(item)  
  self.select = self.recipeList:GetBackhashResult(item)
  if self.select then 
    return self.select
  else        
    return {}
  end  
end

function UIBenchController:GetDescription(item)
  local descriptions = self.recipeList:GetDescriptions()  
  if descriptions[item] then    
    return descriptions[item]
  else
    return ""
  end
end

function UIBenchController:GetMaxCraftValue(resultItem)  
  local items = self.recipeList:GetBackhashResult(resultItem)
  local counts = {}
  for k, v in pairs(items) do
    counts[k] = GameController.inventory:GetItemCount(v)
  end  
  return math.min(unpack(items))  
end

function UIBenchController:CraftRequest(item, count)
  if self.select then
    for k, v in pairs(self.select) do      
      GameController.inventory:RemoveItems(v, 1)
    end
    GameController.inventory:AddItem(item, count)
    Console:Message("Craft")
  end
end

function UIBenchController:AddRecipes(recipes)
  local hides = self.recipeList:GetHides()
  for _, v in pairs(recipes) do
    if not hides[v[1]] then
      self.benchController:AddRecipe(v[1], v[2])
    end
  end
end

function UIBenchController:AddRecipe(recipe)  
  local recipes = self.recipeList:GetNames()  
  for k, v in pairs (recipes) do
    if v[1] == recipe then
      self.benchController:AddRecipe(v[1], v[2])
      break
    end
  end
end

function UIBenchController:Remove(ingredients)
  for k, v in ingredients do
    local count, item = unpack(v)
    --Console:Message("Quantity: "..count..", Item: "..item)
  end
end

function UIBenchController:GetCountAvailableItem(item)  
  return tonumber(GameController.inventory:GetItemCount(item))
end

function UIBenchController:GetResult(request)
  local result = self.recipeList:GetResultOnlyComponents(unpack(request))
  if result then
    return {result, self.recipeList:GetHides()[result]}
  else
    return {result, nil}
  end
end

function UIBenchController:Initiate(entity)
  self.benchEntity = entity
end

-- Callbacks
function UIBenchController:Callback(method, ...)  
  if self[method] ~= nil then      
    local result = self[method](self, ...)
    return result
  else
    return {}  
  end
end

return UIBenchController