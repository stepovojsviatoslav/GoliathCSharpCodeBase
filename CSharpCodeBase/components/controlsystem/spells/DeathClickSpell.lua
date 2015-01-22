local Action = require 'components.controlsystem.action'
local Class = require 'utils.hump.class'
local Vector3 = require 'utils.hump.vector3'
local HitTester = require 'utils.hittest'

local STATE_MOVING = 0
local STATE_REQUEST_ATTACK = 1
local STATE_ATTACK = 2

local SpellAction = Class({__includes=Action, init = function (self, name)
      Action.init(self, 3, false, name)
      self.position = nil
end})

function SpellAction:OnStartRunning()    
  -- call spell animation
  if self.position then    
    self.target = Input.RaycastTargetEntityWithRadius(1, self.position)        
  else
    self.target = Input.RaycastTargetEntityWithRadius(1)    
  end
end

function SpellAction:OnStopRunning()
  -- stop spell animation?
end


function SpellAction:Update()
  if not self.target then return true end
  
  if self.target.Death then
    self.target:Death()
  end
  -- cast spell
  return true
end

function SpellAction:OnEvent(data)
  -- Cast spell by animation here
end

function SpellAction:IsContinuous()
  return false
end

function SpellAction:BeginDraw()
end

function SpellAction:StopDraw()
end

function SpellAction.CanApply(position)    
  if position then        
    return Input.RaycastTargetEntityWithRadius(1, position) ~= nil
  else        
    return Input.RaycastTargetEntityWithRadius(1) ~= nil
  end
end

return SpellAction