local Class = require 'utils.hump.class'
local CharacterEntity = require 'entities.player.character'
local Component = require 'components.component'

local PlayerController = Class({__includes=Component, init = function (self, entity)      
      Component.init(self, entity)
      self.entity.playerController = self
      self.slots = {}      
      self.countSlots = 0
      self.maxCountSlots = GameController.database:Get("heroes", "Common/maxCountSlots")
      self.currentSlot = 0                  
      self.characterGear = luanet.GameFacade.characterGear
      self.characterGear:SetupFinalCallback(function (idx) self:OnUIFinalSelectionCallback(idx) end)
      self.characterGearEnabled = false
end})

function PlayerController:OnUIFinalSelectionCallback(idx)
  print("Select slot: " .. idx)
  if idx + 1 ~= currentSlot then
    self:SelectSlot(idx + 1)
  end
end

function PlayerController:LoadAndAdd(entityName)
  local characterEntity = self:LoadSlot(entityName)
  self:AddSlot(characterEntity)
end

function PlayerController:LoadSlot(entityName)      
  local bundleName = GameController.database:Get('heroes', entityName .. '/bundle')
  local character = BundleUtils.CreateFromBundle(bundleName)
  character.name = entityName
  Transform.AddComponent(character, "LuaMapper")
  local characterEntity = CharacterEntity(character)
  return characterEntity
end

function PlayerController:AddSlot(entity)
  if self.countSlots < self.maxCountSlots then        
    entity.gameObject:SetActive(false)
    table.insert(self.slots, entity)
    self.countSlots = self.countSlots + 1    
    return true    
  end
  return false
end

function PlayerController:DeleteSlot(number)
  if number > 1 and number <= self.countSlots then
    local deletedSlot = self.currentSlot
    self:SelectSlot (1)
    deletedSlot = table.remove(self.slots, deletedSlot)
    deletedSlot:Destroy ()
    self.countSlots = self.countSlots - 1
    return true
  end
  return false
end

function PlayerController:DeleteCurrentSlot()
  local deletedSlot = self.currentSlot
  self:SelectSlot (1)
  deletedSlot = table.remove(self.slots, deletedSlot)
  deletedSlot:Destroy ()
  self.countSlots = self.countSlots - 1
  return true
end


function PlayerController:GetSlot(number)
  if number > 0 and number <= self.countSlots then
    return self.slots[number]
  else
    return nil
  end
end

function PlayerController:SelectSlot(number)
  if number ~= self.currentSlot and number > 0 and number <= self.countSlots then
    if self.currentSlot ~= 0 then
      local oldSlot = self:GetCurrentSlot()
      oldSlot.gameObject:SetActive(false)
      oldSlot:OnDeselectCharacter()
      oldSlot:Message("OnDeselectCharacter")
      GameController:RemoveEntity(oldSlot)
      self.currentSlot = number
    else      
      self.currentSlot = 1
    end
    local newSlot = self:GetCurrentSlot()
    newSlot:SetPosition(self.entity:GetPosition())
    newSlot:SetRotation(self.entity:GetRotation())
    newSlot.gameObject:SetActive(true)
    GameController:AddEntity(newSlot)
    newSlot:OnSelectCharacter()
    newSlot:Message("OnSelectCharacter")
    GameController.camera:LoadForEntity(newSlot)
    return true
  else
    return false
  end
end

function PlayerController:GetCurrentSlot()
  if self.currentSlot > 0 then
    return self:GetSlot(self.currentSlot)
  else
    return nil
  end
end

function PlayerController:Update()
  Component.Update(self)
  local slot = self:GetCurrentSlot()
  if slot then
    self.entity:SetPosition(slot:GetPosition())
    self.entity:SetRotation(slot:GetRotation())
  end
  if not self.characterGearEnabled and GameController.inputService:LeftBumperIsPressed() then
    self.characterGearEnabled = true
    for i = 1,5 do 
      local islot = self.slots[i]
      if islot ~= nil then
        self.characterGear:SetupSlot(i - 1, islot.config:Get('cgear_icon_left'), 
          islot.config:Get('cgear_icon_top'),
          islot.config:Get('cgear_icon_bottom'))
      else
        self.characterGear:SetupSlot(i - 1, '', '', '')
      end
    end
    self.characterGearSelected = self.currentSlot - 1
    self.characterGear:Show(self.currentSlot - 1)
    GameController.inputService:PushFrame("character_gear")
  end
  if self.characterGearEnabled and not GameController.inputService:LeftBumperIsPressed("character_gear") then
    self.characterGearEnabled = false
    self.characterGear:Hide()
    GameController.inputService:PopFrame()
  end
  if self.characterGearEnabled then
    if GameController.inputService:IsGamepad() then
      local lookVector = GameController.inputService:GetLookValue("character_gear")
      local selected = self.characterGearSelected
      --selected = self.currentSlot - 1
      if lookVector:Length() > 0.4 then
        local angle = math.atan2(lookVector.z, lookVector.x) * 180 / math.pi
        while angle < 0 do angle = angle + 360 end
        if angle > 315 or angle <= 45 then
          selected = 1
        elseif angle > 225 and angle <= 315 then
          selected = 0
        elseif angle > 135 and angle <= 225 then
          selected = 3
        else 
          selected = 2
        end
      end
      if self.slots[selected + 1] == nil then
        selected = self.characterGearSelected
        --selected = self.currentSlot - 1
      end
      if selected ~= self.characterGearSelected then
        -- update
        self.characterGear:UpdateSelected(selected)
        self.characterGearSelected = selected
      end
    end
  end
end

return PlayerController