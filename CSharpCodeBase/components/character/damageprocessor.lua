local Class = require 'utils.hump.class'
local Component = require 'components.component'

local DamageProcessorComponent = Class({__includes=Component, init = function (self, entity)
      Component.init(self, entity)
      self.entity.damageProcessor = self
end})

function DamageProcessorComponent:SendDamage(target, damage)
  local damageData = {
    target=target,
    damage=damage.damage,
    effects=damage.effects,
    source=self.entity,
  }
  target:Message('OnDamageReceive', damageData)
end

return DamageProcessorComponent