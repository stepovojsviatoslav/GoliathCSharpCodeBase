local Class = require 'utils.hump.class'
local UnityExistsEntity = require 'unity.unityentity'
local Vector3 = require 'utils.hump.vector3'

local BulletEntity = Class({__includes = UnityExistsEntity, init = function (self, bulletName, source, target, weapon)
      local gameObject = GameController.pooler:Fetch(bulletName)
      self._pooled = true
      UnityExistsEntity.init(self, gameObject)
      self.luaMapper:SetAlwaysVisible(true)
      self:SetPosition(source:GetPosition() + Vector3(0, source.height / 2, 0))
      self.weapon = weapon
      self.dv = Transform.GetForwardVector(source.transform)
      self.dv.y = 0
      self.dv:Normalize()
      
      self.timeout = weapon.config:Get('bulletTimeout') or 2
      self.speed = weapon.config:Get('bulletSpeed')
      self.damageRadius = weapon.config:Get('bulletDamageRadius') or 1
      self.source = source
      self.target = target
      self.rigidbody = self.gameObject:GetComponent("Rigidbody")
      self.rigidbody.useGravity = false
      RigidbodyUtils.ApplyImpulse(self.rigidbody, self.dv * self.speed)
end})

function BulletEntity:OnCollisionEnter(targetEntity)
  --print("Bullet collision! " .. targetEntity.name)
  --self.entity.damageProcessor:SendDamage(target, self:GetDamage())  
  if targetEntity ~= self.source then
    self.source.damageProcessor:SendDamage(targetEntity, self.weapon:GetDamage())
    self.timeout = 0
  end
end

function BulletEntity:Update()
  UnityExistsEntity.Update(self)
  self.timeout = self.timeout - GameController.deltaTime
  if self.timeout < 0 then
    self:Destroy()
  end
end

return BulletEntity