local Class = require 'utils.hump.class'
local EnemyEntity = require 'entities.enemies.enemy'
local FragileThingComponent = require 'components.thing.fragilethingcomponent'

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
local AI_ScaredClass = require 'aiactions.scaredclass'

local ActionMachine = require 'components.controlsystem.actionmachine'
local ActionBehaviour = require 'components.controlsystem.enemyactions.actionbehaviour'
local ActionHit = require 'components.controlsystem.enemyactions.actionhit'
local ActionPush = require 'components.controlsystem.enemyactions.actionpush'

local DrontEntity = Class({__includes=EnemyEntity, init=function(self, gameObject)
      EnemyEntity.init(self, gameObject)
      
      local root = AISelectorNode(self, {
          AI_Sleep(self),          
          AI_Scream(self),
          AI_Scared(self),
          AI_ScaredClass(self),
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
          AI_AttackEnemies(self),
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
      self:AddComponent(FragileThingComponent)
end})

function DrontEntity:Update()
  EnemyEntity.Update(self)
  if not self.isDeath then
    self.actionMachine:Update()
  end
end

function DrontEntity:FixedUpdate()
  EnemyEntity.FixedUpdate(self)
  if not self.isDeath then
    self.actionMachine:FixedUpdate()
  end
end

function DrontEntity:Hit(damageData)
  EnemyEntity.Hit(self, damageData)
  if damageData.summary > 0 and damageData.source.characterClass >= self.characterClass then
    self.actionMachine:PushAction(ActionHit(self))
  end
end

function DrontEntity:Pushed(damageData)
  --local dv = self:GetPosition() - targetPosition
  --dv.y = 0
  if damageData.summary > 0 and damageData.source.characterClass >= self.characterClass then
    self.actionMachine:PushAction(ActionPush(self, damageData.source:GetPosition()))
  end
end

function DrontEntity:OnEvent(data)
  EnemyEntity.OnEvent(self, data)
  local state = self.actionMachine:GetCurrentAction()
  if state ~= nil then
    state:OnEvent(data)
  end  
end

function DrontEntity:OnFragile()
  if not self.isDeath then
    self:Death()
  end
end

function DrontEntity:OnCollisionEnter(targetEntity)
  self:Message('OnCollisionEnter', targetEntity)
end

function DrontEntity:Death()
  EnemyEntity.Death(self)
  Transform.SetColliderState(self.transform, false)
end

function DrontEntity:Respawn()
  if EnemyEntity.Respawn(self) then
    Transform.SetColliderState(self.transform, true)
    return true
  else
    return false
  end
end

return DrontEntity