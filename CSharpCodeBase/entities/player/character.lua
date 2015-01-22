local Class = require 'utils.hump.class'
local UnityExistsEntity = require 'unity.unityentity'
local Vector3 = require 'utils.hump.vector3'

local CharacterWeaponContainer = require 'components.character.weaponcontainer'
local Mover = require 'components.character.mover'
local MecanimComponent = require 'components.animations.mecanim'
local PlayerControllerComponent = require 'components.controlsystem.playercontroller'
local CombatComponent = require 'components.character.combat'
local ResistComponent = require 'components.character.resist'
local DamageProcessorComponent = require 'components.character.damageprocessor'
local DamageReceiverComponent = require 'components.character.damagereceiver'
local ConfigComponent = require 'components.services.config'
local PossibleActionsComponent = require 'components.character.possibleactions'

local DamageVisualizerComponent = require 'components.character.damagevisualizer'
local HealthComponent = require 'components.character.health'
local CharacterDamageReceiver = require 'components.character.characterdamagereceiver'
local GrenageVisualizer = require 'components.character.grenademodevisualizer'
local GamePadMover = require 'components.character.gamepadmover'
local TimeManagerComponent = require 'components.thing.timemanagercomponent'

local CharacterEntity = Class({__includes=UnityExistsEntity, init = function (self, gameObject)
      UnityExistsEntity.init(self, gameObject)
      self.config = ConfigComponent('heroes', self.name)
      self.rigidbody = self.gameObject:GetComponent("Rigidbody")
      self:AddComponent(HealthComponent)
      self:AddComponent(CharacterDamageReceiver)
      self:AddComponent(DamageVisualizerComponent)
      self:AddComponent(MecanimComponent)      
      self:AddComponent(Mover)        
      self:AddComponent(CharacterWeaponContainer)      
      self:AddComponent(CombatComponent)
      self:AddComponent(ResistComponent)
      self:AddComponent(DamageProcessorComponent)
      self:AddComponent(DamageReceiverComponent)
      self:AddComponent(PlayerControllerComponent)
      self:AddComponent(PossibleActionsComponent)
      self:AddComponent(GrenageVisualizer)
      if GameController.inputService:IsGamepad() then
        self:AddComponent(GamePadMover)
      end
      self:AddComponent(TimeManagerComponent)
      self.luaMapper:SetAlwaysVisible(true)
      self.isDeath = false
      self.deathTimeout = 0
      self.characterClass = self.config:Get('characterClass') or 1
      self.static = false
      self.mover.autoLook = false
end})

function CharacterEntity:Update()
  --[[ for left trigger
  if self.gamepadRightStickController then
    if GameController.inputService:LeftTriggerIsPressed() then
      self.gamepadRightStickController.active = false
      self.gamepadRightStickController:Disable()
    else
      self.gamepadRightStickController.active = true
    end
  end
  ]]  
  if not self.isDeath then
    UnityExistsEntity.Update(self)
  else
    self.deathTimeout = self.deathTimeout - GameController.deltaTime
    if self.deathTimeout <= 0 then
      self:Respawn()
    end
  end
end

function CharacterEntity:FixedUpdate()
  if not self.isDeath then
    UnityExistsEntity.FixedUpdate(self)
  end
end

function CharacterEntity:CanAttack(targetEntity)
  -- check weapon for target entity
  return self.weaponContainer:CanAttack(targetEntity)
end

function CharacterEntity:OnEvent(data)
  UnityExistsEntity.OnEvent(self, data)
end

function CharacterEntity:Hit(damageData)
  --if damageData.summary ~= 0 then
    self:Message("Hit", damageData)
  --end
end

function CharacterEntity:Pushed(damageData)
  --if damageData.summary ~= 0 then
    self:Message("Pushed", damageData)
  --end
end

function CharacterEntity:Death()
  if self.characterClass > 1 then
    -- mech
    self.mover:Stop()
    self.isDeath = true
    self.interactable = false
    self.enabled = false
    GameController.player.playerController:DeleteCurrentSlot()
  else
    self.mecanim:ForceSetState('Death')
    self.isDeath = true
    self.interactable = false
    self.deathTimeout = 5
    self.mover:Stop()
  end
end

function CharacterEntity:Respawn()
  self:Message('OnRespawn')
  self:SetPosition(GameController.player.homePosition) 
  GameController.worldController:TeleportTargetTransform(self.transform, 
    GameController.player.homePosition.x,
    GameController.player.homePosition.y,
    GameController.player.homePosition.z)
  self.mecanim:ForceSetState('Idle')
  self.interactable = true
  self.isDeath = false
end

function CharacterEntity:OnSelectCharacter()
  GameController.ui.character:SetupIcon(self.config:Get('cpanel_icon'))
  GameController.ui.character:SetupAmount(self.health:GetPercentAmount())
  --GameController.ui.character:UpdateHealth(self.health:GetPercentAmount())
  print("Spell system initialization!")
  GameController.spellSystem:SetupSpells(self.config:Get('spell'))
  --GameController.ui.slotsController:SetContainerSubTag('weapon', self.config:Get('weaponTag'))  
  self.interactable = true
  self.enabled = true
end

function CharacterEntity:OnDeselectCharacter()  
  GameController.spellSystem:SaveStatuses()
  self.interactable = false
  self.enabled = false
  if self.gamepadRightStickController then
    self.gamepadRightStickController:DropTarget()
  end  
end

function CharacterEntity:OnSpellCast(spell)
  return self.playerController:OnSpellCast(spell)
end

function CharacterEntity:OnHealthChanged()
  GameController.ui.character:SetupAmount(self.health:GetPercentAmount())
  --GameController.ui.character:UpdateHealth(self.health:GetPercentAmount())
end

function CharacterEntity:OnCollisionEnter(targetEntity)
end

function CharacterEntity:SetTimer(name, value)  
  self.timeManager:Add("TimerHandler", value, name, 'extended')
end

function CharacterEntity:TimerHandler(tResult)    
  if tResult.timeLeft <= 0 then
    tResult.complete = true
    GameController.spellSystem:TimerUpdate(self.name, tResult.name, 0)    
  else
    GameController.spellSystem:TimerUpdate(self.name, tResult.name, tResult.timeLeft)
  end  
end

return CharacterEntity