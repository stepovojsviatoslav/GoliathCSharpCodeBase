local Class = require 'utils.hump.class'
local Vector3 = require 'utils.hump.vector3'
local UnityExistsEntity = require 'unity.unityentity'
local PossibleActionsComponent = require 'components.character.possibleactions'
local ConfigComponent = require 'components.services.config'

local STATE_MOVING = 0
local STATE_STAYING = 1

local Droppable = Class({__includes=UnityExistsEntity, init = function (self, gameObject)
      UnityExistsEntity.init(self, gameObject)
      self.config = ConfigComponent('items', self.name)
      self:AddComponent(PossibleActionsComponent)
      self.item = nil
      self.count = 0
      self.position = Vector3()
      self.rigidBody = self.gameObject:GetComponent("Rigidbody")
      self.state = STATE_MOVING
      self.stateTimeout = 1
end})

function Droppable:SetupDrop(item, count, sourceEntity, forceVec)
  -- calculate position from prefab
  if forceVec ~= nil then
    forceVec:Randomize(0.4, 0, 0.4)
  end
  local forceVector = forceVec or Vector3.GetRandom(5, 0, 5)
  forceVector:Normalize()
  local position = sourceEntity:GetPosition() + forceVector * sourceEntity.radius + Vector3(0, 1, 0) * (sourceEntity.height / 2.0)
  self:SetPosition(position)
  
  self.item = item
  self.count = count
  if sourceEntity ~= nil then
    RigidbodyUtils.IgnoreCollision(sourceEntity.transform, self.transform)
  end
  RigidbodyUtils.ApplyImpulse(self.rigidBody, forceVector * 5)
end

function Droppable:Message(funcname, data)
  if funcname == "OnInteract" then
    self:OnInteract(data)
  end
  UnityExistsEntity.Message(self, funcname, data)
end

function Droppable:OnInteract(sourceEntity)
  local restItem = GameController.inventory:AddItem(self.item, self.count)
  if restItem.count > 0 then
    GameController.dropManager:DropItem(restItem.name, restItem.count, self:GetPosition())
  end
  self:Destroy()
end

function Droppable:Update()
  UnityExistsEntity.Update(self)
  if self.state == STATE_MOVING then
    self.stateTimeout = self.stateTimeout - GameController.deltaTime
    if self.stateTimeout < 0 then
      self.state = STATE_STAYING
      self.rigidBody.isKinematic=true
      self.gameObject.collider.isTrigger = true
    end
  end
end

function Droppable:Save(storage)
  storage:SetBool("isLua", true)
  storage:SetTransform(self.transform)
end

function Droppable:Load(storage)
  storage:LoadTransform(self.transform)
  self.spawnPosition = self:GetPosition()  
end

function Droppable.Create(storage, x, y, z, _type)
  storage:SetBool("isLua", true)
  storage:SetPosition(x, y, z)
end

return Droppable