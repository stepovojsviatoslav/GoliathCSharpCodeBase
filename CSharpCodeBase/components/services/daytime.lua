local Class = require 'utils.hump.class'

local DayTime = Class({init=function(self)
      
    self.PHASE_MORNING = 1
    self.PHASE_DAY_START = 2
    self.PHASE_DAY_PROGRESS = 3
    self.PHASE_DAY_END = 4
    self.PHASE_EVENING = 5
    self.PHASE_NIGHT = 6
    local configPhases = {'morning', 'daystart', 'day', 'dayend', 'evening', 'night'}
    
    self.currentDay = 0
    self.phaseTimes = {}
    self.phaseColors = {}
    self.phaseAmbientColors = {}
    
    for k, v in pairs(configPhases) do
      self.phaseTimes[k] = GameController.database:Get('daytime', v .. '/time')
      self.phaseColors[k] = GameController.database:Get('daytime', v .. '/color')
      self.phaseAmbientColors[k] = GameController.database:Get('daytime', v .. '/acolor')
    end
    self.currentPhase = self.PHASE_MORNING
    self.currentPhaseTime = self.phaseTimes[self.currentPhase]
    self.currentPhaseTimeRunning = 0
    self:OnLoadPhase(self.currentPhase)
end})

function DayTime:Update()
  self.currentPhaseTimeRunning = self.currentPhaseTimeRunning + GameController.deltaTime
  self.currentPhaseTime = self.currentPhaseTime - GameController.deltaTime
  if self.currentPhaseTime <= 0 then
    self:SwitchPhase()
  end
end

function DayTime:SwitchPhase()
  local previousPhase = self.currentPhase
  self.currentPhase = self.currentPhase + 1
  if self.currentPhase > self.PHASE_NIGHT then
    self.currentPhase = self.PHASE_MORNING
    self.currentDay = self.currentDay + 1
  end
  self.currentPhaseTime = self.phaseTimes[self.currentPhase]
  self.currentPhaseTimeRunning = 0
  self:OnUnloadPhase(previousPhase)
  self:OnLoadPhase(self.currentPhase)
end

function DayTime:OnUnloadPhase(phase)
  
end

function DayTime:OnLoadPhase(phase)
  print("Current phase " .. phase)
  GameController.eventSystem:Event('DAY_PHASE_CHANGED',
    {
      current = phase,
      color=self.phaseColors[phase],
      ambientColor=self.phaseAmbientColors[phase],
    })
end

return DayTime