local Action = require 'components.controlsystem.action'
local Class = require 'utils.hump.class'

local ActionBehaviour = Class({__includes=Action, init = function (self, entity, tree)
      Action.init(self, 0, true, 'behaviour')
      self.entity = entity
      self.tree = tree
end})

function ActionBehaviour:Update()
  self.tree:Process()
  return false
end

function ActionBehaviour:OnStopRunning()
  self.tree:Reset()
end

function ActionBehaviour:OnEvent(data)
  self.tree:OnEvent(data)
end


return ActionBehaviour