local Class = require 'utils.hump.class'
local Component = require 'components.component'
local Vector3 = require 'utils.hump.vector3'
local ConfigComponent = require 'components.services.config'

local alpha = 45
local dt = 0
local g = 9.81
local speed = 15
local vectorForward = Vector3(0, 0, 1)
local vectorRight = Vector3(1, 0, 0)

local GrenadeModeVisualizerComponent = Class({__includes=Component, init=function(self, entity)
    Component.init(self, entity)
    self.entity.grenadeModeVisualizer = self
    self.pointList = {}
    self.targetPosition = nil
     
    self.lineManager = GameController.pooler:Fetch('line')
    self.lineRendererComponent = self.lineManager:GetComponent('LineRenderer')
    self.expRadius = GameController.pooler:Fetch('explosionRadius')
    self.flightRadius = GameController.pooler:Fetch('distance')
    self.lineRendererComponent:SetColors(luanet.UnityEngine.Color.green, luanet.UnityEngine.Color.red)
    self.lineRendererComponent:SetWidth(0.07, 0.07);
      
    self.oldDistance = 0
    dt = 0.15
      
    if not GameController.inputService:IsGamepad() then
      self.targetPosition = Input.RaycastMouseOnTerrain() - self.entity:GetPosition()
    else
      local position = self.entity:GetPosition() + self.entity:GetForwardVector()*5 + Vector3(0,0.05,0)
      Transform.SetPosition(self.expRadius.transform, position)
      self.targetPosition =  Transform.GetPosition(self.expRadius.transform) - self.entity:GetPosition()
    end
    
    self:Disable()
end})

function GrenadeModeVisualizerComponent:CalculateTrajectory()
  self.pointList = {}
  local speedY = self.speed.y
  local pos = Vector3(self.entity.gameObject.transform.position.x, self.entity.gameObject.transform.position.y + self.entity.height / 2, self.entity.gameObject.transform.position.z)
  table.insert(self.pointList, pos)
  while (pos.y - 0.1 > self.entity.gameObject.transform.position.y) do
    speedY = speedY - (g * dt)
    local position = Vector3(pos.x + self.speed.x *dt, 
                    pos.y + speedY * dt, 
                    pos.z + self.speed.z * dt)
    pos = position
    table.insert(self.pointList, position)
  end 
  self.lineRendererComponent:SetVertexCount(#self.pointList)
end

function GrenadeModeVisualizerComponent:FixedUpdate()
  if not self.active then
    return
  end
  
  if GameController.inputService:IsGamepad() == false then
    self.targetPosition = Input.RaycastMouseOnTerrain()- self.entity:GetPosition()  
  else
    Transform.SetPosition(self.expRadius.transform, self.targetPosition + self.entity:GetPosition())
  end
  
  local vec1 = Vector3(0,0,0)
  local vec2 = self.targetPosition
  vec2.y = 0
  self.distance = Vector3.Distance(vec1, vec2)
  self.direction = (vec2 - vec1)
  self.direction = self.direction:Normalize()
 
  if GameController.inputService:IsGamepad() then
    self:GamepadExplotionRadiusMoving()
  else
    if self.distance > self.radius then
      self.distance = self.radius
    else
      self.oldDistance = self.distance
    end
  end
  
  self.speedScalar = math.sqrt(self.distance*g/ (math.sin(2*alpha)))
  self.speed = Vector3(self.speedScalar* math.sin(alpha)*self.direction.x, self.speedScalar* math.cos(alpha), self.speedScalar* math.sin(alpha)*self.direction.z)
  self.time = 2* self.speedScalar* math.cos(alpha)/g
  
  self:CalculateTrajectory()
  self:DrawLine()
  
  if GameController.inputService:IsGamepad() == false then
    local vec = Vector3(self.pointList[#self.pointList].x, self.entity:GetPosition().y + 0.05, self.pointList[#self.pointList].z)
    Transform.SetPosition(self.expRadius.transform, vec)
  end 
  Transform.SetPosition(self.flightRadius.transform, Vector3(0,0.05,0) + self.entity:GetPosition())
end

function GrenadeModeVisualizerComponent:DrawLine()
  for i=1, #self.pointList, 1 do
    RigidbodyUtils.SetPointPositionToLineRenderer(self.lineRendererComponent, self.pointList[i].x,self.pointList[i].y,self.pointList[i].z, i-1)
  end
end

function GrenadeModeVisualizerComponent:GamepadExplotionRadiusMoving()
  local dir = GameController.inputService:GetLookValue()
  if GameController.inputService:GetLookValue().x ~=0 or GameController.inputService:GetLookValue().z ~=0 then
    self:MoveTransform(dir.x*speed, dir.z *speed)
    self.targetPosition =  Transform.GetPosition(self.expRadius.transform) - self.entity:GetPosition()
  end
end

function GrenadeModeVisualizerComponent:MoveTransform(speedX, speedZ)
  local position = Transform.GetPosition(self.expRadius.transform) + vectorForward*speedZ*GameController.deltaTime + vectorRight*speedX*GameController.deltaTime
  local currentDistance = Vector3.Distance(position, self.entity:GetPosition())
  if currentDistance < self.radius  then
      Transform.SetPosition(self.expRadius.transform, position)
  end
end


function GrenadeModeVisualizerComponent:Enable(name)
  self.name = name
  self.config = ConfigComponent('gadget', name)
  self.active = true
  self.lineManager:SetActive(true)
  self.flightRadius:SetActive(true)
  self.expRadius:SetActive(true)
  self.radius = self.config:Get('radius')
  self.radiusExp = self.config:Get('radius_exp')
  Transform.SetScale(self.flightRadius.transform, Vector3(self.radius*2, 0.1, self.radius*2))
  Transform.SetScale(self.expRadius.transform, Vector3(self.radiusExp*2, 0.1, self.radiusExp*2))
end

function GrenadeModeVisualizerComponent:Disable()
  self.active = false
  self.lineManager:SetActive(false)
  self.flightRadius:SetActive(false)
  self.expRadius:SetActive(false)
end

function GrenadeModeVisualizerComponent:GetSpeed()
  self:Disable()
  return self.speed
end

return GrenadeModeVisualizerComponent