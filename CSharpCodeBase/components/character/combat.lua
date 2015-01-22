local Class = require 'utils.hump.class'
local Component = require 'components.component'

local CombatComponent = Class({__includes=Component, init=function(self, entity)
  Component.init(self, entity)
  self.comboTimeout = 0
  self.currentAction = 1
  self.entity.combat = self
end})

function CombatComponent:Attack(target, weapon)
  local actionTypes = self.entity.weaponContainer:GetActionTypes(self.target, weapon)
  
  if os.clock() > self.comboTimeout or self.currentAction > #actionTypes then
    self.currentAction = 1
  end
  
  self.entity.weaponContainer:SetComboState(self.currentAction, weapon)
  --print("Attack action type: " .. actionTypes[self.currentAction][1])
  self.entity.mecanim:SetFloat("action_type", actionTypes[self.currentAction][1])
  self.entity.mecanim:SetTrigger("action")    
  
  self.currentAction = self.currentAction + 1
  if self.currentAction > #actionTypes then
    self.currentAction = 1
  end
  self.comboTimeout = os.clock() + actionTypes[self.currentAction][2]
  
  self.entity.mover:LookAt(target:GetPosition())
end

function CombatComponent:ResetCombo()
  self.currentAction = 1
end

function CombatComponent:AttackVector(vec3, weapon)
  local actionTypes = self.entity.weaponContainer:GetActionTypes(nil, weapon)
  if os.clock() > self.comboTimeout or self.currentAction > #actionTypes then
    self.currentAction = 1
  end
  self.entity.weaponContainer:SetComboState(self.currentAction, weapon)
  self.entity.mecanim:SetFloat("action_type", actionTypes[self.currentAction][1])
  self.entity.mecanim:SetTrigger("action")    
  self.currentAction = self.currentAction + 1
  if self.currentAction > #actionTypes then
    self.currentAction = 1
  end
  self.comboTimeout = os.clock() + actionTypes[self.currentAction][2]
  self.entity.mover:LookAt(vec3)
end

return CombatComponent