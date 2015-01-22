local Class = require 'utils.hump.class'
local UnityExistsEntity = require 'unity/unityentity'
local RecipeList = require 'components.inventory.recipelist'

local MainInterface = Class({__includes=UnityExistsEntity, init=function(self, gameObject)
      UnityExistsEntity.init(self, gameObject)
      self.unityInterface = gameObject:GetComponent("MainInterface")
      self.unityInterface:SetupLuaController(self)
      self:ToggleInventory()
      self.luaMapper:SetAlwaysVisible(true)
      GameController.eventSystem:AddListener("INVENTORY_SLOT_CHANGED_CORE", self)
      self.unityInterface:SetCallback(self.Callback)
    
      self.simpleRecipeList = RecipeList('data/config/recipes_simple.dat')
      self.unityInterface.GetCraftRecipe = function (item1, item2)
        local result = self.simpleRecipeList:GetResult(item1, item2)
        return result
      end
      self.unityInterface.GetMaxCraftValue = function (resultItem) 
        local item1, item2 = unpack(self.simpleRecipeList:GetBackhashResult(resultItem))
        local count1 = GameController.inventory:GetItemCount(item1)
        local count2 = GameController.inventory:GetItemCount(item2)
        return math.min(count1, count2)        
      end
      self.containers = {}
end})

function MainInterface:AddContainer(containerName, container)
  self.containers[containerName] = container
end

function MainInterface:RemoveContainer(containerName)
  self.containers[containerName] = nil
end

function MainInterface:IsInventory(containerName)
  local result = false
  for k, v in pairs(GameController.inventory.grids) do
    if v._name == containerName then
      result = true
    end
  end
  return result
end

--using API
function MainInterface:ToggleInventory()
  self.unityInterface:ToggleInventory()
end

function MainInterface:SetSlot(container, index, item, count)
  self.unityInterface:SetSlot(container, index, item, count)
end

function MainInterface:SelectSlot(container, index)
  self.unityInterface:SelectInContainer(container, index)
end

function MainInterface:SetCharacterInfo(text)
  self.unityInterface:SetCharacterInfo(text)
end

function MainInterface:SetCharacterSlot(index, name)
  self.unityInterface:SetCharacterSlot(index, name)
end

function MainInterface:SelectCharacterSlot(index)
  self.unityInterface:SelectCharacterSlot(index)
end

function MainInterface:ResetSpells()  
  self.unityInterface:ResetSpells()
end

function MainInterface:SetupSpell(...)
  self.unityInterface:SetupSpell(...)
end

function MainInterface:GetSpellSlot(index)
  return self.unityInterface:GetSpellSlot(index)
end

function MainInterface:IsAttackPossible()
  return self.unityInterface:IsAttackPossible()
end

function MainInterface:GetSpellStatus(index)
  return self.unityInterface:GetSpellStatus(index)
end

function MainInterface:SetSpellStatus(index, status)
  self.unityInterface:SetSpellStatus(index, status)
end

function MainInterface:Update()
  if Input.GetKeyDown(KeyCode.Tab) then
    self.unityInterface:ToggleInventory()
  end
end

function MainInterface:OnEvent(e, params)
  if e == "INVENTORY_SLOT_CHANGED_CORE" then
    local container, idx, item, count = unpack(params)
    self:SetSlot(container, idx - 1, item, count)
    GameController.gadgetController:OnInventaryChanged()
  end  
end

-- Callbacks
function MainInterface:Callback(method, ...)  
  if self[method] ~= nil then      
    local result = self[method](self, ...)
    return result
  else
    return {}  
  end
end

function MainInterface:OnSlotChanged(containerName, slotIndex, slotName, slotItem, slotCount)  
  if self:IsInventory(containerName) then
    GameController.inventory:SetSlotFromUI(containerName, slotIndex + 1, slotItem, slotCount)
    
    if GameController.weaponController and slotName == GameController.weaponController.activeWeaponSlot then
      GameController.player.playerController:GetCurrentSlot():Message("OnWeaponSelect", slotItem)
    end
    
    -- check equipment
  else
    self.containers[containerName]:SetSlotFromUI(slotIndex + 1, slotItem, slotCount)
  end
