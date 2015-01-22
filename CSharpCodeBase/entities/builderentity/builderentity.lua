local Class = require 'utils.hump.class'
local Entity = require 'unity.entity'

local BuilderEntity = Class({__includes=Entity, init = function (self)
      Entity.init(self)
      self.enabled = true
      self.active = false
      self.static = false
      self.doneCallback = nil
      self.interruptCallback = nil
end})

function BuilderEntity:StartConstruction(entityName, doneCallback, interruptCallback)
  self.entity = GameController.entityFactory:Create(entityName, Input.RaycastMouseOnTerrain())
  self.entity:Enable()
  self.active = true
  self.doneCallback = doneCallback
  self.interruptCallback = interruptCallback
end

function BuilderEntity:InterruptConstruction()
  self.entity:Disable()
  self.active = false
  if self.interruptCallback then
      self:interruptCallback()
    end
end

function BuilderEntity:DoneConstruction()
    GameController.entityFactory:CreateInWorld('TreeFirEntity', self.entity:GetPosition())
    self.entity:Disable()
    self.active = false
    if self.doneCallback then
      self:doneCallback()
    end
end

function BuilderEntity:Update()
  Entity.Update(self)
  if self.active then
    if Input.GetKeyDown(KeyCode.Escape) then
      self:InterruptConstruction()
    end
    
    if GameController.inputService:BottomButtonWasPressed() or Input.GetKeyDown(KeyCode.KeypadEnter) then
      if self.entity.count == 0 then
        self:DoneConstruction()
      end
    end
    
    if Input.GetKey(KeyCode.KeypadPlus) then
      --повернуть +
      self.entity:Rotate(1)
    elseif Input.GetKey(KeyCode.KeypadMinus) then
      self.entity:Rotate(-1)
       --повернуть -
    end
    
    if Input.GetKey(KeyCode.UpArrow) then
      self.entity:MoveUpTransform(5)
    elseif Input.GetKey(KeyCode.DownArrow) then
      self.entity:MoveUpTransform(-5)
    end
    if Input.GetKey(KeyCode.RightArrow) then
      self.entity:MoveRightTransform(5)
    elseif Input.GetKey(KeyCode.LeftArrow) then
      self.entity:MoveRightTransform(-5)
    end
    
  else
    if Input.GetKeyDown(KeyCode.B) then
      self:StartConstruction('MockupEntity')
    end
  end
end

return BuilderEntity
