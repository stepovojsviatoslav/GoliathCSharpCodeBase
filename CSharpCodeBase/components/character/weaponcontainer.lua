local Class = require 'utils.hump.class'
local Component = require 'components.component'

local CharacterWeaponContainer = Class({__includes=Component, init = function (self, entity)
      Component.init(self, entity)
      self.entity.weaponContainer = self
      local defaultWeaponName = self.entity.config:Get('defaultWeapon')
      local interactWeaponName = self.entity.config:Get('interactWeapon') or defaultWeaponName
      local takeWeaponName = self.entity.config:Get('takeWeapon') or defaultWeaponName
      self:SetWeapon(defaultWeaponName)
      self.interactWeapon = self:LoadWeapon(interactWeaponName)
      self.takeWeapon = self:LoadWeapon(takeWeaponName)
end})

function CharacterWeaponContainer:LoadWeapon(name)
  local classPath = GameController.database:Get('weapons', name ..'/classPath')
  local class = require(classPath)
  local weapon = class(self.entity, name)
  return weapon
end

function CharacterWeaponContainer:SetWeapon(weapon)
  if weapon == nil or weapon == '' then
    weapon = self.entity.config:Get('defaultWeapon')
  end
  if self.currentWeapon ~= nil then
    self.currentWeapon:OnDeactivate()
  end
  self.currentWeapon = self:LoadWeapon(weapon)
  self.currentWeapon:OnActivate()
end

function CharacterWeaponContainer:GetActionTypes(target, weapon)
  -- if target is hardcoded action, just replace action target here
  local currentWeapon = weapon or self.currentWeapon
  return currentWeapon:GetActionTypes(target)
end

function CharacterWeaponContainer:CanAttack(target, weapon)
  local currentWeapon = weapon or self.currentWeapon
  return currentWeapon:CanAttack(target)
end

function CharacterWeaponContainer:GetAttackDistance(weapon)
  local currentWeapon = weapon or self.currentWeapon
  return currentWeapon:GetAttackDistance()
end

function CharacterWeaponContainer:GetAttackTimeout(weapon)
  local currentWeapon = weapon or self.currentWeapon
  return currentWeapon:GetAttackTimeout()
end

function CharacterWeaponContainer:GetAttackPrepareTimeout(weapon)
  local currentWeapon = weapon or self.currentWeapon
  return currentWeapon:GetAttackPrepareTimeout()
end

function CharacterWeaponContainer:OnWeaponSelect(weapon)
  if self.currentWeapon.name ~= weapon then
    if weapon ~= '' and weapon ~= nil then
      if GameController.database:Get('items', weapon ..'/subTag') == self.entity.config:Get('weaponTag') then
        self:SetWeapon(weapon)
      else
        self:SetWeapon(nil)
      end
    else
      self:SetWeapon(nil)
    end
  end
end

function CharacterWeaponContainer:Attack(target, weapon)
  local currentWeapon = weapon or self.currentWeapon
  currentWeapon:Attack(target)
end

function CharacterWeaponContainer:SetComboState(state, weapon)
  local currentWeapon = weapon or self.currentWeapon
  currentWeapon.currentComboState = state
end
return CharacterWeaponContainer