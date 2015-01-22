local Class = require 'utils.hump.class'

local Vector3 = Class({init = function(self, x, y, z)
    self.x, self.y, self.z = x or 0, y or 0, z or 0
end})

function Vector3:__add( rhs )
    if self == nil then
      print(debug.traceback())
    end
    return Vector3( self.x + rhs.x, self.y + rhs.y, self.z + rhs.z)
end

function Vector3:__sub( rhs )
    return Vector3( self.x - rhs.x, self.y - rhs.y, self.z - rhs.z)
end

function Vector3:__mul( rhs )
    return Vector3( self.x * rhs, self.y * rhs, self.z * rhs)
end

function Vector3:__div( rhs )
    return Vector3( self.x / rhs, self.y / rhs, self.z / rhs)
end

function Vector3:Dot( rhs )
    return self.x * rhs.x + self.y * rhs.y + self.z * rhs.z
end

function Vector3:Cross( rhs )
    return Vector3( self.y * rhs.z - self.z * rhs.y,
                    self.z * rhs.x - self.x * rhs.z,
                    self.x * rhs.y - self.y * rhs.x)
end

function Vector3:__tostring()
    return string.format("(%2.2f, %2.2f, %2.2f)", self.x, self.y, self.z) 
end

function Vector3:__eq( rhs )
    return self.x == rhs.x and self.y == rhs.y and self.z == rhs.z
end

function Vector3:DistSq(other)
    return (self.x - other.x)*(self.x - other.x) + (self.y - other.y)*(self.y - other.y) + (self.z - other.z)*(self.z - other.z)
end

function Vector3:Dist(other)
    return math.sqrt(self:DistSq(other))
end

function Vector3.Distance(v1, v2)
  return v1:Dist(v2)
end

function Vector3:LengthSq()
    return self.x*self.x + self.y*self.y + self.z*self.z
end

function Vector3:Length()
    return math.sqrt(self:LengthSq())
end

function Vector3:Normalize()
    local len = self:Length()
    if len > 0 then
        self.x = self.x / len
        self.y = self.y / len
        self.z = self.z / len
    end
    return self
end

function Vector3:RotateAroundY(a)
  local tmpx = self.x
  local tmpz = self.z
  self.x = math.cos(a) * tmpx - math.sin(a) * tmpz
  self.z = math.sin(a) * tmpx + math.cos(a) * tmpz
end

function Vector3:GetNormalized()
    return self / self:Length()
end

function Vector3:Get()
    return self.x, self.y, self.z
end

function Vector3:Zero()
  self.x = 0
  self.y = 0
  self.z = 0
end

function Vector3.Lerp(start, final, percent)
  dv = final - start
  dv = dv * percent
  return start + dv
end

function Vector3:Clone()
  return Vector3(self.x, self.y, self.z)
end

function Vector3:Randomize(x, y, z)
  self.x = self.x + math.random() * x - x/2
  self.y = self.y + math.random() * y - y/2
  self.z = self.z + math.random() * z - z/2
end
function Vector3.GetRandom(ax, ay, az)
  return Vector3(math.random() * ax - ax/2,
    math.random() * ay - ay/2,
    math.random() * az - az/2)
end

return Vector3