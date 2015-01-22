local Class = require 'utils.hump.class'
local Vector3 = require 'utils.hump.vector3'
local UnityExistsEntity = require 'unity.unityentity'

local CameraEntity = Class({__includes=UnityExistsEntity, init = function (self, gameObject)
      Transform.AddComponent(gameObject, "LuaMapper")
      UnityExistsEntity.init(self, gameObject)
      
      self.targetEntity = nil
      
      self.minHeight = 6
      self.minDistance = 12
      self.maxHeight = 30
      self.maxDistance = 40
      self.targetHeight = 1
      
      self.currentZoom = 0.5
      self._zoom = self.currentZoom
      self.zoomSoftLimit = 0.05
      self.lerpZoomSpeed = 5
      self.lerpLimitSpeed = 20
      
      self.angle = 0
      self.currentAngle = 0
      
      self.scrollSpeed = 3
      
      self.luaMapper:SetAlwaysVisible(true)
end})

function CameraEntity:LoadForEntity(target)
  self.targetHeight = target.height / 2
  self.minHeight = target.config:Get('cameraMinHeight') or 6
  self.minDistance = target.config:Get('cameraMinDistance') or 12
  self.maxHeight = target.config:Get('cameraMaxHeight') or 30
  self.maxDistance = target.config:Get('cameraMaxDistance') or 40 
end

function CameraEntity:SetTargetEntity(entity)
  self.targetEntity = entity
end

function CameraEntity:Update()
  local isGamepad = GameController.inputService:IsGamepad()
  local lookValue = GameController.inputService:GetLookValue()
  local scroll = 0
  if isGamepad and GameController.inputService:RightBumperIsPressed() then
    if math.abs(lookValue.z) > 0.5 then
      scroll = lookValue.z
    end
  else
    scroll = Input.GetMouseScrollValue()
  end
  if scroll ~= 0 then
    local sign = scroll > 0 and 1 or -1
    self._zoom = self._zoom - sign * self.scrollSpeed * GameController.deltaTime
    self._zoom = math.clamp(0 - self.zoomSoftLimit, self._zoom, 1 + self.zoomSoftLimit)
  end
  if isGamepad then
    if GameController.inputService:RightBumperIsPressed() then
      local val = lookValue.x
      if math.abs(val) > 0.5 then
        if val < 0 and self.isGamepadRotateBlocked == nil then
          self.currentAngle = self.currentAngle - math.rad(90)
        elseif self.isGamepadRotateBlocked == nil then
          self.currentAngle = self.currentAngle + math.rad(90)
        end
        self.isGamepadRotateBlocked = true
      else
        self.isGamepadRotateBlocked = nil
      end
    end
  else
    if Input.GetKeyDown(KeyCode.Z) then
      self.currentAngle = self.currentAngle - math.rad(90)
    elseif Input.GetKeyDown(KeyCode.X) then
      self.currentAngle = self.currentAngle + math.rad(90)
    end
  end
  
  self.angle = math.lerp(self.angle, self.currentAngle, GameController.deltaTime * self.lerpLimitSpeed)
end

function CameraEntity:FixedUpdate()
end

function CameraEntity:LateUpdate()
  if self.targetEntity ~= nil then  
    -- Align zoom
    if self._zoom < 0 then 
      self._zoom = math.lerp(self._zoom, 0, GameController.deltaTime * self.lerpLimitSpeed)
    elseif self._zoom > 1 then
      self._zoom = math.lerp(self._zoom, 1, GameController.deltaTime * self.lerpLimitSpeed)
    end
    self.currentZoom = math.lerp(self.currentZoom, self._zoom, self.lerpZoomSpeed * GameController.deltaTime)
    
    -- Calculate current parameters
    local heightValue = math.lerp(self.minHeight, self.maxHeight, self.currentZoom)
    local distanceValue = math.lerp(self.minDistance, self.maxDistance, self.currentZoom)

    if self.heightValue == nil then
      self.heightValue = heightValue
      self.distanceValue = distanceValue
    end

    --heightValue = math.lerp(self.heightValue, heightValue, GameController.deltaTime * 50)
    --distanceValue = math.lerp(self.distanceValue, distanceValue, GameController.deltaTime * 50)

    self.heightValue = heightValue
    self.distanceValue = distanceValue

    local offsetVector = Vector3(0, heightValue, -distanceValue)
    offsetVector:RotateAroundY(self.angle)
    
    -- Setup position and rotation
    local position = self.targetEntity:GetPosition() + offsetVector
    self:SetPosition(position)
    
    -- Setup rotation
    offsetVector.y = offsetVector.y - self.targetHeight
    local rotation = RotationUtils.LookRotation(offsetVector * -1)
    self:SetRotation(rotation)
  end
end

return CameraEntity