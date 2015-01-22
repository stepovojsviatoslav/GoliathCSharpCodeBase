local Class = require 'utils.hump.class'
local Timer = require 'utils.hump.timer'
local AINode = require 'utils.ailib.ainode'

local AI_Following = Class({__includes=AINode, init = function (self, entity)
  AINode.init(self, entity)
  self.minRadius = self.entity.config:Get('minDistanceProsecution')
  self.maxRadius = self.entity.config:Get('maxDistanceProsecution')
end})

function AI_Following:Visit()
  self.target = self.entity.goalProsecution
  if self.target then
    if self.status == NODE_READY then
      self.status = NODE_RUNNING
      self:Moving()
    elseif self.status == NODE_RUNNING then
      if not self.target.interactable then
        self.status = NODE_FAILURE
        return
      end
      if not self.entity.mover:IsHaveGoal() then
        self.status = NODE_SUCCESS
      end
    end
  end
  return self.status
end

function AI_Following:Moving()
  self.entity.mover:ResetSpeed()
  self.entity.mover:LookAt(self.target:GetPosition())
  self.entity.mover:SetGoal(self.target, nil, math.random(self.minRadius, self.maxRadius))
end

return AI_Following