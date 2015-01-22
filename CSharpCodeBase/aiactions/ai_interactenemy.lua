local Class = require 'utils.hump.class'
local AINode = require 'utils.ailib.ainode'

-- Attack enemy
local AI_InteractEnemy = Class({__includes=AINode, init = function (self, entity)
  AINode.init(self, entity)
end})

function AI_InteractEnemy:Visit()
  if self.status == NODE_READY then
    self.timeout = self.entity.weaponContainer:GetAttackTimeout(self.entity.weaponContainer.interactWeapon)
    self.status = NODE_RUNNING
    self.attack = false
    -- start attacking
    self.entity.combat:Attack(self.parent.selected_target, self.entity.weaponContainer.interactWeapon)
  end
  
  if self.status == NODE_RUNNING and self.attack then
    -- calc timeout
    self.timeout = self.timeout - GameController.deltaTime
    if self.timeout <= 0 then
      self.status = NODE_SUCCESS
    end
  end
  
  return self.status
end

function AI_InteractEnemy:OnEvent(data)
  if data == "punch" then       
    self.attack = true
    if self.parent.selected_target.interactable and self.entity.weaponContainer:CanAttack(self.parent.selected_target, self.entity.weaponContainer.interactWeapon) then
      local canBeFood = true
      if self.parent.selected_target.CanBeFood then
        canBeFood = self.parent.selected_target:CanBeFood()
      end
      if canBeFood then
        self.parent.selected_target:Message("OnInteract", self.entity)
      end
      self.entity.starvation:SetState(false)
    end        
  end
end

return AI_InteractEnemy