--[[
Base component-system model, partially maped from unity
]]

local Class = require 'utils.hump.class'

local Component = Class({init = function (self, entity)
      self.entity = entity      
      self.transform = self.entity.transform
end})

function Component:Awake() end
function Component:Update() end
function Component:FixedUpdate() end
function Component:LateUpdate() end

return Component