end

function MainInterface:OnSlotSelected(containerName, slotIndex, slotName, slotItem, slotCount)
  --print('Slot selected: ' .. slotItem .. " - " .. slotCount)  
  --local characterEntity = GameController.player.playerController:GetCurrentSlot()
  --characterEntity.grenadeModeVisualizer:Disable()
  --if slotItem == 'grenade' and slotCount > 0 then
  --  characterEntity.grenadeModeVisualizer:Enable(slotItem)
  --end
end

function MainInterface:OnCharacterChanged(idx)
  -- show inventory for new character
end

function MainInterface:OnSlotDropped(item, count)
  -- drop items
end

function MainInterface:OnSpellCursorBegin(spell, idx)
  if (not GameController.inputService:CheckLayer('Default Layer')) then
    self.unityInterface:CancelSpellCursor()    
  end    
end

function MainInterface:OnSpellCursorEnd(spell, idx)  
end

function MainInterface:OnSpellApply(spell, idx, position)  
  if position then    
    return GameController.spellSystem:ApplySpell(spell, position)
  else    
    return GameController.spellSystem:ApplySpell(spell)
  end
end

function MainInterface:OnSpellCursorCancel(spell, idx)
end
--without left trigger
function MainInterface:OnStartChoiseTarget(spell, idx) 
  if (GameController.player.playerController:GetCurrentSlot().gamepadRightStickController) then
    GameController.player.playerController:GetCurrentSlot().gamepadRightStickController:Disable()
  end
end

function MainInterface:OnStopChoiseTarget(spell, idx)
  if (GameController.player.playerController:GetCurrentSlot().gamepadRightStickController) then
    GameController.player.playerController:GetCurrentSlot().gamepadRightStickController:Enable()
  end
end
--]]

function MainInterface:OnCraftRequest(container1, idx1, container2, idx2, result, count)
  local resultIsMech = GameController.database.data.heroes[result] ~= nil
  local slotSource = idx1 + 1
  local slotTarget = idx2 + 1  
  local item1, item2 =  unpack(self.simpleRecipeList:GetBackhashResult(result))
  -- here we need to get items from container1(2)/id1(2) at first
  local count1 = count    
  local slotCount1
  if self:IsInventory(container1) then
    slotCount1 = GameController.inventory:GetSlotCount(container1, slotSource)
    count1 = count1 - GameController.inventory:RemoveFromSlot(container1, slotSource, count1)
    GameController.inventory:RemoveItems(item1, count1)
  else
    slotCount1 = self.containers[container1]:GetSlotCount(slotSource)
    count1 = count1 - self.containers[container1]:RemoveFromSlot(slotSource, count1)
    self.containers[container1]:RemoveItems(item1, count1)
  end
  
  local count2 = count
  local slotCount2
  local restItem = 0
  if self:IsInventory(container2) then
    slotCount2 = GameController.inventory:GetSlotCount(container2, slotTarget)
    count2 = count2 - GameController.inventory:RemoveFromSlot(container2, slotTarget, count2)
    GameController.inventory:RemoveItems(item2, count2)
    if resultIsMech then
      GameController.player.playerController:LoadAndAdd(result)
    else
      restItem = GameController.inventory:AddItem(result, count)
    end
  else
    slotCount2 = self.containers[container2]:GetSlotCount(slotTarget)
    count2 = count2 - self.containers[container2]:RemoveFromSlot(slotTarget, count2)
    self.containers[container2]:RemoveItems(item2, count2)
    if resultIsMech then
      GameController.player.playerController:LoadAndAdd(result)
    else
      restItem = self.containers[container2]:AddItem(result, count)
    end
  end
  
  if restItem.count > 0 then
    GameController.dropManager:DropItem(restItem.name, restItem.count, GameController.player:GetPosition())
  end    
end

return MainInterface