local Class = require 'utils.hump.class'
local AINode = require 'utils.ailib.ainode'

-- Select enemy using ai vision
local AI_SelectWeapon = Class({__includes=AINode, init=function(self, entity)
  AINode.init(self, entity)
end})

function AI_SelectWeapon:Visit()
  if self.status == NODE_READY then
    local nextWeapon = self.entity.weaponSequencer:GetNextWeapon()
    self.entity.weaponContainer:SetWeapon(nextWeapon)
    self.status = NODE_SUCCESS
  end
  return self.status
end

return AI_SelectWeapon
