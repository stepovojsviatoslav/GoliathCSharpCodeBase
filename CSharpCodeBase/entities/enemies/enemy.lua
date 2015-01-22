local Class = require 'utils.hump.class'
local Vector3 = require 'utils.hump.vector3'

local UnityExistsEntity = require 'unity.unityentity'
local MecanimComponent = require 'components.animations.mecanim'
local MoverComponent = require 'components.character.mover'
local CharacterWeaponContainer = require 'components.character.weaponcontainer'
local VisionComponent = require 'components.character.vision'
local AIWorldMapComponent = require 'components.character.aiworldmap'
local RelationshipComponent = require 'components.character.relationship'
local ResistComponent = require 'components.character.resist'
local DamageProcessorComponent = require 'components.character.damageprocessor'
local DamageReceiverComponent = require 'components.character.damagereceiver'
local ConfigComponent = require 'components.services.config'
local PossibleActionsComponent = require 'components.character.possibleactions'
local CharacterDamageReceiver = require 'components.character.characterdamagereceiver'
local EnemySupportComponent = require 'components.character.enemysupportcomponent'
local CombatComponent = require 'components.character.combat'
local DamageVisualizerComponent = require 'components.character.damagevisualizer'
local HealthComponent = require 'components.character.health'
local StarvationComponent = require 'components.character.starvation'
local WeaponSequencerComponent = require 'components.character.weaponsequencer'
local TimeManagerComponent = require 'components.thing.timemanagercomponent'

local AI_Wander = require 'aiactions.wander'
local AI_ReturnHome = require 'aiactions.returnhome'
local AI_AttackEnemies = require 'aiactions.attackenemies'
local AI_ScanEnemies = require 'aiactions.scanenemies'
local AI_TargetZone = require 'aiactions.targetzone'
local AI_Child = require 'aiactions.child'
local AI_Scared = require 'aiactions.scared'
local AITree = require 'utils.ailib.aitree'
local AISelectorNode = require 'utils.ailib.aiselector'

local EnemyEntity = Class({__includes=UnityExistsEntity, init = function (self, gameObject)
      UnityExistsEntity.init(self, gameObject)
      self.config = ConfigComponent('enemies', self.name)
      self.storage = {}
      self.rigidbody = self.gameObject:GetComponent("Rigidbody")
      
      self:AddComponent(HealthComponent)
      self:AddComponent(StarvationComponent)
      self:AddComponent(MecanimComponent)
      self:AddComponent(MoverComponent)
      self:AddComponent(CharacterWeaponContainer)
      self:AddComponent(VisionComponent)
      self:AddComponent(AIWorldMapComponent)
      self:AddComponent(RelationshipComponent)
      self:AddComponent(ResistComponent)
      self:AddComponent(DamageReceiverComponent)
      self:AddComponent(DamageProcessorComponent)
      self:AddComponent(PossibleActionsComponent)
      self:AddComponent(CharacterDamageReceiver)
      self:AddComponent(EnemySupportComponent)
      self:AddComponent(CombatComponent)
      self:AddComponent(DamageVisualizerComponent)
      self:AddComponent(WeaponSequencerComponent)
      self:AddComponent(TimeManagerComponent)
      
      self.isChild = self.config:Get("isChild")
      self.parent = nil
      self.isAgressive = true
      self.static = false
      
      --self.luaMapper:EnableComplexVisibilityControl()
      
      self.isDeath = false
      self.deathTime = 0
      self.deathHideTimer = 0
      self.characterClass = self.config:Get('characterClass') or 1
end})

function EnemyEntity:CanAttack(targetEntity)
  -- check weapon for target entity
  return self.weaponContainer:CanAttack(targetEntity)
end

function EnemyEntity:Update()
  if self.isDeath and self.deathHideTimer > 0 then
    self.deathHideTimer = self.deathHideTimer - GameController.deltaTime
    if self.deathHideTimer <= 0 then
      self:SetPosition(self.worldmap:GetLocation('home') + Vector3(0, -10, 0))
      local homeVec = self.worldmap:GetLocation('home')
      self.luaMapper:SetFrustumSpherePosition(homeVec.x, homeVec.y, homeVec.z)
      self.timeManager:Add('Respawn', 3) --self.config:Get('respawnTimeout') or 3)
    end
  end
  if not self.isDeath then
    UnityExistsEntity.Update(self)
    --[[
  elseif not self.visible then 
    if GameController.gameTime - self.deathTime > (self.config:Get('respawnTimeout') or 3) then
      print("Respawn: " .. self.name)
      self:Respawn()
    end
  ]]
  end
