local Class = require 'utils.hump.class'
local AINode = require 'utils.ailib.ainode'
local AISequenceNode = require 'utils.ailib.aisequence'

local AI_Sleep = Class({__includes=AINode, init = function (self, entity, sleepPhases)
  AINode.init(self, entity)
  self.sleepPhases = sleepPhases or {GameController.daytime.PHASE_EVENING, GameController.daytime.PHASE_NIGHT}
end})

function AI_Sleep:CheckSleepPhase()
  return Tables.Find(self.sleepPhases, GameController.daytime.currentPhase) > -1
end

function AI_Sleep:Visit()
  if self.status == NODE_READY then
    if self:CheckSleepPhase() and self.entity:HasEmptyHitlist() and math.chance(75) then
      self.status = NODE_RUNNING
      self.entity:Sleep()
      self.sleepScanTimeout = self.entity.config:Get('sleepScanTimeout') or 5
    else
      self.status = NODE_FAILURE
      self.parent:Sleep(10)
    end
  end
  
  if self.status == NODE_RUNNING then
    self.sleepScanTimeout = self.sleepScanTimeout - GameController.deltaTime
    if self.sleepScanTimeout < 0 then
      self.sleepScanTimeout = self.entity.config:Get('sleepScanTimeout') or 5
      local targets = self.entity.vision:GetVisibleEntitiesByRSTag("enemy", self.entity.config:Get("visionRadius") * 0.3)
      if #targets > 0 then
        if math.chance(self.entity.config:Get('sleepStopChance') or 10) then
          self.status = NODE_SUCCESS
          self.entity:WakeUp()
          self.entity:ScanEnemies()
        end
      end
    end
    if not self:CheckSleepPhase() or not self.entity.isSleeping then
      self.status = NODE_SUCCESS
      --self.entity:WakeUp()
    end
  end
  return self.status
end

local AI_WakeUp = Class({__includes=AINode, init = function (self, entity)
  AINode.init(self, entity)
end})

function AI_WakeUp:Visit()
  if self.status == NODE_READY then
    if self.entity.isSleeping then
      self.entity:WakeUp()
      self.status = NODE_RUNNING
    else
      self.status = NODE_FAILURE
    end
  end
  
  if self.status == NODE_RUNNING then
    if self.entity.mecanim:CheckStateName('Idle') then
      print("Awakened!")
      self.status = NODE_SUCCESS
    end
  end
  return self.status
end

local AI_SleepCycle = Class({__includes=AISequenceNode, init = function (self, entity, sleepPhases)
      local childNodes = {
        AI_Sleep(entity, sleepPhases),
        AI_WakeUp(entity),
      }
      AISequenceNode.init(self, entity, childNodes)
end})


return AI_SleepCycle