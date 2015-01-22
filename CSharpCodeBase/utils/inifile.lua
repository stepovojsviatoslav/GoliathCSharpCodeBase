-- Copyright 2011 Bart van Strien. All rights reserved.
-- 
-- Redistribution and use in source and binary forms, with or without modification, are
-- permitted provided that the following conditions are met:
-- 
--    1. Redistributions of source code must retain the above copyright notice, this list of
--       conditions and the following disclaimer.
-- 
--    2. Redistributions in binary form must reproduce the above copyright notice, this list
--       of conditions and the following disclaimer in the documentation and/or other materials
--       provided with the distribution.
-- 
-- THIS SOFTWARE IS PROVIDED BY BART VAN STRIEN ''AS IS'' AND ANY EXPRESS OR IMPLIED
-- WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
-- FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL BART VAN STRIEN OR
-- CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
-- CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
-- SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
-- ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
-- NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
-- ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
-- 
-- The views and conclusions contained in the software and documentation are those of the
-- authors and should not be interpreted as representing official policies, either expressed
-- or implied, of Bart van Strien.
--
-- The above license is known as the Simplified BSD license.

inifile = {}
local FileUtils = require 'utils.translates.fileutils'

local readfile = function(name) 
  return FileUtils.GetFileContents(name)
end

local function trim(s)
  return (s:gsub("^%s*(.-)%s*$", "%1"))
end

function inifile.parse(name)
	local t = {}
	local section
  local data = readfile(name)
  for line in data:gmatch("[^\r\n]+") do	
		local s = line:match("^%[([^%]]+)%]$")
		if s then
			section = s
			t[section] = t[section] or {}
		end
		local key, value = line:match("^([_%w]+)%s-=%s-(.+)$")
		if key and value then            
      key = trim(key)
      value = trim(value)
			if tonumber(value) then value = tonumber(value) end
			if value == "true" then value = true end
			if value == "false" then value = false end
			t[section][key] = value       
		end
	end
	return t
end

function inifile.save(name, t)
	local contents = ""
	for section, s in pairs(t) do
		contents = contents .. ("[%s]\n"):format(section)
		for key, value in pairs(s) do
			contents = contents .. ("%s=%s\n"):format(key, tostring(value))
		end
		contents = contents .. "\n"
	end
	write(name, contents)
end

return inifile