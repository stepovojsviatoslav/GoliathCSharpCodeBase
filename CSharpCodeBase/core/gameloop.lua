local Class = require 'utils.hump.class'

local GameLoop = Class({init = function (self, gameObject)
      self.items = {}
      self.affectQueue = {}
end})

function GameLoop:FixedUpdate()
  for k, v in pairs(self.items) do
    if v.enabled then
      v:FixedUpdate()
    end
  end
  self:ProcessAffectQueue()
end

function GameLoop:LateUpdate()
  for k, v in pairs(self.items) do
    if v.enabled then
      v:LateUpdate()
    end
  end
  self:ProcessAffectQueue()
end

function GameLoop:Update()
  --local updateCount = 0
  for k, v in pairs(self.items) do
    if v.enabled then
      --updateCount = updateCount + 1
      v:Update()
    end
  end    
  self:ProcessAffectQueue()
  --print(updateCount)
end

function GameLoop:Add(item)
  table.insert(self.affectQueue, {item, 1})
  --self.affectQueue[#self.affectQueue + 1] = {item, 1}
end

function GameLoop:AddForce(item)
  local idx = Tables.Find(self.items, item)
  if idx < 0 then
    table.insert(self.items, item)
    --self.items[#self.items + 1] = item
  end
end

function GameLoop:Remove(item)
  item.enabled = false
  self.affectQueue[#self.affectQueue + 1] = {item, 0}
end

function GameLoop:ProcessAffectQueue()
  if #self.affectQueue > 0 then
    for k, v in pairs(self.affectQueue) do
      if v[2] == 0 then
        self:RemoveForce(v[1])
      else
        self:AddForce(v[1])
      end
    end
    self.affectQueue = {}
  end
end

function GameLoop:RemoveForce(item)
  local idx = Tables.Find(self.items, item)
  if idx > -1 then
    table.remove(self.items, idx)
  end
end

return GameLoop