end

function EnemyEntity:Save(storage)
  storage:SetBool("isDeath", self.isDeath)
  storage:SetFloat("deathTime", self.deathTime)
  storage:SetBool("isLua", true)
  storage:SetTransform(self.transform)
  self:Message('Save', storage)
end

function EnemyEntity:Load(storage)
  storage:LoadTransform(self.transform)
  self.isDeath = storage:GetBool("isDeath", false)
  self.deathTime = storage:GetFloat("deathTime", 0)
  self:Message('Load', storage)
  self.mecanim:ForceSetState('Idle')
end

function EnemyEntity.Create(storage, x, y, z)
  storage:SetBool("isLua", true)
  storage:SetPosition(x, y, z)
end

function EnemyEntity:HasEmptyHitlist()
  return self.relationship:GetInstance('enemy') == nil
end

function EnemyEntity:ScanEnemies()
  local targets = self.vision:GetVisibleEntitiesByRSTag("enemy")
  for k, v in pairs(targets) do
    if v.interactable then
      self.relationship:AddInstance("enemy", v)
    end
  end
end

function EnemyEntity:GetPriorityFood()
  local targets = self.vision:GetVisibleEntitiesByRSTag("food")
  if #targets > 0 and targets[1].CanBeFood and targets[1]:CanBeFood() then
    return targets[1]
  end
  return nil
end

function EnemyEntity:GetPriorityTarget()
  local targets = self.relationship:GetInstances("enemy")
  if #targets > 0 then
    for i = #targets, 1, -1 do
      local v = targets[i]
      if v.interactable then
        return v
      end
    end
      --[[
    for k, v in pairs(targets) do
      if v.interactable then
        return v
      end
      --targets[1]
    end
    ]]
    return nil
  else
    return nil
  end
end

function EnemyEntity:Hit(damageData)
  local friends = self.relationship:GetTagTypes("friend")
  if friends ~= nil and #friends > 0 then
    local friendEntities = RaycastUtils.GetEntitiesInRadiusByTypes(self:GetPosition(), self.config:Get('supportRadius'), friends)
    for k, v in pairs(friendEntities) do
      if v ~= self then
        v:Message("OnFriendAttacked", {source=self, target=damageData.source})
      end
    end
  end
end

function EnemyEntity:OnEvent(data)
  UnityExistsEntity.OnEvent(self, data)
end

function EnemyEntity:Message(...)
  local args = {...}
  if #args > 0 then
    if args[1] == 'Respawn' then
      return self:Respawn()
    end
  end
  UnityExistsEntity.Message(self, ...)
end

function EnemyEntity:Respawn()
  if self.visible then 
    return false
  end
  if self.isDeath then
    self:SetPosition(self.worldmap:GetLocation('home'))
    self.interactable = true
    self.isDeath = false
    self.mecanim:ForceSetState("Idle")
    self.luaMapper:ResetFrustumSphere()
    self:Message("OnRespawn")
  end
  return true
end

function EnemyEntity:Death()
  if not self.isDeath then
    self.interactable = false
    self.isDeath = true
    self.mover:Stop()
    self.mecanim:ForceSetState("Death")
    --self:SetPosition(self.worldmap:GetLocation('home') + Vector3(0, -1 - self.height, 0))
    self.deathHideTimer = 3
    self.deathTime = GameController.gameTime
  end
end

function EnemyEntity:Sleep()
  if self.visibilityTime < 0.5 then
    self.mecanim:ForceSetState("Sleep")
  else
    self.mecanim:ForceSetState("SleepStart")
  end
  self.mecanim:SetBool("sleep", true)
  self.mover:Stop()
  self.isSleeping = true
end

function EnemyEntity:WakeUp()
  if self.visibilityTime < 0.5 then
    self.mecanim:ForceSetState("Idle")
    self.mecanim:SetBool("sleep", false)
  else
    self.mecanim:SetBool("sleep", false)
  end
  self.isSleeping = false
end

return EnemyEntity