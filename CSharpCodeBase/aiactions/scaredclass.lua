local Class = require 'utils.hump.class'
local AI_ConditionalNode = require 'utils.ailib.aiifnode'
local AISequenceNode = require 'utils.ailib.aisequence'
local AI_GetTargetClass = require 'aiactions.ai_gettargetclass'
local AI_RunawayFromTarget = require 'aiactions.ai_runawayfromtarget'

local AI_ScaredClass = Class({__includes=AISequenceNode, init = function (self, entity)
      local childNodes = {
        AI_GetTargetClass(entity),
        AI_RunawayFromTarget(entity),
      }
      AISequenceNode.init(self, entity, childNodes)
end})

return AI_ScaredClass