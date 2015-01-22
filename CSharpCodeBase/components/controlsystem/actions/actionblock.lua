local Action = require 'components.controlsystem.action'
local Class = require 'utils.hump.class'
local Vector3 = require 'utils.hump.vector3'

local ActionBlock = Class({__includes=Action, init = function (self, entity, target)
      Action.init(self, 1, false, 'block')
      self.entity = entity
      self.inputService = GameController.inputService
end})

function ActionBlock:OnPushed()
  --self.priority = 1
end

function ActionBlock:OnStartRunning()
  Action.OnStartRunning(self)
  self.entity.mecanim:SetBool("block", true)
  self.priority = 1
  self.entity.damageReceiver:SetOverrideSideResist('front', 0)
  self.entity.damageReceiver:SetOverrideSideResist('side', 0)
end

function ActionBlock:OnStopRunning()
  self.entity.mecanim:SetBool("block", false)
  self.entity.damageReceiver:DropOverrideSideResist('front')
  self.entity.damageReceiver:DropOverrideSideResist('side')
end

function ActionBlock:Update()
  -- Look at
  local input = Vector3()
  input = self.inputService:LeftStickValues() 
  
  if self.inputService:GetMouseButton(1) then
    input = Input.RaycastMouseOnTerrain()
  elseif input:Length() > 0 then
    input:Normalize()
    input:RotateAroundY(GameController.camera.angle) 
    input = input + self.entity:GetPosition()
  end
  --if input:Length() > 0 then
    --self.entity.mover:LookAt(input)
  --end
  
  return not self.inputService:RightTriggerIsPressed()--self.isBlock and not self.entity.mecanim:CheckStateName("Block")
end

return ActionBlock