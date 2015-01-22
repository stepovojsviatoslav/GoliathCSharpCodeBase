local Class = require 'utils.hump.class'
local UnityExistsEntity = require 'unity.unityentity'
local PlayerController = require 'components.character.playercontroller'
local GuiControl = require 'components.control.guicontrol'

local Player = Class({__includes = UnityExistsEntity, init = function (self, gameObject)
      UnityExistsEntity.init(self, gameObject)
      self:AddComponent(PlayerController)
      self:AddComponent(GuiControl)
      self.luaMapper:SetAlwaysVisible(true)
      self.static = false
end})

function Player:Update()
  UnityExistsEntity.Update(self)
end

function Player:FixedUpdate()
  UnityExistsEntity.FixedUpdate(self)
end

function Player:LateUpdate()
  UnityExistsEntity.LateUpdate(self)  
end

function Player:StoreHomePosition(position)
  self.homePosition = position
end

return Player