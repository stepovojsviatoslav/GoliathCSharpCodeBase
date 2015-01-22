local Class = require 'utils.hump.class'
local EnemyEntity = require 'entities.enemies.enemy'

local AI_Sleep = require 'aiactions.sleep'
local AI_Wander = require 'aiactions.wander'
local AI_ReturnHome = require 'aiactions.returnhome'
local AI_AttackEnemies = require 'aiactions.attackenemies'
local AI_ScanEnemies = require 'aiactions.scanenemies'
local AI_Child = require 'aiactions.child'
local AI_TargetZone = require 'aiactions.targetzone'
local AI_Scared = require 'aiactions.scared'
local AI_Scream = require 'aiactions.ai_scream'
local AI_Fooding = require 'aiactions.ai_fooding'
local AITree = require 'utils.ailib.aitree'
local AISelectorNode = require 'utils.ailib.aiselector'
local AISequenceNode = require 'utils.ailib.aisequence'
local AI_ConditionalNode = require 'utils.ailib.aiifnode'
local AI_ScaredClass = require 'aiactions.scaredclass'
local AI_TargetSearch = require 'aiactions.snowtroll.ai_target_search'
local AI_Following = require 'aiactions.snowtroll.ai_following'
local AI_InviteToHunt = require 'aiactions.snowtroll.ai_invite_to_hunt'

local ActionMachine = require 'components.controlsystem.actionmachine'
local ActionBehaviour = require 'components.controlsystem.enemyactions.actionbehaviour'
local ActionHit = require 'components.controlsystem.enemyactions.actionhit'
local ActionPush = require 'components.controlsystem.enemyactions.actionpush'

local friendlyTrolls = {}

local AI_ScreamToInvite = Class({__includes=AISequenceNode, init = function (self, entity)
      local childNodes = {
        AI_ConditionalNode(entity, function (entity) 
            entity.screamTimeOut = entity.screamTimeOut - GameController.deltaTime
            if entity.screamTimeOut <= 0 then
              entity.screamTimeOut = entity.config:Get('inviteScreamTimeout')
              return true
            else
              return false
            end
        end),
        AI_InviteToHunt(entity),
      }
      AISequenceNode.init(self, entity, childNodes)
end})


local AI_FollowTarget = Class({__includes=AISequenceNode, init = function (self, entity)
      local childNodes = {
        AI_ConditionalNode(entity, function (entity) return entity.health:GetState() ~= 'low' end), 
        AI_ConditionalNode(entity, function (entity) return entity:HasEmptyHitlist() end), 
        AI_ConditionalNode(entity, function (entity) return entity.goalProsecution ~= nil and entity.lead == entity end), 
        AI_Following(entity, entity.config:Get('minDistanceTarget'), entity.config:Get('maxDistanceTarget'), function (entity) return entity.goalProsecution end),
        AI_ScreamToInvite(entity),
        AI_ConditionalNode(entity, function (entity) 
            entity.attackTimeOut = entity.attackTimeOut - GameController.deltaTime
            if entity.attackTimeOut <= 0 then
              entity:AddTargetToHitList(entity.goalProsecution)
              return true
            else
              return false
            end
        end),
      }
      AISequenceNode.init(self, entity, childNodes)
end})

local AI_FollowLead = Class({__includes=AISequenceNode, init = function (self, entity)
      local childNodes = {
        AI_ConditionalNode(entity, function (entity) return entity.health:GetState() ~= 'low' end), 
        AI_ConditionalNode(entity, function (entity) return entity:HasEmptyHitlist() end), 
        AI_ConditionalNode(entity, function (entity) return entity.goalProsecution ~= nil and entity.lead ~= entity end), 
        AI_Following(entity, entity.config:Get('minDistanceTarget'), 3, 
          function (entity) return entity.goalProsecution end, 
          function (entity) return entity.lead end
        ),
        AI_ScreamToInvite(entity),
        AI_ConditionalNode(entity, function (entity) 
            entity.attackTimeOut = entity.attackTimeOut - GameController.deltaTime
            if entity.attackTimeOut <= 0 then
              entity:AddTargetToHitList(entity.goalProsecution)
              return true
            else
              return false
            end
        end),
      }
      AISequenceNode.init(self, entity, childNodes)
end})

