local Class = require 'utils.hump.class'
local AINode = require 'utils.ailib.ainode'
local AISequenceNode = require 'utils.ailib.aisequence'
local AI_ConditionalNode = require 'utils.ailib.aiifnode'
local AI_FindParent = require 'aiactions.ai_findparent'
local AI_FollowParent = require 'aiactions.ai_followparent'

local AI_Child = Class({__includes=AISequenceNode, init = function (self, entity)
      local childNodes = {
        AI_ConditionalNode(entity, function (entity)
            if not entity.isChild then return false end
            local parent = entity.relationship:GetInstance("parent")
            local safeRadius = entity.config:Get("parentSafeRadiusMax")
            return parent == nil or entity:GetSimpleDistance(parent) > safeRadius
        end),
        AI_FindParent(entity),
        AI_FollowParent(entity),
      }
      AISequenceNode.init(self, entity, childNodes)
end})

return AI_Child