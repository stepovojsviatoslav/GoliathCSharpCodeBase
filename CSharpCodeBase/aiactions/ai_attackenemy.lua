local Class = require 'utils.hump.class'
local AINode = require 'utils.ailib.ainode'
local HitTester = require 'utils.hittest'

-- Attack enemy
local AI_AttackEnemy = Class({__includes=AINode, init = function (self, entity)
  AINode.init(self, entity)
end})

function AI_AttackEnemy:Visit()
  if self.status == NODE_READY then
    self.timeout = self.entity.weaponContainer:GetAttackTimeout()
    self.status = NODE_RUNNING
    self.attack = false
    -- start attacking
    self.entity.combat:Attack(self.parent.selected_target)
  end
  
  if self.status == NODE_RUNNING and self.attack then
    --[[
    if self.parent.selected_target ~= nil then
      self.entity.mover:LookAt(self.parent.selected_target:GetPosition())
    end
    ]]
    -- calc timeout
    self.timeout = self.timeout - GameController.deltaTime
    if self.timeout <= 0 then
      self.status = NODE_SUCCESS
    end
  end
  
  return self.status
end

function AI_AttackEnemy:OnEvent(data)
  if data == "punch" then        
    self.entity.relationship:AddInstance('enemy', self.parent.selected_target)
    --print("Punch!")
    self.attack = true
    -- OK, now need to calculate damage
    if self.entity.weaponContainer:CanAttack(self.parent.selected_target) and self.parent.selected_target.interactable then
      if self.entity.weaponContainer.currentWeapon.remote > 0 then
        self.entity.weaponContainer:Attack(self.parent.selected_target)
      else
        if HitTester.CheckHitEntity(self.entity, Transform.GetForwardVector(self.entity.transform), self.parent.selected_target, 80) then
          self.entity.weaponContainer:Attack(self.parent.selected_target)
        end
        --self.entity.damageProcessor:SendDamage(self.parent.selected_target, self.entity.weaponContainer.currentWeapon:GetDamage())
      end
    else
      print("Can not attack!")
    end
  end
end

return AI_AttackEnemy