local Action = require 'components.controlsystem.action'
local Class = require 'utils.hump.class'
local Vector3 = require 'utils.hump.vector3'

local STATE_MOVING = 0
local STATE_REQUEST_INTERACT = 1
local STATE_INTERACT = 2

local ActionTake = Class({__includes=Action, init = function (self, entity, target)
      Action.init(self, 1, false, 'take')
      self.entity = entity
      self.target = target
      self.state = STATE_MOVING
end})

function ActionTake:OnMoveComplete()
  self.state = STATE_REQUEST_INTERACT
  self.entity.combat:Attack(self.target, self.entity.weaponContainer.takeWeapon)
end

function ActionTake:OnStartRunning()
  self.state = STATE_MOVING
  self.entity.mover:SetGoal(self.target, nil, self.entity.weaponContainer:GetAttackDistance(self.entity.weaponContainer.takeWeapon))
end

function ActionTake:OnStopRunning()
  if self.state == STATE_MOVING then
    self.entity.mover:Stop()
  end
end

function ActionTake:Update()
  if self.state == STATE_MOVING and not self.target.interactable then
    return true 
  end
  
  if self.state == STATE_MOVING and not self.entity.mover:IsHaveGoal() then
    self:OnMoveComplete()
  end
  
  return self.state == STATE_INTERACT and not self.entity.mecanim:CheckStateName("Action")
end

function ActionTake:OnEvent(data)
  if data == "punch" then            
    self.state = STATE_INTERACT
    if self.target.interactable and self.entity.weaponContainer:CanAttack(self.target)then
      self.target:Message("OnInteract", self.entity)
    end    
  end
end

function ActionTake:GetPriority()
  return self.state == STATE_MOVING and 1 or 3
end

function ActionTake:IsContinuous()
  return self.state == STATE_MOVING
end

return ActionTake