local Class = require 'utils.hump.class'
local EnemyEntity = require 'entities.enemies.enemy'

local AI_Sleep = require 'aiactions.sleep'
local AI_Wander = require 'aiactions.wander'
local AI_ReturnHome = require 'aiactions.returnhome'
local AI_AttackEnemies = require 'aiactions.attackenemies'
local AI_SelectEnemy = require 'aiactions.ai_selectenemy'
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
local AINode = require 'utils.ailib.ainode'

local ActionMachine = require 'components.controlsystem.actionmachine'
local ActionBehaviour = require 'components.controlsystem.enemyactions.actionbehaviour'
local ActionHit = require 'components.controlsystem.enemyactions.actionhit'
local ActionPush = require 'components.controlsystem.enemyactions.actionpush'

-- Custom AI cases
local AI_PlayAnimationStateAndLookAtTarget = Class({__includes=AINode, init=function(self, entity, state)
  AINode.init(self, entity)
  self.state = state
end})

function AI_PlayAnimationStateAndLookAtTarget:Visit()
  if self.status == NODE_READY then
    self.status = NODE_RUNNING
    self.entity.mecanim:ForceSetState(self.state)
    self.isAnimationPlaying = false
    self.entity.mover:Stop()
  end
  if self.status == NODE_RUNNING then
    if self.parent.selected_target ~= nil then
      self.entity.mover:LookAt(self.parent.selected_target:GetPosition())
    end
    local statePlaying = self.entity.mecanim:CheckStateName(self.state)
    if not self.isAnimationPlaying and statePlaying then
      self.isAnimationPlaying = true
    end
    if self.isAnimationPlaying and not statePlaying then
      self.status = NODE_SUCCESS
    end
  end
  return self.status
end

local AI_RunToTarget = Class({__includes=AINode, init=function(self, entity)
  AINode.init(self, entity)
  self.toDistance = self.entity.config:Get('angryLockTargetDistance') or 3
end})

function AI_RunToTarget:Visit()
  if self.status == NODE_READY then
    self.status = NODE_RUNNING
  end
  if self.status == NODE_RUNNING then
    if self.entity:GetEffectiveDistance(self.parent.selected_target) < self.toDistance or not self.parent.selected_target.interactable then
      self.status = NODE_SUCCESS
    else
      self.entity.mover._useLoa = false
      self.entity.mover:SetSpeed(self.entity.config:Get("angryRunSpeed"))
      self.entity.mover:SetInputToVec(self.parent.selected_target:GetPosition(), false, self.entity.config:Get("angryRunType"), true)
    end
  end
  return self.status
end

local AI_RunToVector = Class({__includes=AINode, init=function(self, entity)
  AINode.init(self, entity)
end})

function AI_RunToVector:Visit()
  if self.status == NODE_READY then
    self.status = NODE_RUNNING
    self.vector = self.parent.selected_target:GetPosition() - self.entity:GetPosition()
    self.parent.vector = self.vector
    self.maxDistance = (self.entity.config:Get('angryLockTargetDistance') or 3) * 1.2
  end
  if self.status == NODE_RUNNING then
    local distance = self.entity:GetEffectiveDistance(self.parent.selected_target)
    if distance > self.maxDistance or not self.parent.selected_target.interactable then
      self.parent.faststop = false
      self.status = NODE_SUCCESS
    elseif distance < self.entity.config:Get('angryDistancePunch') then
      -- punch!
      self.entity.weaponContainer.pushWeapon:Attack(self.parent.selected_target)
      self.parent.faststop = true
      self.status = NODE_SUCCESS
    else
      self.entity.mover:SetSpeed(self.entity.config:Get("angryRunSpeed"))
      self.entity.mover:SetInput(self.vector, self.entity.config:Get('angryRunType'), true)
    end
  end
  return self.status
end

local AI_Stop = Class({__includes=AINode, init=function(self, entity)
  AINode.init(self, entity)
end})

