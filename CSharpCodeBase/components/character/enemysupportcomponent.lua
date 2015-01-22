local Class = require 'utils.hump.class'
local Component = require 'components.component'

local EnemySupportComponent = Class({__includes=Component, init=function(self, entity)
      Component.init(self, entity)
end})

function EnemySupportComponent:OnFriendAttacked(data)
  self.entity:WakeUp()
  if not self.entity.relationship:HasInstance("enemy", data.target) then
    if math.chance(self.entity.config:Get('supportScreamChance')) then
      self.entity._screamTrigger = true
      self.entity._screamTarget = data.target
    end
  end
  self.entity.relationship:AddInstance("enemy", data.target)
end

return EnemySupportComponent