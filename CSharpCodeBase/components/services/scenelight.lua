local Class = require 'utils.hump.class'

local SceneLight = Class({init=function(self, gameObject)
    self.light = gameObject:GetComponent("LightController")
    GameController.eventSystem:AddListener('DAY_PHASE_CHANGED', self)
    self.timeout = GameController.database:Get("components", "SceneLight/colorChangeTime")
end})

function SceneLight:OnEvent(e, data)
  if e == 'DAY_PHASE_CHANGED' then
    self.light:SetColor(data.color[1], data.color[2], data.color[3],
      data.ambientColor[1], data.ambientColor[2], data.ambientColor[3], 
      self.timeout)
  end
end

return SceneLight