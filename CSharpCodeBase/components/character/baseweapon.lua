local Class = require 'utils.hump.class'
local ConfigComponent = require 'components.services.config'
local BulletEntity = require 'entities.weapon.bullet'

local BaseWeapon = Class({init = function (self, entity, name)
  self.name = name
  self.entity = entity
  self.config = ConfigComponent('weapons', self.name)
  self.minDistance = self.config:Get('minDistance') or 0
  self.maxDistance = self.config:Get('maxDistance')
  self.midDistance = self.config:Get('midDistance') or ((self.maxDistance - self.minDistance) / 2)
  self.attackTimeout = self.config:Get('attackTimeout')
  self.attackPrepareTimeout = self.config:Get('attackPrepareTimeout')
  self.currentComboState = 1
  self.remote = self.config:Get('remote') or 0
  
  local actionTypes = {}
  local combo = self.config:Get('combo')
  local timing = self.config:Get('combotimeout')
  local effects = self.config:Get('comboeffect') or {}
  
  for k, v in pairs(combo) do
    actionTypes[#actionTypes + 1] = {v, timing[k], effects[k]}
  end
  self.actionTypes = actionTypes
  
  -- Load damage parameters
  self:LoadDamage()
end})

function BaseWeapon:LoadDamage()
  self.criticalChance = self.config:Get('criticalChance')
  self.damage = {}
  local damageString = self.config:Get('damage')
  for dType, dValue in string.gmatch(damageString, '(%w+)%:(%d+)%,?') do
    self.damage[dType] = dValue
  end
end

function BaseWeapon:GetCriticalMultiplier()
  return math.chance(self.criticalChance) and 2 or 1
end

function BaseWeapon:GetDamage()
  local damage = {}
  local critMul = self:GetCriticalMultiplier()
  for k, v in pairs(self.damage) do
    damage[k] = v * critMul
  end
  local effect = self.actionTypes[self.currentComboState][3]
  return {damage=damage, effects={critical=(critMul > 1 and true or false), punch=effect}}
end

function BaseWeapon:CanAttack(target)
  local effectiveDistance = self.entity:GetEffectiveDistance(target)
  -- return effectiveDistance > self.minDistance and effectiveDistance < self.maxDistance
  return effectiveDistance < self.maxDistance and (self.minDistance <= 0 or effectiveDistance > self.minDistance or self.remote)
end

function BaseWeapon:GetActionTypes(target)
  return self.actionTypes
end

function BaseWeapon:GetAttackDistance()
  return self.midDistance --(self.maxDistance - self.minDistance) / 2
end

function BaseWeapon:GetAttackTimeout()
  return self.attackTimeout or 0
end

function BaseWeapon:GetAttackPrepareTimeout()
  return self.attackPrepareTimeout or 0
end

function BaseWeapon:OnActivate()
  local transforms = self.config:Get('transform')
  if transforms ~= nil then
    for k, v in pairs(transforms) do
      Transform.SetRendererState(self.entity.transform, v, true)
    end
  end
end

function BaseWeapon:OnDeactivate()
  local transforms = self.config:Get('transform')
  if transforms ~= nil then
    for k, v in pairs(transforms) do
      Transform.SetRendererState(self.entity.transform, v, false)
    end
  end
end

function BaseWeapon:Attack(target)
  if self.remote > 0 then
    -- throw bullet
    local bullet = BulletEntity('bullet', self.entity, target.GetPosition and target:GetPosition() or target, self)
    GameController:AddEntity(bullet)
    --print("Throw bullet!")
    -- create bullet weapon
  else
    self.entity.damageProcessor:SendDamage(target, self:GetDamage())  
  end
end

return BaseWeapon