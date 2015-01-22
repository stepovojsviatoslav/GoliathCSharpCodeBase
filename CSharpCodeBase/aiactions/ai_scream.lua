local Class = require 'utils.hump.class'
local AINode = require 'utils.ailib.ainode'

-- Select enemy using ai vision
local AI_Scream = Class({__includes=AINode, init=function(self, entity)
  AINode.init(self, entity)
end})

function AI_Scream:Visit()
  if self.status == NODE_READY then
    if self.entity._screamTrigger then
      self.entity._screamTrigger = false
      self.status = NODE_RUNNING
      self.entity.mover:Stop()
      if self.entity.mecanim:CheckStateName("action") then
        self.entity.mecanim:SetFloat('action_type', 3)
        self.entity.mecanim:ForceSetState('action')
      else
        self.entity.mecanim:SetFloat('action_type', 3)
        self.entity.mecanim:SetTrigger('action')
      end
      self.isAnimStarted = false
    else
      self.status = NODE_FAILURE
    end
  end
  
  if self.status == NODE_RUNNING then
    self.entity.mover:LookAt(self.entity._screamTarget:GetPosition())
    if not self.isAnimStarted then
      self.isAnimStarted = self.entity.mecanim:CheckStateName("Action")
    end
    if self.isAnimStarted and not self.entity.mecanim:CheckStateName("Action") then
      self.status = NODE_SUCCESS
    end
  end
  return self.status
end

return AI_Scream