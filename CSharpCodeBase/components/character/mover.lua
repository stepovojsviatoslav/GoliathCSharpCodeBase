local Class = require 'utils.hump.class'
local Vector2 = require 'utils.hump.vector2'
local Vector3 = require 'utils.hump.vector3'
local Component = require 'components.component'

local Mover = Class({__includes=Component, init = function (self, entity)
      Component.init(self, entity)
      self.entity.mover = self
      self.input = Vector3()
      self.speed = self.entity.config:Get("moverSpeed")
      self.target = nil
      self._isStop = false
      self._epsilon = self.entity.config:Get("moverEpsilon")
      self.moveType = 0
      self.newDirection = Vector3(0,0,0)
      self._useLoaDefault = self.entity.config:Get("moverLoa")
      self._useLoa = self._useLoaDefault
      self.raycastSphereRadius = self.entity.config:Get("raycastSphereRadius")
      self.raycastForwardDistance = self.entity.config:Get("raycastForwardDistance")
      self.raycastSphericalDistance = self.entity.config:Get("raycastSphericalDistance")
      self.offsetAngle = self.entity.config:Get("offsetAngle")
      self.speedCurve = nil
      self.autoLook = true
      

      self.count = 0
      self.countmax = 3
end})

function Mover:ResetSpeed()
  self.speed = self.entity.config:Get("moverSpeed")
end

function Mover:OnChangeVisibility(state)
  self:Stop()
end

function Mover:SetCurve(curve)
  self.speedCurve = curve
end

function Mover:SetInput(vec3, moveType, ignoreLoa)
  if ignoreLoa then
    self._useLoa = false
  else
    self._useLoa = self._useLoaDefault
  end
  self.target = nil
  self.input.x = vec3.x or 0
  self.input.z = vec3.z or 0
   if self.input:Length() > 1 then
    self.input:Normalize()
  end
  self:CheckStop()
  self.moveType = moveType or 0
end

function Mover:Stop()
  self:SetInput(Vector3())  
  RigidbodyUtils.Move(self.entity.rigidbody, self.input * self.speed * GameController.gameTime)
end

function Mover:CheckStop()
  local inputLength = self.input:Length() * self.speed
  if not self._isStop and inputLength == 0 then
    self.newDirection = Vector3()
    self._isStop = true
    RigidbodyUtils.Move(self.entity.rigidbody, Vector3())
    --RigidbodyUtils.ResetVelocity(self.entity.rigidbody)
  elseif self._isStop and inputLength ~= 0 then
    self._isStop = false
  end  
end

function Mover:SetSpeed(speed)
  self.speed = speed
end

function Mover:SetInputToVec(vec3, stayTarget, moveType, ignoreLoa)
  if ignoreLoa then
    self._useLoa = false
  else
    self._useLoa = self._useLoaDefault
  end
  if not stayTarget then self.target = nil end
  local dv = vec3 - self.entity:GetPosition()
  self.input.x = dv.x
  self.input.z = dv.z
   if self.input:Length() > 1 then
    self.input:Normalize()
  end
  self:CheckStop()
  self.moveType = moveType or 0
end

function Mover:SetInputFromVec(vec3, stayTarget, moveType, ignoreLoa)
  if ignoreLoa then
    self._useLoa = false
  else
    self._useLoa = self._useLoaDefault
  end
  if not stayTarget then self.target = nil end
  local dv = vec3 - self.entity:GetPosition()
  self.input.x = -dv.x
  self.input.z = -dv.z
  if self.input:Length() > 1 then
    self.input:Normalize()
  end
  self:CheckStop()
  self.moveType = moveType or 0
end

function Mover:LookAt(vec3)
  RigidbodyUtils.LookAt(self.entity.rigidbody, vec3)
end

function Mover:Update()
  if self.target ~= nil then
    -- check if we reach target
    local pos = self.target.GetPosition and self.target:GetPosition() or self.target
    --print(self.entity:GetEffectiveDistance(self.entity, self.target))
    if self.target.GetPosition and (self.entity:GetEffectiveDistance(self.target) < self.targetDistance or not self.target.interactable)then
      self.target = nil
      self:Stop()
    elseif not self.target.GetPosition and self:IsReachDestination(pos) then
      self.target = nil
      self:Stop()
    else
      self:SetInputToVec(pos, true)
    end
  end
  local moveSpeed = (self.input * self.speed):Length()
  if self._latestMoveSpeed ~= moveSpeed then
    self.entity.mecanim:SetFloat("move_speed", moveSpeed)
    self._latestMoveSpeed = moveSpeed
  end
  if self._latestMoveType ~= self.moveType then
    self.entity.mecanim:SetFloat("move_type", self.moveType)
    self._latestMoveType = self.moveType
  end
end


function Mover:FixedUpdate()
  if not self._isStop then
    if self._useLoa == true then
      if self.count >= self.countmax then
        self.count = 0
        local result = RigidbodyUtils.AvoidMove(self.entity.gameObject, 
                                                self.input, 
                                                self.newDirection, 
                                                self.raycastSphereRadius , 
                                                self.raycastForwardDistance, 
                                                self.raycastSphericalDistance ,
                                                self.offsetAngle)
      self.input = Vector3(result.x1, 0, result.z1)
      self.newDirection = Vector3(result.x2, 0, result.z2)

      end
    end
    if self.speedCurve ~= nil then
      self.input = self.input * self.entity.mecanim:GetFloat('speed_curve')
    end

    if self.newDirection and self.newDirection.x ~= 0 and self.newDirection.y ~=0 and self.newDirection.z ~=0 then
      self.input = self.newDirection
    end
  end

  if self.input:Length() > 0 then
    if self.autoLook then
        RigidbodyUtils.Move(self.entity.rigidbody, self.input * self.speed, self._useLoa and 5 or 10)
    else
      if GameController.inputService:IsGamepad() == false then
        RigidbodyUtils.MoveLookAtMouse(self.entity.rigidbody, self.input * self.speed, self.entity.height / 3)
      else
        if self.entity.gamepadRightStickController and  self.entity.gamepadRightStickController:GetTarget() then
          RigidbodyUtils.MoveNotRotate(self.entity.rigidbody, self.input * self.speed)
          self:LookAt((self.entity.gamepadRightStickController:GetTarget():GetPosition()))
        else
          RigidbodyUtils.Move(self.entity.rigidbody, self.input * self.speed)
        end
      end
    end
  end
  self.count = self.count + 1
end


function Mover:SetGoal(target, moveType, distance)
  self.target = target
  self.targetDistance = distance or self._epsilon
  self.moveType = moveType or 0
end

function Mover:IsHaveGoal()
  return self.target ~= nil
end

function Mover:IsReachDestination(vec3)
  local localPos = self.entity:GetPosition()
  local v1 = Vector2(localPos.x, localPos.z)
  local v2 = Vector2(vec3.x, vec3.z)
  return v1:Dist(v2) < self._epsilon
end

return Mover