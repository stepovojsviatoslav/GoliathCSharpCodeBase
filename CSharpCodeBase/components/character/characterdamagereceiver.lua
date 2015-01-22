local Class = require 'utils.hump.class'
local Component = require 'components.component'

local CharacterDamageReceiverComponent = Class({__includes=Component, init=function(self, entity)
      Component.init(self, entity)
end})

function CharacterDamageReceiverComponent:OnApplyDamage(damageData)
  if self.entity.WakeUp then
    self.entity:WakeUp()
  end
  local isDeath = self.entity.health:Decrease(damageData.summary) == 0
  
  if not isDeath then
    if self.entity.relationship ~= nil then
      self.entity.relationship:AddInstance("enemy", damageData.source)
    end
    if damageData.effects.punch == 'push' then
      print("Pushed!")
      if self.entity.Pushed then
        self.entity:Pushed(damageData)--.source:GetPosition())
      end
    else
      if self.entity.Hit then
        self.entity:Hit(damageData)
      end
    end
  else
    -- death
    if self.entity.Death then
      self.entity:Death()
    end
    --print("Enemy is death!")
  end
end

return CharacterDamageReceiverComponent