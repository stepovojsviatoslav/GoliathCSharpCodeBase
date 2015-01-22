local Class = require 'utils.hump.class'
local Timer = require 'utils.hump.timer'
local AINode = require 'utils.ailib.ainode'

local AI_Following = Class({__includes=AINode, init = function (self, entity, r1, r2, callback1, callback2)
  AINode.init(self, entity)
  self.minRadius = r1
  self.maxRadius = r2
  self.callback1 = callback1
  if callback2 == nil then
    self.callback2 = callback1
  else
    self.callback2 = callback2
  end
end})

function AI_Following:Visit()
  self.target = self.callback1(self.entity, self)
  self.avoidanceTarget = self.callback2(self.entity, self)
  
  if self.status == NODE_READY then
    self.status = NODE_RUNNING
  end
  
  if self.status == NODE_RUNNING then
    local distance1 = self.entity:GetSimpleDistance(self.target)
    local distance2 = self.entity:GetSimpleDistance(self.avoidanceTarget)
    self:Moving(distance1, distance2)
    if distance1 < self.maxRadius and distance2 > self.minRadius then
      self.status = NODE_SUCCESS
      self.entity.mover:Stop()
    end
  end
  return self.status
end

function AI_Following:Moving(distance1, distance2)
  if distance2 < self.minRadius then
    local tmp = self.entity:GetPosition() - self.target:GetPosition()
    self.entity.mover:SetInput(tmp , 0, false)
  elseif distance1 > self.maxRadius  then
    local tmp = self.target:GetPosition() - self.entity:GetPosition()
    self.entity.mover:LookAt(self.target:GetPosition())
    self.entity.mover:SetInput(tmp , 0, false)
  end
end

return AI_Following