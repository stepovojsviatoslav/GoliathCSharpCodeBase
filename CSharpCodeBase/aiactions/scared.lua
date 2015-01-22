local Class = require 'utils.hump.class'
local AI_ConditionalNode = require 'utils.ailib.aiifnode'
local AISequenceNode = require 'utils.ailib.aisequence'
local AI_GetTarget = require 'aiactions.ai_gettarget'
local AI_RunawayFromTarget = require 'aiactions.ai_runawayfromtarget'

local AI_Scared = Class({__includes=AISequenceNode, init = function (self, entity)
      local childNodes = {
        AI_ConditionalNode(entity, function (entity)
            return #entity.relationship:GetTagTypes("scary") > 0
        end),
        AI_GetTarget(entity),
        AI_RunawayFromTarget(entity),
      }
      AISequenceNode.init(self, entity, childNodes)
end})

return AI_Scared