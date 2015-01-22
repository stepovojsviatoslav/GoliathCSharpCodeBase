local Class = require 'utils.hump.class'
local FileUtils = require 'utils.translates.fileutils'
local readfile = function(name) 
  return FileUtils.GetFileContents(name)
end

local RecipeList = Class({init=function(self, filename)
  self._filename = luanet.PathUtils.GetPath(filename)
  if not self._filename then
    Console:Message("File " .. filename .. " not found!")
  end
  self.hashes = {}
  self.backhashes = {}
  self.descriptions = {}
  self.hides = {}
  self:LoadList()
end})

function RecipeList:LoadList()
  local data = readfile(self._filename)   
  for line in data:gmatch("[^\r\n]+") do    
    local recipe, description, parameter = unpack(StringUtils.split(line, ':'))
    local leftSide, result = recipe:match("^([a-zA-Z0-9_]+[+[a-zA-Z0-9_]+]*)=([a-zA-Z0-9_]+)$")    
    
    if leftSide and result then         
      local ingredients = StringUtils.split(leftSide, '+')             
      if #ingredients > 0 then                
        table.sort(ingredients)        
        self.hashes[leftSide] = result    
        self.backhashes[result] = ingredients      
      end
      if description then        
        self.descriptions[result] = description
      else
        self.descriptions[result] = ""
      end
      if parameter and parameter == "hide" then
        self.hides[result] = true
      else
        self.hides[result] = false
      end      
    end    
  end
end

function RecipeList:GetResult(...)  
  local arg = {...}
  table.sort(arg)  
  if #arg > 0 then
    local hash = ""
    for _, item in pairs(arg) do
      hash = hash .. item .. "+"
    end
    hash = hash:sub(1, #hash - 1)
    return self.hashes[hash]
  else
    return nil
  end
end

function RecipeList:GetIngredients(item)
  if self.backhashes[item] and #self.backhashes[item] > 0 then    
    local unicalItems = {}
    for k, v in pairs(self.backhashes[item]) do
      local containing = false
      for _, v1 in pairs(unicalItems) do
        if v1 == v then
          containing = true
          break
        end
      end
      if not containing then
        table.insert(unicalItems, v)
      end
    end
    return unicalItems
  else
    return {}
  end
end

function RecipeList:GetResultOnlyComponents(...)  
  local arg = {...}
  local haveInRecipe = false  
  local result = nil
  
  if #arg > 0 then        
    for k, v in pairs(self.backhashes) do         
      if #self:GetIngredients(k) == #arg then        
        for _, item in pairs(arg) do                
          haveInRecipe = false        
          for k1, ingredient in pairs(self:GetIngredients(k)) do
            if ingredient == item then
              haveInRecipe = true
              break
            end            
          end
          if not haveInRecipe then
            break                  
          end
        end      
        if haveInRecipe then        
          result = k
          break
        end
      end
    end    
  end
  return result
end

function RecipeList:GetBackhashResult(result)  
  return self.backhashes[result]
end

function RecipeList:GetNames()
  local result = {}
  for k, _ in pairs(self.backhashes) do
    table.insert(result, {k, GameController.locale[GameController.database:Get("items", k.."/description")]})
  end
  return result
end

function RecipeList:GetDescriptions()
  local result = {}
  for k, v in pairs(self.descriptions) do    
    result[k] = GameController.locale[v]
  end
  return result
end

function RecipeList:GetHides()
  return self.hides
end  

return RecipeList