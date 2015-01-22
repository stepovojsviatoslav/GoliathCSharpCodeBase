local Class = require 'utils.hump.class'
local Timer = require 'utils.hump.timer'
local AINode = require 'utils.ailib.ainode'

local AI_Avoidance = Class({__includes=AINode, init = function (self, entity)
  AINode.init(self, entity)
end})

function AI_Avoidance:Visit()
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

function AI_Avoidance:Moving()
  self.entity.mover:ResetSpeed()
  local tmp = self.entity:GetPosition() - self.target:GetPosition()
  self.entity.mover:SetInput(tmp , 0, true)
end

return AI_Avoidance