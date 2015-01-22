local Class = require 'utils.hump.class'
local UnityExistsEntity = require 'unity.unityentity'
local Vector3 = require 'utils.hump.vector3'

local MockupEntity = Class({__includes=UnityExistsEntity, init = function (self, gameObject)
    UnityExistsEntity.init(self, gameObject)
    self.count = 0
    self.oldCount = 0
    self.enable = false
    self.lerpEnd = true
    self.luaMapper:SetAlwaysVisible(true)
    self.oldPosition = Input.RaycastMouseOnTerrain()
    Transform.SetMaterialColor(self.gameObject, 0, 0.5, 0, 0.5)
end})

function MockupEntity:Enable()
  self.enable = true
end

function MockupEntity:Disable()
  self.enable = false
  self:Destroy()
end

function MockupEntity:Update()
  UnityExistsEntity.Update(self)
  
  if self.enable == true then
    self.position = Input.RaycastMouseOnTerrain()
    if math.abs(self.oldPosition.x - self.position.x) > 0.05 or math.abs(self.oldPosition.z - self.position.z) > 0.05 then
      Transform.Lerp(self.gameObject, self.position.x, self.position.y, self.position.z)
      self.lerpEnd = false
    elseif self.lerpEnd == false then
      if math.abs(self.oldPosition.x - self.gameObject.transform.position.x)<0.1 and math.abs(self.gameObject.transform.position.z - self.oldPosition.z)<0.1 then
        self.lerpEnd = true
      else
        Transform.Lerp(self.gameObject, self.oldPosition.x, self.oldPosition.y, self.oldPosition.z) 
      end
    end
    self.oldPosition = self.position
  end
  if self.oldCount ~= self.count then
      self.oldCount = self.count
      if self.count == 0 then
        Transform.SetMaterialColor(self.gameObject, 0, 0.5, 0, 0.5)
      else
        Transform.SetMaterialColor(self.gameObject, 0.5, 0, 0, 0.5)
      end
  end
end

function MockupEntity:Rotate(angle)
  RigidbodyUtils.Rotate(self.gameObject, angle)
end

function MockupEntity:MoveUpTransform(speed)
  RigidbodyUtils.MoveTransform(self.gameObject, speed, 0)
end

function MockupEntity:GetPosition()
  return Vector3(self.gameObject.transform.position.x, self.gameObject.transform.position.y, self.gameObject.transform.position.z)
end

function MockupEntity:MoveRightTransform(speed)
  RigidbodyUtils.MoveTransform(self.gameObject, 0, speed)
end

function MockupEntity:OnCollisionEnter(targetEntity)
    self.count = self.count + 1
end

function MockupEntity:OnCollisionExit(targetEntity)
    self.count = self.count - 1
end

return MockupEntity