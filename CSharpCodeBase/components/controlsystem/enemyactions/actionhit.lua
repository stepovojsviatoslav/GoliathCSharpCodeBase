local Action = require 'components.controlsystem.action'
local Class = require 'utils.hump.class'

local ActionHit = Class({__includes=Action, init = function (self, entity)
      Action.init(self, 4, false, 'hit')
      self.entity = entity
end})

function ActionHit:OnStartRunning()
  --Action.OnStart(self)
  self.entity.mover:Stop()
  self.entity.mecanim:SetFloat("action_type", 0)
  self.entity.mecanim:ForceSetState("Hit")
  self.isHitStarted = false
end

function ActionHit:Update()
  if not self.isHitStarted then
    self.isHitStarted = self.entity.mecanim:CheckStateName("Hit")
  end
  return self.isHitStarted and not self.entity.mecanim:CheckStateName("Hit")
end

return ActionHit