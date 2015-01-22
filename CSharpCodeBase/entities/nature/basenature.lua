local Class = require 'utils.hump.class'
local UnityExistsEntity = require 'unity.unityentity'
local ConfigComponent = require 'components.services.config'
local BuilderComponent = require 'components.builder.buildercomponent'
local PossibleActionsComponent = require 'components.character.possibleactions'
local StateManagerComponent = require 'components.thing.statemanagercomponent'
local TimeManagerComponent = require 'components.thing.timemanagercomponent'

local BaseNatureEntity = Class({__includes=UnityExistsEntity, init = function (self, gameObject)
    UnityExistsEntity.init(self, gameObject)
    self.config = ConfigComponent('nature', self.name)
    self:AddComponent(PossibleActionsComponent)
    self:AddComponent(StateManagerComponent)
    self:AddComponent(TimeManagerComponent)
    self:AddComponent(BuilderComponent)
end})

function BaseNatureEntity.Create(storage, x, y, z, v)
  storage:SetBool("isLua", true)
  storage:SetPosition(x, y, z)
  storage:SetRotation(0, math.random(0, 360), 0)
  if v.scale then
    local scale = 1
    for key, value in string.gmatch(v.scale, '(%d%p%d):(%d%p%d)') do
      if tonumber(key) and tonumber(value) then    
        if key == value then
          scale = tonumber(value)
        else          
          scale = math.random() * tonumber(key) + tonumber(value)
        end      
      end
    end
    storage:SetScale(scale, scale, scale)
  end  
end

function BaseNatureEntity:Save(storage)
  storage:SetBool("isLua", true)
  storage:SetTransform(self.transform)
  self:Message('Save', storage)
end

function BaseNatureEntity:Load(storage)
  storage:LoadTransform(self.transform)
  self.spawnPosition = self:GetPosition()
  self:Message('Load', storage)
end

function BaseNatureEntity:Message(name, params)
  UnityExistsEntity.Message(self, name, params)
  if name == "OnInteract" then
    self:Message('OnStateEvent', 'OnTake')
  end
end

function BaseNatureEntity:OnCollisionEnter(targetEntity)
  self:Message('OnCollisionEnter', targetEntity)
end

function BaseNatureEntity:OnFragile()
  self:Message('OnStateEvent', 'OnFragile')
end

function BaseNatureEntity:OnFragileForce(damageData)
  self:Message('OnStateEvent', 'OnFragileForce')
  self:Message('OnFragileForce')
end

function BaseNatureEntity:OnRegenerate(incValue)
  self:Message('Increase', incValue)
  self:Message('OnRegeneration')
end

function BaseNatureEntity:DestroyThing(damageData)
  self:Message('OnStateEvent', 'OnHealthZero')
  self:Message('DropAfterDeath')
  self:Message('OnDeath', damageData)
end

function BaseNatureEntity:Hit(damageData)
  self:Message('OnStateEvent', 'OnHit')
  self:Message('DropAfterHit')
end

return BaseNatureEntity