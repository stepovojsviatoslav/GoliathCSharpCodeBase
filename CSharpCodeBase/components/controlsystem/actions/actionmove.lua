local Action = require 'components.controlsystem.action'
local Class = require 'utils.hump.class'
local Vector3 = require 'utils.hump.vector3'

local ActionMove = Class({__includes=Action, init = function (self, entity, target)
      Action.init(self, 1, true, 'move')
      self.isKeyboardControl = true
      self.currentInput = Vector3()
      self.entity = entity
      self.target = target
end})

function ActionMove:SwitchToKeyboard()
  self.isKeyboardControl = true
end

function ActionMove:SwitchToMouse()
  self.isKeyboardControl = false
end

function ActionMove:OnStartRunning()
  if self.target ~= nil then
    self:SwitchToMouse()
    self.entity.mover:SetGoal(self.target)
  end
  self.entity.mover:ResetSpeed()
end

function ActionMove:Update()
  local actionResult = true
  if GameController.inputService:LeftStickYIsPressed() or  GameController.inputService:LeftStickXIsPressed() then
    self.currentInput = GameController.inputService:LeftStickValues()
    self.currentInput:RotateAroundY(GameController.camera.angle) 
    self.entity.mover:SetInput(self.currentInput)
    actionResult = false
  end
  if self.isKeyboardControl then
    if Input.GetMouseButtonDown(1) then
      self.entity.mover:SetGoal(Input.RaycastMouseOnTerrain())
      self:SwitchToMouse()
      actionResult = false
    end
    
    if GameController.inputService:GetMouseButton(1) and self.currentInput:Length() == 0 then
      self:SwitchToMouse()
      actionResult = false
    end
  end
  
  if not self.isKeyboardControl then
    if GameController.inputService:GetMouseButton(1) then        
      self.entity.mover:SetGoal(Input.RaycastTargetEntity() or Input.RaycastMouseOnTerrain())
      actionResult = false
    end    
    
    if not self.entity.mover:IsHaveGoal() or GameController.inputService:LeftStickYIsPressed() or GameController.inputService:LeftStickXIsPressed() then
      self:SwitchToKeyboard()
    end
    
    if self.entity.mover:IsHaveGoal() then
      actionResult = false
    end
  end
  return actionResult
end

function ActionMove:OnStopRunning()
  self.entity.mover:Stop()
end
return ActionMove