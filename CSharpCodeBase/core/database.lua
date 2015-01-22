local Class = require('utils.hump.class')
local inifile = require('utils.inifile')
local inspect = require('utils.inspect')

local function endswith(s, send)
return #s >= #send and s:find(send, #s-#send+1, true) and true or false
end

local function split(inputstr, sep)
  if sep == nil then
          sep = "%s"
  end
  local t={} ; local i=1
  for str in string.gmatch(inputstr, "([^"..sep.."]+)") do
          t[i] = str
          i = i + 1
  end
  return t
end

local Database = Class({init = function (self)
     self.data = {}
end})

function Database:Load(path, table)
  if table == nil then
    table = string.gmatch(path, "%/(%w+)%.%w+")()
  end  
  Console:Message("Load " .. path)  
  local abspath = luanet.PathUtils.GetPath(path)  
  self.data[table] = inifile.parse(abspath)
  
  -- collapse indexes to array
  local tbl = self.data[table]
  for section, values in pairs(tbl) do
    local addTable = {}
    local parentTable = nil
    for key, value in pairs(values) do
      -- Process include
      if key == 'include' then
        parentTable = self.data[table][value]
      end
      -- Process arrays
      if endswith(key, '_0') then
        local newArray = {}
        local valueName = string.sub(key, 0, -3)
        for i = 0,100 do
          local currentName = valueName .. "_" .. tostring(i)
          if values[currentName] ~= nil then
            newArray[#newArray + 1] = values[currentName]
          else
            break
          end
        end
        addTable[valueName] = newArray
      end
    end
    if parentTable ~= nil then
      for k, v in pairs(parentTable) do
        if values[k] == nil then
          values[k] = v
        end
      end
    end
    for k, v in pairs(addTable) do
      values[k] = v
    end
  end
end

function Database:Get(table, path)
  local tbl = self.data[table]
  local chunks = split(path, '/') 
  local t = tbl
  local cur = t
  for k,v in pairs(chunks) do
    if cur == nil then break end
    cur = cur[v]
  end
  return cur
end

function Database:Set(table, path, value)
  local chunks = split(path, '/') 
  local t = self.data[table]
  local cur = t
  for k,v in pairs(chunks) do
    if cur == nil then break end
    if k == #chunks then
      cur[v] = value
    else
      cur = cur[v]
    end
  end
end

return Database