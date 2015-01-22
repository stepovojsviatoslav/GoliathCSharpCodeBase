local Class = require 'utils.hump.class'
local Vector3 = require 'utils.hump.vector3'
local AINode = require 'utils.ailib.ainode'

local AI_Wander = Class({__includes=AINode, init = function (self, entity)
  AINode.init(self, entity)
  self.wanderPosition = nil 
  self.timeout = nil
  self.wanderRadius = self.entity.config:Get("wanderRadius")
end})

function AI_Wander:Visit()
  if self.status == NODE_READY then
    -- Choose new position
    self:NewPosition()
    self.status = NODE_RUNNING
    self.entity.mover:ResetSpeed()
    self.entity.mover:SetGoal(self.wanderPosition)
  elseif self.status == NODE_RUNNING then
    if not self.entity.mover:IsHaveGoal() then
      if self.idleTimeout > 0 then
        self.idleTimeout = self.idleTimeout - GameController.deltaTime
        if self.idleTimeout <= 0 then
          self.entity.mecanim:SetFloat('idle_type', math.random(0, self.entity.config:Get('wanderIdleTypes')))
          self.entity.mecanim:SetTrigger('idle')
        end
      end
      if self.timeout > 0 then
        self.timeout = self.timeout - GameController.deltaTime
      else
        self.status = NODE_SUCCESS
      end
    end
  end
  return self.status
end

function AI_Wander:NewPosition()
  local aroundPosition = self.entity.worldmap:GetLocation('home')
  if self.entity.isChild and self.entity.relationship:GetInstance('parent') ~= nil then
    aroundPosition = self.entity.relationship:GetInstance('parent'):GetPosition()
  end
  self.wanderPosition = aroundPosition
    + Vector3(math.random(-self.wanderRadius, self.wanderRadius), 
        0,
        math.random(-self.wanderRadius, self.wanderRadius))
  self.timeout = math.random(self.entity.config:Get("wanderIdleMin"),
    self.entity.config:Get("wanderIdleMax"))
  self.idleTimeout = math.random(0, self.timeout)
end

return AI_Wander