local SnowTroll = Class({__includes=EnemyEntity, init=function(self, gameObject)
      EnemyEntity.init(self, gameObject)
      self.screamTimeOut = self.config:Get('inviteScreamTimeout')
      self.attackTimeOut = self.config:Get('attackTimeout')
      local root = AISelectorNode(self, {
          AI_Sleep(self),      
          AISequenceNode(self, {
              AI_ConditionalNode(self, function (entity) return entity.health:GetState() ~= 'low' end), 
              AI_ConditionalNode(self, function (entity) return entity:HasEmptyHitlist() end), 
              AI_ConditionalNode(self, function (entity) return entity.goalProsecution == nil end), 
              AI_TargetSearch(self, function (entity) 
                  table.insert(friendlyTrolls, entity)
              end),
          }),
          AI_FollowTarget(self),
          AI_FollowLead(self),
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
                  return entity.health:GetState() ~= 'low' and entity.goalProsecution == nil
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
          AI_Wander(self), 
      }, true)
      self.actionMachine = ActionMachine(ActionBehaviour(self, AITree(self, root)))
end})

function SnowTroll:Update()
  EnemyEntity.Update(self)
  if not self.isDeath then
    self.actionMachine:Update()
  end
  
  if #friendlyTrolls > 2 then
    self:AddTargetToHitList()
    for k, v in pairs(friendlyTrolls) do
      v:AddTargetToHitList(self.goalProsecution)
    end
    friendlyTrolls = {}
  end
  
  if not self:HasEmptyHitlist() then
    self:AttackTargetByOtherTrolls()
  end
end

function SnowTroll:FixedUpdate()
  EnemyEntity.FixedUpdate(self)
  if not self.isDeath then
    self.actionMachine:FixedUpdate()
  end
end

function SnowTroll:Hit(damageData)
  EnemyEntity.Hit(self, damageData)
  if damageData.summary > 0 and damageData.source.characterClass >= self.characterClass then
    self.actionMachine:PushAction(ActionHit(self))
  end
end

function SnowTroll:Pushed(damageData)
  --local dv = self:GetPosition() - targetPosition
  --dv.y = 0
  if damageData.summary > 0 and damageData.source.characterClass >= self.characterClass then
    self.actionMachine:PushAction(ActionPush(self, damageData.source:GetPosition()))
  end
end

function SnowTroll:OnEvent(data)
  EnemyEntity.OnEvent(self, data)
  local state = self.actionMachine:GetCurrentAction()
  if state ~= nil then
    state:OnEvent(data)
  end  
end

function SnowTroll:OnCollisionEnter(targetEntity)
  self:Message('OnCollisionEnter', targetEntity)
end

function SnowTroll:Death()
  EnemyEntity.Death(self)
  Transform.SetColliderState(self.transform, false)
  self.lead = nil
  self.goalProsecution = nil
end

function SnowTroll:Respawn()
  if EnemyEntity.Respawn(self) then
    Transform.SetColliderState(self.transform, true)
    return true
  else
    return false
  end
end

function SnowTroll:AddTargetToHitList(goalProsecution)
  self.relationship:AddInstance("enemy", goalProsecution)
end

function SnowTroll:InviteToHunt()
  local friends = self.relationship:GetTagTypes("friend")
  if friends ~= nil and #friends > 0 then
    local friendEntities = RaycastUtils.GetEntitiesInRadiusByTypes(self:GetPosition(), self.config:Get('inviteScreamRadius'), friends)
    for k, v in pairs(friendEntities) do
      if v ~= self then
        if math.chance(self.config:Get('inviteScreamChance')) then
          v.lead = self
          v.goalProsecution = self.goalProsecution
          table.insert(friendlyTrolls, v)
        end
      end
    end
  end
end

function SnowTroll:AttackTargetByOtherTrolls()
  local friends = self.relationship:GetTagTypes("friend")
  if friends ~= nil and #friends > 0 then
    local friendEntities = RaycastUtils.GetEntitiesInRadiusByTypes(self:GetPosition(), self.config:Get('inviteScreamRadius'), friends)
    for k, v in pairs(friendEntities) do
      if v ~= self then
        v:AddTargetToHitList(self.goalProsecution)
      end
    end
  end
end

return SnowTroll