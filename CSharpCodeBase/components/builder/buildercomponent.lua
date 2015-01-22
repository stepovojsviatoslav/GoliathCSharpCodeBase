local Class = require 'utils.hump.class'
local Component = require 'components.component'

local BuilderComponent = Class({__includes=Component, init=function(self, entity)
      Component.init(self, entity)
      self.entity.builder = self
      self.components = self.entity.config:Get('component')
      
      if self.components then
        for key, value in pairs(self.components) do
          self.entity:AddComponent(require(value))
        end
      end
end})

return BuilderComponent