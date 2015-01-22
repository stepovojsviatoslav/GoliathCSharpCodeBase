local Action = require 'components.controlsystem.action'
local Class = require 'utils.hump.class'

local ActionDashPunch = Class({__includes=Action, init = function (self, entity, target)
      Action.init(self, 4, false, 'dashpunch')
      self.entity = entity
      self.target = target
end})

function ActionDashPunch:OnStartRunning()
  self.entity.mover:Stop()
  self.entity.combat:Attack(self.target)
  self.isAnimationStarted = false
  self.jumping = false
  self.canpunch = false
  self.dv = self.target:GetPosition() - self.entity:GetPosition()
  self.dv:Normalize()
end

function ActionDashPunch:Update()
  if not self.isAnimationStarted then
    self.isAnimationStarted = self.entity.mecanim:CheckStateName('Action')
  end
  return self.isAnimationStarted and not self.entity.mecanim:CheckStateName("Action")
end

function ActionDashPunch:FixedUpdate()
  if not self.jumping then
    self.entity.mover:LookAt(self.target:GetPosition())
  end
  local speedCurve = self.entity.mecanim:GetFloat('speed_curve')
  --local dv = self.target:GetPosition() - self.entity:GetPosition()
  --dv:Normalize()
  if self.jumping then
    RigidbodyUtils.MoveNotRotate(self.entity.rigidbody, self.dv * speedCurve * 10)
  end
  if self.canpunch then
    if self.target.interactable and self.entity.weaponContainer:CanAttack(self.target) then
      self.entity.weaponContainer:Attack(self.target)
      --self.entity.damageProcessor:SendDamage(self.target, self.entity.weaponContainer.currentWeapon:GetDamage())
      self.canpunch = false
    end
  end
end


function ActionDashPunch:OnEvent(data)
  if data == "punch" then        
    --if self.target.interactable and self.entity.weaponContainer:CanAttack(self.target) then
    --  self.entity.damageProcessor:SendDamage(self.target, self.entity.weaponContainer.currentWeapon:GetDamage())
    --end
  elseif data == "jumping" then
    self.jumping = true
    self.canpunch = true
    self.dv = self.target:GetPosition() - self.entity:GetPosition()
    self.dv:Normalize()    
  elseif data == "stoppunch" then
    self.canpunch = false
  end
  
end

function ActionDashPunch:OnStopRunning()
  self.entity.mover:Stop()
end

return ActionDashPunch