local Class = require 'utils.hump.class'
local UnityExistsEntity = require 'unity/unityentity'
local RecipeList = require 'components.inventory.recipelist'

local UISlotsController = Class({__includes=UnityExistsEntity, init=function(self, gameObject)
      UnityExistsEntity.init(self, gameObject)
      self.slotsController = gameObject:GetComponent("UISlotsController")
      self.slotsController:Select('weapon', 0)      
      self.slotsController:SetupLuaController(self)
      self.luaMapper:SetAlwaysVisible(true)
      GameController.eventSystem:AddListener("INVENTORY_SLOT_CHANGED_CORE", self)
      self.simpleRecipeList = RecipeList('data/config/recipes_simple.dat')
      self.slotsController:Toggle()
end})

function UISlotsController:SetContainerSubTag(container, tag)
  self.slotsController:ChangeSubTag(container, tag)
end

function UISlotsController:OnEvent(e, params)
  if e == "INVENTORY_SLOT_CHANGED_CORE" then
    local container, idx, item, count = unpack(params)
    self.slotsController:SetSlotFromCore(container, idx - 1, item, count)
  end
end

function UISlotsController:UserSetupSlot(container, slotId, item, count)
  GameController.inventory:SetSlotFromUI(container, slotId + 1, item, count)
end

function UISlotsController:UserSelectSlot(container, slotId, item, count)
  print("Slot: " .. slotId .. " selected in " .. container .. " to " .. item .. ":" .. count)
  if container == 'weapon' and (count > 0 or item == '') then
    GameController.player.playerController:GetCurrentSlot():Message("OnWeaponSelect", item)
  end
end

function UISlotsController:UserUseSlot(container, slotId, item, count)
  print("Slot: " .. slotId .. " used in " .. container .. " to " .. item .. ":" .. count)
end

function UISlotsController:UserDropItem(item, count)
  GameController.dropManager:DropItem(item, count, GameController.player:GetPosition(), 
    GameController.player.playerController:GetCurrentSlot(), 
    Transform.GetForwardVector(GameController.player.transform))
end

function UISlotsController:GetCraftRecipe(item1, item2)
  return self.simpleRecipeList:GetResult(item1, item2)
end

function UISlotsController:GetMaxCraftValue(resultItem)
  local item1, item2 = unpack(self.simpleRecipeList:GetBackhashResult(resultItem))
  local count1 = GameController.inventory:GetItemCount(item1)
  local count2 = GameController.inventory:GetItemCount(item2)
  return math.min(count1, count2)
end

function UISlotsController:CraftRequest(item, count, id1, id2, container1, container2)
  local slotSource = id1 + 1
  local slotTarget = id2 + 1
  local item1, item2 =  unpack(self.simpleRecipeList:GetBackhashResult(item))
  -- here we need to get items from container1(2)/id1(2) at first
  local count1 = count
  local slotCount1 = GameController.inventory:GetSlotCount(container1, slotSource)
  count1 = count1 - GameController.inventory:RemoveFromSlot(container1, slotSource, count1)
  GameController.inventory:RemoveItems(item, count1)
  
  local count2 = count
  local slotCount2 = GameController.inventory:GetSlotCount(container2, slotTarget)
  count2 = count2 - GameController.inventory:RemoveFromSlot(container2, slotTarget, count2)
  GameController.inventory:RemoveItems(item, count2)
  
  local restItem = GameController.inventory:AddItem(item, count)
  if restItem.count > 0 then
    GameController.dropManager:DropItem(restItem.name, restItem.count, GameController.player:GetPosition())
  end  
end

function UISlotsController:Update()
  UnityExistsEntity.Update(self)
  if Input.GetKeyDown(KeyCode.K) then
    GameController.inventory:AddItem('hero_axe', 1)
  end
  if Input.GetKeyDown(KeyCode.Tab) then
    self.slotsController:Toggle()
  end
  if Input.GetKeyDown(KeyCode.Escape) then
    self.slotsController:Toggle(false)
  end
end

return UISlotsController