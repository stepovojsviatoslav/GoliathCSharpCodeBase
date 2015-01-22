local Vector3 = require 'utils.hump.vector3'

KeyCode = UnityEngine.KeyCode

Input = {}
Input.enabled = true

local _RaycastMouseOnTerrain = luanet.InputUtils.RaycastMouseOnTerrain
function Input.RaycastMouseOnTerrain()
  local result = {}
  _RaycastMouseOnTerrain(result)
  return Vector3(result.x, result.y, result.z)
end

local _RaycastTerrain = luanet.InputUtils.RaycastTerrain
function Input.RaycastTerrain(position)
  local result = {}
  _RaycastTerrain(result, position.x, position.y, position.z)
  return Vector3(result.x, result.y, result.z)
end

local _RaycastMouseOnTerrainWithHeight = luanet.InputUtils.RaycastMouseOnTerrainWithHeight
function Input.RaycastMouseOnTerrainWithHeight(height)
  local result = {}
  _RaycastMouseOnTerrainWithHeight(result, height)
  return Vector3(result.x, result.y, result.z)
end


local _RaycastTargetEntity = luanet.InputUtils.RaycastTargetEntity
function Input.RaycastTargetEntity()
  return _RaycastTargetEntity()
end

local _RaycastTargetEntityWithRadius = luanet.InputUtils.RaycastTargetEntityWithRadius
Input.RaycastTargetEntityWithRadius = _RaycastTargetEntityWithRadius

local _GetAxis = UnityEngine.Input.GetAxis
function Input.GetAxis(name)
  return _GetAxis(name)
end

local _GetKey = UnityEngine.Input.GetKey
function Input.GetKey(keycode)
  return Input.enabled and _GetKey(keycode)
end

function Input.GetKeyDown(keycode)
  return Input.enabled and UnityEngine.Input.GetKeyDown(keycode)
end

local _GetKeyDown = UnityEngine.Input.GetKeyDown
function Input.GetKeyDownUnlocked(keycode)
  return _GetKeyDown(keycode)
end

local _IsPointerOverUI = luanet.InputUtils.IsPointerOverUI
local _GetMouseButtonDown = UnityEngine.Input.GetMouseButtonDown
function Input.GetMouseButtonDown(keycode)
  return _GetMouseButtonDown(keycode) and not _IsPointerOverUI() 
end

function Input.IsPointerOverUI()
  return _IsPointerOverUI()
end

local _GetMouseButton = UnityEngine.Input.GetMouseButton
function Input.GetMouseButton(keycode)
  return _GetMouseButton(keycode) and not _IsPointerOverUI()
end


function Input.GetMouseScrollValue()
  local value = Input.GetAxis("Mouse ScrollWheel")
  if value ~= 0 and _IsPointerOverUI() then
    return 0
  end
  return value
end

local _GetTargetEntityForAction = luanet.InputUtils.GetTargetEntityForAction
function Input.GetTargetEntityForAction(vec3, radius)
  return _GetTargetEntityForAction(vec3.x, vec3.y, vec3.z, radius)
end