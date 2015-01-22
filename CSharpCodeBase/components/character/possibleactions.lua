local Class = require 'utils.hump.class'
local Component = require 'components.component'

local PossibleActions = Class({__includes=Component, init=function(self, entity)
  Component.init(self, entity)
  self.actions = {}
  self.defaultAction = 'attack'
  self:LoadActions()
  self.entity.possibleActions = self
end})

function PossibleActions:LoadActions()
  self.actions = self.entity.config:Get('action') or {}
end

function PossibleActions:GetAction(idx)
  if idx < 1 then
    return self.actions[1] or self.defaultAction
  elseif idx > #self.actions then
    return self.actions[#self.actions] or self.defaultAction
  else
    return self.actions[idx] or self.defaultAction
  end
end

function PossibleActions:SetActions(actions)
  self.actions = actions
end

return PossibleActions