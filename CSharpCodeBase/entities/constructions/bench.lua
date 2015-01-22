local Class = require 'utils.hump.class'
local UnityExistsEntity = require 'unity.unityentity'
local ConfigComponent = require 'components.services.config'
local HealthComponent = require 'components.character.health'
local DamageReceiverComponent = require 'components.character.damagereceiver'
local DamageVisualizerComponent = require 'components.character.damagevisualizer'
local MecanimComponent = require 'components.animations.mecanim'
local ResistComponent = require 'components.character.resist'
local PossibleActionsComponent = require 'components.character.possibleactions'
local ThingDamageReceiverComponent = require 'components.thing.thingdamagereceiver'
local InventoryItem = require 'components.inventory.item'
local InventoryGrid = require 'components.inventory.grid'

local STATE_NORMAL = 0
local STATE_DAMAGED = 1
local STATE_DESTROYED = 2

local BenchEntity = Class({__includes=UnityExistsEntity, init = function (self, gameObject)
    UnityExistsEntity.init(self, gameObject)
    self.config = ConfigComponent('constructions', self.name)
    self:AddComponent(PossibleActionsComponent)
    self:AddComponent(HealthComponent)    
    self:AddComponent(DamageVisualizerComponent)
    self:AddComponent(MecanimComponent)
    self:AddComponent(ResistComponent)    
    self:AddComponent(DamageReceiverComponent)
    self:AddComponent(ThingDamageReceiverComponent)
    
    self.currentState = STATE_NORMAL
    self.nextState = STATE_NORMAL
    
    self.gameObjects = {}      
    --self.gameObjects[STATE_NORMAL] = self.entity.gameObject.transform:Find("Group47944").gameObject    
    --self.gameObjects[STATE_DAMAGED] = self.entity.gameObject.transform:Find("Group47944").gameObject
    self.gameObjects[STATE_NORMAL] = self.gameObject
    self.gameObjects[STATE_DAMAGED] = self.gameObject
    self.gameObjects[STATE_DAMAGED]:SetActive(false)
    self.gameObjects[STATE_NORMAL]:SetActive(true)     
    
    self.content = InventoryGrid(32, "bench")
end})

function BenchEntity.Create(storage, x, y, z, v)
  storage:SetBool("isLua", true)
  storage:SetPosition(x, y, z)  
  storage:SetRotation(0, math.random(0, 360), 0)
end

function BenchEntity:Save(storage)
  storage:SetBool("isLua", true)
  storage:SetTransform(self.transform)  
end

function BenchEntity:Load(storage)
  storage:LoadTransform(self.transform)
  self.spawnPosition = self:GetPosition()  
end

function BenchEntity:Update()
  UnityExistsEntity.Update(self) 
  if self.currentState ~= self.nextState then
    self:SetState(self.nextState)
  end
end

function BenchEntity:SetState(state)
  if self.currentState ~= state then
    self:Deactivate(self.currentState)
    self:Activate(state)
    self.currentState = state
  end
end

function BenchEntity:Activate(state)
  self.gameObjects[state]:SetActive(true)
end

function BenchEntity:Deactivate(state)
  self.gameObjects[state]:SetActive(false)
end

function BenchEntity:DeactivateAll()
  for k, v in pairs (self.gameObjects) do
    v:SetActive(false)
  end
end

function BenchEntity:Hit()
  local currentHealth = self.health.amount
  if self.state == STATE_NORMAL then
    if currentHealth < 0.5 * self.health.maxAmount then
      self.nextState = STATE_DAMAGED    
    end
  end 
end

function BenchEntity:DestroyThing(damageData)
  self:DeactivateAll()
  self:Destroy()
end

function BenchEntity:Message(name, params)  
  UnityExistsEntity.Message(self, name, params)  
  if name == "OnInteract" then    
    GameController.eventSystem:Event("BENCH_PANEL_ACTIVATE", self)
  end
end

--work with grid
function BenchEntity:AddItem(item, count)
  if count ~= nil then
    item = InventoryItem(item, count)
  end  
  return self.content:AddItem(item)
end

function BenchEntity:GetItemCount(item)  
  return self.content:GetItemCount(item)
end

function BenchEntity:SetSlotFromUI(idx, item, count)
  self.content:SetItem(idx, InventoryItem(item, count))
end

function BenchEntity:GetSlotCount(idx)
  return self.content:GetItem(idx).count
end

function BenchEntity:RemoveFromSlot(idx, count)
  return self.content:RemoveFromSlot(idx, count)
end

function BenchEntity:RemoveItems(item, count)
  self.content:RemoveItem(item, count)  
end
return BenchEntity