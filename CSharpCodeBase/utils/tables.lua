Tables = {}

Tables.DeepCopy = function(orig)
    local orig_type = type(orig)
    local copy
    if orig_type == 'table' then
        copy = {}
        for orig_key, orig_value in next, orig, nil do
            copy[Tables.DeepCopy(orig_key)] = Tables.DeepCopy(orig_value)
        end
        setmetatable(copy, Tables.DeepCopy(getmetatable(orig)))
    else -- number, string, boolean, etc
        copy = orig
    end
    return copy
end

Tables.Inherit = function(tParent, tChild)
  result = Tables.DeepCopy(tParent)
  -- And move every tChild key to tParent
  Tables.CopyTo(tChild, result)
  return result
end

Tables.Length = function(t)
  local count = 0
  for _ in pairs(t) do
    count = count + 1
  end
  return count
end

Tables.CopyTo = function (source, target)
  for k, v in pairs(source) do
    if type(v) == 'table' then
      if target[k] == nil then target[k] = {} end
      Tables.CopyTo(v, target[k])
    else
      target[k] = v
    end
  end
end

Tables.Find = function(table, item)
  if table == {} or table == nil then
    return -1
  end
  for k, v in pairs(table) do
    if v == item then
      return k
    end
  end
  return -1
end