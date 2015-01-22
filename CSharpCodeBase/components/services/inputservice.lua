local Class = require 'utils.hump.class'
local Stack = require 'utils.hump.stack'
local Vector3 = require 'utils.hump.vector3'

local default = 'Default Layer'
local mouseAndKeyboard = 'Keyboard/Mouse'

local InputService = Class({init=function(self)
    self.inputLayersStack = Stack()
    self.inputLayersStack:Push(default)
    self.inputDevice = luanet.InControl.InputManager.ActiveDevice
end})

function InputService:IsGamepad()
  if self.inputDevice.Name == mouseAndKeyboard then
    return false
  end
  return true
end

function InputService:CheckLayer(layer)
  local mLayer = layer or default
  if mLayer and mLayer == self.inputLayersStack:Top() then
    return true
  end
  return false
end

function InputService:PushFrame(name)
  self.inputLayersStack:Push(name)
end

function InputService:PopFrame()
  if self.inputLayersStack:Top() ~= default then
    self.inputLayersStack:Pop()
  end
end

function InputService:GetKey(keycode, layer)
  if self:CheckLayer(layer) then
    return Input.GetKey(keycode)
  end
  return false
end

function InputService:GetKeyDown(keycode, layer)
  if self:CheckLayer(layer) then
    return Input.GetKeyDown(keycode)
  end
  return false
end

function InputService:GetKeyDownUnlocked(keycode, layer)
  if self:CheckLayer(layer) then
    return Input.GetKeyDownUnlocked(keycode)
  end
  return false
end

function InputService:GetMouseButtonDown(keycode, layer)
  if self:CheckLayer(layer) then
    return Input.GetMouseButtonDown(keycode)
  end
  return false
end

function InputService:GetMouseButton(keycode, layer)
  if self:CheckLayer(layer) then
    return Input.GetMouseButton(keycode)
  end
  return false
end

function InputService:BottomButtonWasPressed(layer)
  if self:CheckLayer(layer) then
    return self.inputDevice.Action1.WasPressed
  end
  return false
end

function InputService:LeftButtonWasPressed(layer)
  if self:CheckLayer(layer) then
    return self.inputDevice.Action3.WasPressed
  end
  return false
end

function InputService:TopButtonWasPressed(layer)
  if self:CheckLayer(layer) then
    return self.inputDevice.Action4.WasPressed
  end
  return false
end

function InputService:RightButtonWasPressed(layer)
  if self:CheckLayer(layer) then
    return self.inputDevice.Action2.WasPressed
  end
  return false
end

function InputService:LeftStickValues(layer)
  local value = Vector3()
  if self:CheckLayer(layer) then
    value.x = self.inputDevice.LeftStickX.Value
    value.z = self.inputDevice.LeftStickY.Value
  end
  return value
end

-- Левый стик был нажат по вертикале
function InputService:LeftStickYIsPressed(layer)
  if self:CheckLayer(layer) then
    return self.inputDevice.LeftStickY.IsPressed
  end
  return false
end

function InputService:LeftStickXIsPressed(layer)
  if self:CheckLayer(layer) then
    return self.inputDevice.LeftStickX.IsPressed
  end
  return false
end

function InputService:RightTriggerWasPressed(layer)
  if self:CheckLayer(layer) then
    return self.inputDevice.RightTrigger.WasPressed
  end
  return false
end

function InputService:RightTriggerIsPressed(layer)
  if self:CheckLayer(layer) then
    return self.inputDevice.RightTrigger.IsPressed
  end
  return false
end

function InputService:LeftTriggerWasPressed(layer)
  if self:CheckLayer(layer) then
    return self.inputDevice.LeftTrigger.WasPressed
  end
  return false
end

function InputService:LeftTriggerIsPressed(layer)
  if self:CheckLayer(layer) then
    return self.inputDevice.LeftTrigger.IsPressed
  end
  return false
end

function InputService:GetLookValue(layer)
  local value = Vector3()
  if self:CheckLayer(layer) then
    value.x = self.inputDevice.RightStickX.Value
    value.z = self.inputDevice.RightStickY.Value
  end
  return value
end

function InputService:LookXWasPressed(layer)
  if self:CheckLayer(layer) then
    return self.inputDevice.RightStickX.WasPressed
  end
  return false
end

function InputService:LookYWasPressed(layer)
  if self:CheckLayer(layer) then
    return self.inputDevice.RightStickY.WasPressed
  end
  return false
end

function InputService:RightStickYWasPressed(layer)
  if self:CheckLayer(layer) then
    return self.inputDevice.RightStickY.WasPressed
  end
  return false
end

function InputService:RightStickXWasPressed(layer)
  if self:CheckLayer(layer) then
    return self.inputDevice.RightStickX.WasPressed
  end
  return false
end

function InputService:RightStickButtonWasPressed(layer)
  if self:CheckLayer(layer) then
    return self.inputDevice.RightStickButton.WasPressed
  end
  return false
end

function InputService:LeftStickButtonWasPressed(layer)
  if self:CheckLayer(layer) then
    return self.inputDevice.LeftStickButton.WasPressed
  end
  return false
end

function InputService:LeftBumperWasPressed(layer)
  if self:CheckLayer(layer) then
    return self.inputDevice.LeftBumper.WasPressed
  end
  return false
end

function InputService:LeftBumperIsPressed(layer)
  if self:CheckLayer(layer) then
    return self.inputDevice.LeftBumper.IsPressed
  end
  return false
end

function InputService:RightBumperWasPressed(layer)
  if self:CheckLayer(layer) then
    return self.inputDevice.RightBumper.WasPressed
  end
  return false
end

function InputService:RightBumperIsPressed(layer)
  if self:CheckLayer(layer) then
    return self.inputDevice.RightBumper.IsPressed
  end
  return false
end

function InputService:DPadLeftIsPressed(layer)
  if self:CheckLayer(layer) then
    return self.inputDevice.DPadLeft.IsPressed
  end
  return false
end

function InputService:DPadRightIsPressed(layer)
  if self:CheckLayer(layer) then
    return self.inputDevice.DPadRight.IsPressed
  end
  return false
end

function InputService:DPadUpIsPressed(layer)
  if self:CheckLayer(layer) then
    return self.inputDevice.DPadUp.IsPressed
  end
  return false
end

function InputService:DPadDawnIsPressed(layer)
  if self:CheckLayer(layer) then
    return self.inputDevice.DPadDown.IsPressed
  end
  return false
end

return InputService