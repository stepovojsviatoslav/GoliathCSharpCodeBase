local Class = require 'utils.hump.class'

local SpellSystem = Class({init=function(self)
   self.currentSpellAction = nil
   self.statuses = {}
end})

function SpellSystem:GetSpell(spellName)
  local spell = GameController.database:Get('spells', spellName)
  spell.name = spellName  
  return spell
end

function SpellSystem:StartSpell(spellName)
  local spell = self:GetSpell(spellName)
  local spellActionClass = require(spell.class)
  self.currentSpellAction = spellActionClass(spellName)
  self.currentSpellAction:BeginDraw()
end

function SpellSystem:DropSpell()
  if self.currentSpellAction ~= nil then
    self.currentSpellAction:StopDraw()
  end
end

function SpellSystem:ApplySpell(spellName, position)   
  if spellName == '' and spellName == nil then return false end
  local spell = self:GetSpell(spellName)
  local spellActionClass = require(spell.class)
  if spellActionClass.CanApply(position) then
    --if self.currentSpellAction == nil then
      self.currentSpellAction = spellActionClass(spellName)      
    --end
    if position then               
      self.currentSpellAction.position = position
    else      
      self.currentSpellAction.position = nil
    end
    GameController.player.playerController:GetCurrentSlot():OnSpellCast(self.currentSpellAction)        
    return true
  else    
    return false
  end
end

function SpellSystem:SaveStatuses()
  local currentHero = GameController.player.playerController:GetCurrentSlot()
  if not self.statuses[currentHero.name] then
    self.statuses[currentHero.name] = {}    
  end
  for i=1, 4 do        
    local status = GameController.ui.mainInterface:GetSpellStatus(i)
    self.statuses[currentHero.name][i] = status
    if status.mode == "active" or status.mode == "recharge" then      
      currentHero:SetTimer(i, status.timeout)
    end
  end
end

function SpellSystem:LoadStatuses()
  local currentHero = GameController.player.playerController:GetCurrentSlot()
  if self.statuses[currentHero.name] then
    for i=1, 4 do
      GameController.ui.mainInterface:SetSpellStatus(i, self.statuses[currentHero.name][i])
      self.statuses[currentHero.name][i] = {}
    end
    currentHero.timeManager:Clear()
    self.statuses[currentHero.name] = nil
  end  
end

function SpellSystem:TimerUpdate(hero, action, timeout)  
  if self.statuses[hero] then
    self.statuses[hero][action]['timeout'] = timeout
  end
end

function SpellSystem:SetupSpells(spellNames)  
  GameController.ui.mainInterface:ResetSpells()  
  if spellNames ~= nil then    
    for k, v in pairs(spellNames) do
      local spell = GameController.database:Get('spells', v)
      GameController.ui.mainInterface:SetupSpell(spell.action, v, spell.icon, spell.recharge_timeout or 1, spell.useCursor, spell.action_timeout)      
    end
  end
  self:LoadStatuses()
end

return SpellSystem