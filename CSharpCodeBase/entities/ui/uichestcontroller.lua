local Class = require 'utils.hump.class'
local UnityExistsEntity = require 'unity/unityentity'

local UIChestController = Class({__includes=UnityExistsEntity, init=function(self, gameObject)            
      UnityExistsEntity.init(self, gameObject)         
      GameController.eventSystem:AddListener("CHEST_PANEL_ACTIVATE", self)
      self.luaMapper:SetAlwaysVisible(true)
      self.chestController = gameObject:GetComponent("UIChestController")            
      self.chestController:Toggle(false)
      self.chestController:SetupLuaController(self)
      self.chestController:SetCallback(self.Callback)          
end})

function UIChestController:Update()
  UnityExistsEntity.Update(self)  
  if Input.GetKeyDown(KeyCode.Escape) then
    self:OnHide()
  end
end

function UIChestController:OnShow()
  self.chestController:Toggle(true)
  local mainInterface = GameController.ui.mainInterface
  mainInterface:AddContainer("chest", self.chestEntity)
  mainInterface.unityInterface:ToggleInventory(true)  
end

function UIChestController:OnHide()
  self.chestController:Toggle(false)
  local mainInterface = GameController.ui.mainInterface
  mainInterface:RemoveContainer("chest")
  mainInterface.unityInterface:ToggleInventory(false)
end

function UIChestController:OnEvent(event, source)
  if event == "CHEST_PANEL_ACTIVATE" then
    self:Initiate(source)
    self:OnShow()    
  end
end

function UIChestController:Initiate(entity)
  self.chestEntity = entity
end

-- Callbacks
function UIChestController:Callback(method, ...)
  if self[method] ~= nil then
    self[method](self, ...)
  end
end

return UIChestController