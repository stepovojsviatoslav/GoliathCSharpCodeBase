local Action = require 'components.controlsystem.action'
local Class = require 'utils.hump.class'
local Vector3 = require 'utils.hump.vector3'

local STATE_MOVING = 0
local STATE_REQUEST_INTERACT = 1
local STATE_INTERACT = 2

local ActionInteract = Class({__includes=Action, init = function (self, entity, target)
      Action.init(self, 1, false, 'interact')
      self.entity = entity
      self.target = target
      self.state = STATE_MOVING
end})

function ActionInteract:OnMoveComplete()
  self.state = STATE_REQUEST_INTERACT
  self.entity.combat:Attack(self.target, self.entity.weaponContainer.interactWeapon)
end

function ActionInteract:OnStartRunning()
  self.state = STATE_MOVING
  self.entity.mover:SetGoal(self.target, nil, self.entity.weaponContainer:GetAttackDistance(self.entity.weaponContainer.interactWeapon))
end

function ActionInteract:OnStopRunning()
  if self.state == STATE_MOVING then
    self.entity.mover:Stop()
  end
end

function ActionInteract:Update()
  if self.state == STATE_MOVING and not self.target.interactable then
    return true 
  end
  if self.state == STATE_MOVING and not self.entity.mover:IsHaveGoal() then
    self:OnMoveComplete()
  end
  
  return self.state == STATE_INTERACT and not self.entity.mecanim:CheckStateName("Action")
end

function ActionInteract:OnEvent(data)
  if data == "punch" then            
    self.state = STATE_INTERACT
    if self.target.interactable and self.entity.weaponContainer:CanAttack(self.target)then
      self.target:Message("OnInteract", self.entity)
    end    
  end
end

function ActionInteract:GetPriority()
  return self.state == STATE_MOVING and 1 or 3
end

function ActionInteract:IsContinuous()
  return self.state == STATE_MOVING
end

return ActionInteract