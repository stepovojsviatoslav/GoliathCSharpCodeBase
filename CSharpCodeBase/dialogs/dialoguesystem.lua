local Class = require 'utils.hump.class'

local DialogueSystem = Class({init=function(self)
  self.says = {}
  self.answers = {}
  self.dialogueUnity = luanet.GameFacade.dialogueSystem
  self.dialogueUnity:SetAnswerCallback(function (id)
      self:OnAnswerCallback(id)
  end)
end})

function DialogueSystem:LoadDialogue(dialogue)
  self:ResetBuffer()
  self.current = coroutine.create(require(dialogue))
  self.isRunning = false
end

function DialogueSystem:Show(dialogue)
  self:LoadDialogue(dialogue)
  self:Step()
  self.dialogueUnity:Show(self.says[1], self.answers[1], self.answers[2], self.answers[3], self.answers[4])
  luanet.TimeUtils.SetTimeScale(0)
  GameController.inputService:PushFrame("dialogue")
end

function DialogueSystem:ResetBuffer()
  self.says = {}
  self.answers = {}
end

function DialogueSystem:Say(phrase)
  self.says[#self.says + 1] = phrase
end

function DialogueSystem:Answer(phrase)
  self.answers[#self.answers + 1] = phrase
end

function DialogueSystem:OnAnswerCallback(id)
  self:Step(id)
  if not self:IsFinished() then
    self.dialogueUnity:Show(self.says[1], self.answers[1], self.answers[2], self.answers[3], self.answers[4])
  else
    self.dialogueUnity:Hide()
    GameController.inputService:PopFrame()
    luanet.TimeUtils.SetTimeScale(1)
    self.current = nil
  end
end

function DialogueSystem:Update()
  if self.current ~= nil then
    local lookVector = GameController.inputService:GetLookValue("dialogue")
    lookVector.x = 0
    if lookVector:Length() > 0.3 then
      if not self.switch then
        if lookVector.z > 0 then
          self.dialogueUnity:SelectDown()
        else  
          self.dialogueUnity:SelectUp()
        end
      end
      self.switch = true
    else
      self.switch = false
    end
    
    if GameController.inputService:LeftButtonWasPressed("dialogue") then
      self.dialogueUnity:JoysticApprove()
    end
  end
end

function DialogueSystem:Step(data)
  self:ResetBuffer()
  if self.current ~= nil then
    if not self.isRunning then
      coroutine.resume(self.current, self) 
      self.isRunning = true
    else
      coroutine.resume(self.current, data) 
    end
    return coroutine.status(self.current) ~= 'dead'
  else
    return false
  end
end

function DialogueSystem:IsFinished()
  return self.current == nil or coroutine.status(self.current) == 'dead'
end

return DialogueSystem