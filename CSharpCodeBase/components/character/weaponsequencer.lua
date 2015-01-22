local Class = require 'utils.hump.class'
local Component = require 'components.component'

local WeaponSequencerComponent = Class({__includes=Component, init=function(self, entity)
      Component.init(self, entity)
      self.index = 1
      self.weapons = self.entity.config:Get('weaponSequence') or {}
      self.entity.weaponSequencer = self
end})

function WeaponSequencerComponent:GetNextWeapon()
  if #self.weapons == 0 then
    return nil
  end
  local result = self.index
  self.index = self.index + 1
  if self.index > #self.weapons then
    self.index = 1
  end
  return self.weapons[result]
end

return WeaponSequencerComponent