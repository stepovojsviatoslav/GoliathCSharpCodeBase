local Class = require 'utils.hump.class'
local Timer = require 'utils.hump.timer'
local AINode = require 'utils.ailib.ainode'

-- Move to enemy
local AI_MoveToEnemy = Class({__includes=AINode, init = function (self, entity)
  AINode.init(self, entity)
  self.timer = Timer(self.entity.config:Get("chaseTimeout"))
end})

function AI_MoveToEnemy:Visit()
  if self.status == NODE_READY then
    self.status = NODE_RUNNING
    self.entity.mover:ResetSpeed()
    self.entity.mover:SetGoal(self.parent.selected_target, nil, self.entity.weaponContainer:GetAttackDistance())
    self.timer:Reset()
  elseif self.status == NODE_RUNNING then
    if not self.parent.selected_target.interactable then
      self.status = NODE_FAILURE
      return
    end
    if not self.entity.mover:IsHaveGoal() then
      self.status = NODE_SUCCESS
    else
      if self.timer:Tick() then
        self.status = NODE_FAILURE
      end
    end
  end
  return self.status
end

return AI_MoveToEnemy