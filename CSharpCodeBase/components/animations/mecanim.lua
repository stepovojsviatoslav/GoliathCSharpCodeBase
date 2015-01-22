local Class = require 'utils.hump.class'
local Component = require 'components.component'

local MecanimComponent = Class({__includes=Component, init = function (self, entity)
      Component.init(self, entity)
      self.animator = self.transform:GetComponent("Animator")
      self.entity.mecanim = self
end})

function MecanimComponent:ForceSetState(name, layer)
  self.animator:CrossFade(name, 0, layer or -1, 0)
end

function MecanimComponent:ForceSetNewState(name, time)
  self.animator:CrossFade(name, 0, -1, time)
end

function MecanimComponent:SetFloat(name, val)
  self.animator:SetFloat(name, val)
end

function MecanimComponent:SetBool(name, val)
  self.animator:SetBool(name, val)
end

function MecanimComponent:ResetTrigger(name)
  self.animator:ResetTrigger(name)
end

function MecanimComponent:SetTrigger(name)
  self.animator:SetTrigger(name)
end

function MecanimComponent:CheckStateName(name, layer)
  return MecanimUtils.CheckCurrentStateName(self.animator, name, layer or 0)
end

function MecanimComponent:GetFloat(name)
  return self.animator:GetFloat(name)
end

return MecanimComponent