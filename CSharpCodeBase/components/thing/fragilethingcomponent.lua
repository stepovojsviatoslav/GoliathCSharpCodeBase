local Class = require 'utils.hump.class'
local Component = require 'components.component'

local FragileThingComponent = Class({__includes=Component, init=function(self, entity)
      Component.init(self, entity)
end})

function FragileThingComponent:OnCollisionEnter(targetEntity)
  if targetEntity.characterClass ~= nil and targetEntity.characterClass > 1 then
    self.entity:OnFragile(targetEntity)
  end
end

return FragileThingComponent