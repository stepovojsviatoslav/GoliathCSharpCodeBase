function math.clamp(low, n, high) return math.min(math.max(n, low), high) end
function math.lerp(a, b, k) 
  local result = a * (1-k) + b * k 
  if a < b and result > b then
    result = b
  elseif a > b and result > a then
    result = a
  end
  return result
end
function math.chance(percent) 
  return math.random() < (percent/100)
end
function math.sectomin(sec)
  return sec * 60
end

function math.sign(x)
   if x<0 then
     return -1
   elseif x>0 then
     return 1
   else
     return 0
   end
end