local Class = require 'utils.hump.class'
local AINode = require 'utils.ailib.ainode'
local AISequenceNode = require 'utils.ailib.aisequence'
local AI_SelectEnemy = require 'aiactions.ai_selectenemy'
local AI_MoveToEnemy = require 'aiactions.ai_movetoenemy'
local AI_AttackEnemy = require 'aiactions.ai_attackenemy'
local AI_SelectWeapon = require 'aiactions.ai_selectweapon'
local AI_Timeout = require 'aiactions.ai_timeout'

local AI_AttackEnemies = Class({__includes=AISequenceNode, init = function (self, entity)
      local childNodes = {
        AI_SelectEnemy(entity),
        AI_SelectWeapon(entity),
        AI_MoveToEnemy(entity),
        AI_Timeout(entity, entity.weaponContainer:GetAttackPrepareTimeout()),
        AI_AttackEnemy(entity),
      }
      AISequenceNode.init(self, entity, childNodes)
end})

return AI_AttackEnemies