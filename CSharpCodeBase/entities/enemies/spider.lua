local Class = require 'utils.hump.class'
local EnemyEntity = require 'entities.enemies.enemy'

local AI_Sleep = require 'aiactions.sleep'
local AI_Wander = require 'aiactions.wander'
local AI_ReturnHome = require 'aiactions.returnhome'
local AI_AttackEnemies = require 'aiactions.attackenemies'
local AI_ScanEnemies = require 'aiactions.scanenemies'
local AI_TargetZone = require 'aiactions.targetzone'
local AI_Child = require 'aiactions.child'
local AI_Scared = require 'aiactions.scared'
local AI_Scream = require 'aiactions.ai_scream'
local AI_Fooding = require 'aiactions.ai_fooding'
local AITree = require 'utils.ailib.aitree'
local AISelectorNode = require 'utils.ailib.aiselector'
local AISequenceNode = require 'utils.ailib.aisequence'
local AI_ConditionalNode = require 'utils.ailib.aiifnode'

local AI_SelectEnemy = require 'aiactions.ai_selectenemy'
local AI_MoveToEnemy = require 'aiactions.ai_movetoenemy'
local AI_AttackEnemy = require 'aiactions.ai_attackenemy'
local AI_SelectWeapon = require 'aiactions.ai_selectweapon'
local AI_Timeout = require 'aiactions.ai_timeout'

local AI_AttackEnemiesSpider = Class({__includes=AISequenceNode, init = function (self, entity)
      local childNodes = {
        AI_SelectEnemy(entity),
        AI_ConditionalNode(entity, function (entity, self)
            if entity:GetEffectiveDistance(self.parent.selected_target) > self.entity.config:Get('throwMinDistance') then
              self.entity.weaponContainer:SetWeapon('spider_throw')
            else
              local nextWeapon = self.entity.weaponSequencer:GetNextWeapon()
              self.entity.weaponContainer:SetWeapon(nextWeapon)            
            end
            return true
        end),        
        AI_MoveToEnemy(entity),
        AI_Timeout(entity, entity.weaponContainer:GetAttackPrepareTimeout()),
        AI_AttackEnemy(entity),
      }
      AISequenceNode.init(self, entity, childNodes)
end})

local ActionMachine = require 'components.controlsystem.actionmachine'
local ActionBehaviour = require 'components.controlsystem.enemyactions.actionbehaviour'
local ActionHit = require 'components.controlsystem.enemyactions.actionhit'
local ActionPush = require 'components.controlsystem.enemyactions.actionpush'

local SpiderEntity = Class({__includes=EnemyEntity, init=function(self, gameObject)
      EnemyEntity.init(self, gameObject)
      
      local root = AISelectorNode(self, {
          AI_Sleep(self, {GameController.daytime.PHASE_MORNING, GameController.daytime.PHASE_DAY_START, GameController.daytime.PHASE_DAY_PROGRESS, GameController.daytime.PHASE_DAY_END}),          
          AI_Scream(self),
          AI_Scared(self),
          AISequenceNode(self, {
              AI_ConditionalNode(self, function (entity)
                  return entity.starvation:GetState() and entity:HasEmptyHitlist()
              end),
              AI_Fooding(self),
          }),
          AISequenceNode(self, {
              AI_ConditionalNode(self, function (entity)
                  return entity.health:GetState() ~= 'low'
              end),
              AI_ScanEnemies(self),
          }),
          AI_AttackEnemiesSpider(self),
          AISequenceNode(self, {
              AI_ConditionalNode(self, function (entity)
                  return entity.health:GetState() ~= 'low'
              end),
              AI_TargetZone(self),
          }),          
          --AI_Child(self),
          AI_ReturnHome(self),
          AI_Wander(self), 
      }, true)
      self.actionMachine = ActionMachine(ActionBehaviour(self, AITree(self, root)))
      self.remoter = math.chance(50)
      self.weaponContainer.remoteWeapon = self.weaponContainer:LoadWeapon('spider_throw')
end})

function SpiderEntity:Update()
  EnemyEntity.Update(self)
  if not self.isDeath then
    self.actionMachine:Update()
  end
end

function SpiderEntity:FixedUpdate()
  EnemyEntity.FixedUpdate(self)
  if not self.isDeath then
    self.actionMachine:FixedUpdate()
  end
end

function SpiderEntity:Hit(damageData)
  EnemyEntity.Hit(self, damageData)
  if damageData.summary > 0 and not self.isSleeping then
    self.actionMachine:PushAction(ActionHit(self))
  end
end

function SpiderEntity:Pushed(damageData)
  --local dv = self:GetPosition() - targetPosition
  --dv.y = 0
  if damageData.summary > 0 then
    self.actionMachine:PushAction(ActionPush(self, damageData.source:GetPosition()))
  end
end

function SpiderEntity:OnCollisionEnter(target)
  if self.isSleeping then
    self.relationship:AddInstance("enemy", target)
    self:WakeUp()
  end
end
  
function SpiderEntity:OnEvent(data)
  EnemyEntity.OnEvent(self, data)
  local state = self.actionMachine:GetCurrentAction()
  if state ~= nil then
    state:OnEvent(data)
  end  
end

return SpiderEntity