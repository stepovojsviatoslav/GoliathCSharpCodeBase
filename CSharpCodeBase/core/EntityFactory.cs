using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MainGame.core
{
    class EntityFactory
    {
        //local Class = require 'utils.hump.class'

        public void LoadFromStorage(storage, chunk)
        {
          String eType = storage:GetString('type')
          local class = require(GameController.database.data.prefabs[eType].require)
          local gameObject = GameController.pooler:Fetch(eType)
          local entity = class(gameObject)
          entity:Load(storage)
          entity._pooled = true
          entity._type = eType
          entity._inWorld = true
  
          if not entity.static then
            GameController.worldController:CheckForMigrations(entity.gameObject, chunk)
          end
  
          GameController:AddEntity(entity)
          return entity
        }

function EntityFactory:Create(eType, position)
  local class = require(GameController.database.data.prefabs[eType].require)
  local gameObject = GameController.pooler:Fetch(eType)
  local entity = class(gameObject)
  entity._pooled = true
  entity._type = eType
  entity._inWorld = false
  entity:SetPosition(position)
  GameController:AddEntity(entity)
  return entity  
end

function EntityFactory:CreateInWorld(eType, position)
  local class = require(GameController.database.data.prefabs[eType].require)
  local gameObject = GameController.pooler:Fetch(eType)
  local entity = class(gameObject)
  entity._type = eType
  entity._pooled = true
  entity:SetPosition(position)
  GameController:AddEntity(entity)
  GameController:AddToWorld(entity)
  return entity
end

function EntityFactory:Destroy(entity)
  GameController:RemoveEntity(entity)
  if entity._inWorld then
    GameController:RemoveFromWorld(entity)
  end
  if entity._pooled then
    GameController.pooler:Release(entity.gameObject)
  else
    luanet.UnityEngine.GameObject.Destroy(entity.gameObject)
  end
end
    }
}
