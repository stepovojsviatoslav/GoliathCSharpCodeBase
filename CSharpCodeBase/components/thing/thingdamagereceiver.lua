local Class = require 'utils.hump.class'
local Component = require 'components.component'

local ThingDamageReceiverComponent = Class({__includes=Component, init=function(self, entity)
      Component.init(self, entity)
end})

function ThingDamageReceiverComponent:OnApplyDamage(damageData)
  local isDeath = self.entity.health:Decrease(damageData.summary) == 0
  if not isDeath then
    self.entity:Hit(damageData)
  else
    self.entity:DestroyThing(damageData)
  end
end

return ThingDamageReceiverComponent