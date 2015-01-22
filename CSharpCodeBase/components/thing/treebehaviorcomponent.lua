local Class = require 'utils.hump.class'
local Component = require 'components.component'

local TreeBehaviorComponent = Class({__includes=Component, init=function(self, entity)
      Component.init(self, entity)
      self.entity.treeBehavior = self
      self.animator = self.entity.mecanim
      self.deathTimeout = self.entity.config:Get('deathTimeout')
      self.fallenTreeTransformName = self.entity.config:Get('rigidbodyTransform')
      self.stumpTransformName = self.entity.config:Get('stumpTransform')
      
      self.stumpTransform = self.entity.gameObject.transform:Find(self.stumpTransformName)
      self.fallenTreeTransform = self.entity.gameObject.transform:Find(self.fallenTreeTransformName).gameObject.transform
end})

function TreeBehaviorComponent:OnDeath(damageData)
  if self.entity.stateManager.stateName[self.entity.stateManager.currentState] == 'fallenTree'  then
    self:SaveTransform()
    Transform.SetColliderState(self.stumpTransform, true)
    RigidbodyUtils.MoveDown(self.fallenTreeTransform.gameObject, damageData.source.gameObject)
  end
end

function TreeBehaviorComponent:SaveTransform()
  self.localPosition = self.fallenTreeTransform.localPosition
  self.localRotation = self.fallenTreeTransform.localRotation
end

function TreeBehaviorComponent:RestoreTransform()
  if self.localPosition and self.localRotation then
    self.fallenTreeTransform.localPosition = self.localPosition
    self.fallenTreeTransform.localRotation = self.localRotation
  end
end

function TreeBehaviorComponent:AfterFall(result)
  self.entity.stateManager:SetStateByNumber(#self.entity.stateManager.stateName)
  self:RestoreTransform()
  self.entity:Destroy()
  result:Complete()
end

function TreeBehaviorComponent:OnFragileForce()
    Transform.SetColliderState(self.stumpTransform, false)
end

return TreeBehaviorComponent