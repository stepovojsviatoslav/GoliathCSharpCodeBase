local Class = require 'utils.hump.class'
local Component = require 'components.component'

local ResistComponent = Class({__includes=Component, init = function (self, entity)
      Component.init(self, entity)
      self:LoadResist()
      self:LoadSideResist()
      self.entity.resist = self
end})

function ResistComponent:LoadResist(resist)
  local resistString = resist or self.entity.config:Get("resist")
  self.resist = {}
  if resistString ~= nil then
    for dType, dValue in string.gmatch(resistString, '([%w%*]+)%:(%d+)%,?') do
      self.resist[dType] = dValue/100.0
    end
  end
  if self.resist['*'] == nil then
    self.resist['*'] = 1
  end
end

function ResistComponent:LoadSideResist()
  local resistString = self.entity.config:Get("sideResist")
  self.sideResist = {}
  if resistString ~= nil then
    for dType, dValue in string.gmatch(resistString, '([%w%*]+)%:(%d+)%,?') do
      self.sideResist[dType] = dValue/100.0
    end
  end
  if self.sideResist['*'] == nil then
    self.sideResist['*'] = 1
  end
end

function ResistComponent:GetResist(damageType)
  local value = self.resist[damageType]
  return value ~= nil and value or self.resist['*']
end

function ResistComponent:GetSideResist(sideType)
  local value = self.sideResist[sideType]
  return value ~= nil and value or self.sideResist['*']
end

return ResistComponent