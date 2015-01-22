local Class = require 'utils.hump.class'
local Component = require 'components.component'

local GuiControl = Class({__includes=Component, init = function(self, entity)
      Component.init(self, entity)
      self.entity.guiControl = self      
end })

function GuiControl:Update()
  if Input.GetKey(KeyCode.Alpha0) then
    self.entity.playerController:SelectSlot(1)  
  elseif Input.GetKey(KeyCode.Alpha9) then
    self.entity.playerController:SelectSlot(2)
  end
end

return GuiControl