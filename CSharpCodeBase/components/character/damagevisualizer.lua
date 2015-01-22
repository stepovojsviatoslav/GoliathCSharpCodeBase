local Class = require 'utils.hump.class'
local Component = require 'components.component'
local Vector2 = require 'utils.hump.vector2'
local Vector3 = require 'utils.hump.vector3'

local DamageVisualizerComponent = Class({__includes=Component, init=function(self, entity)
      Component.init(self, entity)
      self.entity.damageReceiver = self
end})

function DamageVisualizerComponent:OnApplyDamage(damageData)
  local text = ''
  
  local maxType
  local maxDamage
  
  for k, v in pairs(damageData.damage) do
    if maxType == nil then
      maxType = k
      maxDamage = v
    else
      if v > maxDamage then
        maxType = k
        maxDamage = v
      end
    end
  end
  
  -- Get color
  local currentColor = self:GetColor(maxType)
  if damageData.summary > 0 then
    text = '-' .. damageData.summary
    
    local critical = damageData.effects.critical or false
    if critical then 
      text = text .. GameController.database:Get('damagecolors', 'global/criticalAppend')
    end
  else
    text = '0'--GameController.database:Get('damagecolors', 'global/blockedHit')
  end
  
  local pos = self.entity:GetPosition()
  luanet.GameFacade.uiDamageManager:Show(pos.x, pos.y + self.entity.height, pos.z, currentColor[1], currentColor[2], currentColor[3], text)
end

function DamageVisualizerComponent:GetColor(dType)
  local defaultSection = GameController.database.data.damagecolors.default
  local section = GameController.database.data.damagecolors[dType] or defaultSection
  return {section.r, section.g, section.b}
end

return DamageVisualizerComponent