function AI_Stop:Visit()
  if self.status == NODE_READY then
    self.entity.angryState = false
    self.status = NODE_RUNNING
    self.vector = self.parent.vector
    self.vector:Normalize()
    self.entity.mover:Stop()
    if self.parent.faststop then
      self.entity.mecanim:ForceSetState('RunStopFast')
    else
      self.entity.mecanim:ForceSetState('RunStop')
    end
    self.isAnimationStarted = false
  end
  if self.status == NODE_RUNNING then
    local animatorState = self.entity.mecanim:CheckStateName('RunStop') or self.entity.mecanim:CheckStateName('RunStopFast')
    
    if animatorState and not self.isAnimationStarted then
      self.isAnimationStarted = true
    end
    
    if self.isAnimationStarted and not animatorState then
      self.status = NODE_SUCCESS
      self.entity.mover:Stop()
    end
    
    local curveValue = self.entity.mecanim:GetFloat('speed_curve')
    RigidbodyUtils.Move(self.entity.rigidbody, self.vector * self.entity.config:Get("angryRunSpeed") * curveValue)
  end
  return self.status
end

local AI_AngryRun = Class({__includes=AISequenceNode, init = function (self, entity)
      local childNodes = {
        AI_SelectEnemy(entity),
        AI_ConditionalNode(entity, function (entity, self)
            local dst = self.entity:GetEffectiveDistance(self.parent.selected_target)
            local result = dst > self.entity.config:Get('angryRunMinDistance') and dst < self.entity.config:Get('angryRunMaxDistance')
            self.entity.angryState = result
            return result
        end),
        AI_PlayAnimationStateAndLookAtTarget(entity, "RunPrepare"), -- prepare to run
        AI_RunToTarget(entity),
        AI_RunToVector(entity),
        AI_Stop(entity)
      }
      AISequenceNode.init(self, entity, childNodes)
end})

-- Class description
local HornerEntity = Class({__includes=EnemyEntity, init=function(self, gameObject)
      EnemyEntity.init(self, gameObject)
      
      local root = AISelectorNode(self, {
          AI_Scream(self),
          AI_Scared(self),
          AI_AngryRun(self),
          AISequenceNode(self, {
              AI_ConditionalNode(self, function (entity)
                  return entity.starvation:GetState() and entity:HasEmptyHitlist()
              end),
              AI_Fooding(self),
          }),
          AI_Sleep(self),
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
      self.weaponContainer.pushWeapon = self.weaponContainer:LoadWeapon(self.config:Get('pushWeapon'))
end})

function HornerEntity:Update()
  EnemyEntity.Update(self)
  if not self.isDeath then
    self.actionMachine:Update()
  end
end

function HornerEntity:FixedUpdate()
  EnemyEntity.FixedUpdate(self)
  if not self.isDeath then
    self.actionMachine:FixedUpdate()
  end
end

function HornerEntity:Hit(damageData)
  EnemyEntity.Hit(self, damageData)
  if damageData.summary > 0 and damageData.source.characterClass >= self.characterClass then
    self.actionMachine:PushAction(ActionHit(self))
  end
end

function HornerEntity:Pushed(damageData)
  --local dv = self:GetPosition() - targetPosition
  --dv.y = 0
  if damageData.summary > 0 and damageData.source.characterClass >= self.characterClass then
    self.actionMachine:PushAction(ActionPush(self, damageData.source:GetPosition()))
  end
end

function HornerEntity:OnEvent(data)
  EnemyEntity.OnEvent(self, data)
  local state = self.actionMachine:GetCurrentAction()
  if state ~= nil then
    state:OnEvent(data)
  end  
end

function HornerEntity:OnCollisionEnter(target)
  if self.angryState then
    --print("Collision with " ..target.name)
    if target.OnFragileForce then
      target:OnFragileForce({source=self})
    end
  end
end

return HornerEntity