local Class = require 'utils.hump.class'
local AINode = require 'utils.ailib.ainode'
local AISequenceNode = require 'utils.ailib.aisequence'
local AI_SelectFood = require 'aiactions.ai_selectfood'
local AI_MoveToEnemy = require 'aiactions.ai_movetoenemy'
local AI_InteractEnemy = require 'aiactions.ai_interactenemy'

local AI_Fooding = Class({__includes=AISequenceNode, init = function (self, entity)
      local childNodes = {
        AI_SelectFood(entity),
        AI_MoveToEnemy(entity),
        AI_InteractEnemy(entity),
      }
      AISequenceNode.init(self, entity, childNodes)
end})

return AI_Fooding