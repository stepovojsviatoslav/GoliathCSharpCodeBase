local Class = require 'utils.hump.class'
local UnityExistsEntity = require 'unity.unityentity'
local Vector3 = require 'utils.hump.vector3'
local ConfigComponent = require 'components.services.config'

local GrenageEntity = Class({__includes = UnityExistsEntity, init = function (self, name, source, speed)
     local gameObject = GameController.pooler:Fetch(name)
     gameObject:GetComponent('Rigidbody').useGravity = false
     GameController:AddEntity(self)
     UnityExistsEntity.init(self, gameObject)
     self.config = ConfigComponent('gadget', self.name)
     self.luaMapper:SetAlwaysVisible(true)
     self:SetPosition(Vector3(source.gameObject.transform.position.x, source.gameObject.transform.position.y + source.height / 2, source.gameObject.transform.position.z))
     self.speed = speed
     self.dt = GameController.deltaTime
     self.radius = self.config:Get('radius_exp') or 1
     self.damage = self.config:Get('damage') or {dType=100}
     self.currentCharacter = GameController.player.playerController:GetCurrentSlot()
     GameController.inventory:RemoveItems(self.name, 1)
end})

function GrenageEntity:FixedUpdate()
   self.speed.y = self.speed.y - (9.81 * self.dt)
   local position = Vector3(self:GetPosition().x + self.speed.x * self.dt, 
                      self:GetPosition().y + self.speed.y * self.dt, 
                      self:GetPosition().z + self.speed.z * self.dt)
   self:SetPosition(position)
  UnityExistsEntity.FixedUpdate(self)
end

function GrenageEntity:OnCollisionEnter(targetEntity)
  local entities = RaycastUtils.GetEntitiesInRadius(self:GetPosition(), self.radius)
  for k, v in pairs(entities) do
    self.currentCharacter.damageProcessor:SendDamage(v, {damage=self.damage, effects = {}})
  end
  self:Destroy()
end

return GrenageEntity