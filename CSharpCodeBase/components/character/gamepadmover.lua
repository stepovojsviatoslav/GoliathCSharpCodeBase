local Class = require 'utils.hump.class'
local Vector2 = require 'utils.hump.vector2'
local Vector3 = require 'utils.hump.vector3'
local Component = require 'components.component'
local HitTest = require 'utils.hittest'

local angle = 15

local GamePadRightStickController = Class({__includes=Component, init = function (self, entity)
      Component.init(self, entity)
      self.entity.gamepadRightStickController = self
      self.radius = 20
      self.sortedEntityList = {}
      self.stickDirAngle = 0
      self.target = nil
      self.border = GameController.pooler:Fetch('target')
      self.lineManager = GameController.pooler:Fetch('line')
      self.lineRendererComponent = self.lineManager:GetComponent('LineRenderer')
      self.lineRendererComponent:SetVertexCount(2)
      self.lineRendererComponent:SetColors(luanet.UnityEngine.Color.red, luanet.UnityEngine.Color.red)
      self.lineRendererComponent:SetWidth(0.05, 0.05);
      self.time = 0
      self.currentCount = 0
      self.active = true
end})

function GamePadRightStickController:Update()  
  if self.active then
    if not self.entity.grenadeModeVisualizer.active then
        local rightBumper = GameController.inputService:RightBumperIsPressed()
        self.sortedEntityList = self:GetEntitiesInRadius()
        if not rightBumper and (math.abs(GameController.inputService:GetLookValue().x) > 0.4 or math.abs(GameController.inputService:GetLookValue().z) > 0.4) then
          if self.deniedTarget == nil then self.deniedTarget = self.oldTarget end
          self.stickDirAngle = self:GetAngle(GameController.inputService:GetLookValue())
          self.avalibleTargets = self:GetTargetFromList(self.stickDirAngle, angle)
          if #self.avalibleTargets == 0 then
            local temp1 = self:GetTargetFromList(self:ConvertAngle(self.stickDirAngle - angle*2), angle)
            local temp2 = self:GetTargetFromList(self:ConvertAngle(self.stickDirAngle + angle*2), angle)
            for key, value in pairs(temp1) do
              self.avalibleTargets[key] = value
            end
            for key, value in pairs(temp2) do
              self.avalibleTargets[key] = value
            end
          end
          if #self.avalibleTargets > 0 then
            table.sort(self.avalibleTargets, function(a,b) return a[1]<b[1] end)
            if self.deniedTarget == self.avalibleTargets[1][2] then
              if self.avalibleTargets[2] then
                self.target = self.avalibleTargets[2][2]
              end
            else
              self.target = self.avalibleTargets[1][2]
            end
            self.border:SetActive(true)
            self.lineManager:SetActive(true)
          end
        else
          self.deniedTarget = nil
        end
      if self.target then
        Transform.SetPosition(self.border.transform, Vector3(self.target:GetPosition().x, self.target:GetPosition().y + 0.1, self.target:GetPosition().z))
        RigidbodyUtils.SetPointPositionToLineRenderer(self.lineRendererComponent, self.target:GetPosition().x,self.target:GetPosition().y + 0.5,self.target:GetPosition().z, 0)
        RigidbodyUtils.SetPointPositionToLineRenderer(self.lineRendererComponent, self.entity:GetPosition().x,self.entity:GetPosition().y + 0.5,self.entity:GetPosition().z, 1)
      end
       
      if (self.target and self.target.visible == false) or 
      GameController.inputService:RightStickButtonWasPressed() or 
        (self.target and self.target.isDeath) 
      then
        self.target = nil
        self.border:SetActive(false)
        self.time = 0
        self.lineManager:SetActive(false)
        self.deniedTarget = nil
      end
      
      if self.target and GameController.inputService:GetLookValue().x ==0 and GameController.inputService:GetLookValue().z ==0 then
        if self.time > 1 then
          self.time = 0
          self.lineManager:SetActive(false)
        end
        self.time = self.time + GameController.deltaTime
      end
      self.oldTarget = self.target
    end
  end
end

function GamePadRightStickController:DropTarget()
  self.target = nil
  self.border:SetActive(false)
  self.time = 0
  self.lineManager:SetActive(false)
  self.deniedTarget = nil  
end

function GamePadRightStickController:GetTarget()
  return self.target
end

function GamePadRightStickController:SetTarget(target)
  self.target = target
  self.border:SetActive(true)
end

function GamePadRightStickController:GetEntitiesInRadius()
  local tempEntityList = RaycastUtils.GetEntitiesInRadius(self.entity:GetPosition(), self.radius)
  local entityList = {}
  for k, v in pairs(tempEntityList) do
    if v.gameObject.tag ~= 'Player' and v.gameObject.tag == 'Enemy' then
      local tbl = {self:GetAngle(v:GetPosition() - self.entity:GetPosition()), v}
      table.insert(entityList, tbl)
    end
  end
  table.sort(entityList, function(a,b) return a[1]<b[1] end)
  return entityList
end

function GamePadRightStickController:GetAngle(vec)
  local mAngle = math.atan2(vec.z, vec.x)*180/math.pi
  mAngle = (180 + mAngle)%360
  return mAngle
end

function GamePadRightStickController:GetTargetFromList(stickDirAngle, angle)
  local bound1 = Vector2(stickDirAngle, stickDirAngle + angle)
  local bound2 = Vector2(stickDirAngle, stickDirAngle - angle)
  local bound3 = Vector2()
  local bound4 = Vector2()
  
  if bound1.y > 360 then
    bound1.x = 0
    bound1.y = bound1.y - 360
    bound3.x = 360 - bound1.y
    bound3.y = 360
  end
  
  if bound2.y < 0 then
    bound2.x = bound2.y + 360
    bound2.y = 360
    bound4.x = 0
    bound4.y = stickDirAngle
  end
  
  local bounds = {}
  table.insert(bounds, bound1)
  table.insert(bounds, bound2)
  if bound3.y > 0 then
    table.insert(bounds, bound3)
  end
  if bound4.y > 0 then
    table.insert(bounds, bound4)
  end
  
  local entities = {}
  for k, v in pairs(bounds) do
    local targets = self:GetEntitiesBetweenAngles(v)
      for targetKey, targetValue in pairs(targets) do
        if targetValue.visible then
          local entity = {Vector3.Distance(targetValue:GetPosition(), self.entity:GetPosition()), targetValue}
          table.insert(entities, entity)
        end
      end
  end
  return entities
end

function GamePadRightStickController:GetEntitiesBetweenAngles(vec2)
  local entities = {}
  for k, v in pairs(self.sortedEntityList) do
    if vec2.x > vec2.y then
      if v[1] >= vec2.y and v[1] <= vec2.x then
        table.insert(entities, v[2])
      end
    else
      if v[1] >= vec2.x and v[1] <= vec2.y then
        table.insert(entities, v[2])
      end
    end
  end
  return entities
end


function GamePadRightStickController:ConvertAngle(angle)
  local mAngle = angle
  if mAngle < 0 then
    mAngle = 360 + mAngle
  elseif mAngle > 360 then
    mAngle = mAngle - 360
  end
  return mAngle
end

function GamePadRightStickController:Disable()
  self.target = nil
  self.border:SetActive(false)
  self.time = 0
  self.lineManager:SetActive(false)
  self.deniedTarget = nil
  self.active = false
end

function GamePadRightStickController:Enable()
  self.active = true
end

return GamePadRightStickController