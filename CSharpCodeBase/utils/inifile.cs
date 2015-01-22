using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class inifile {
 // Copyright 2011 Bart van Strien. All rights reserved.
 // 
 // Redistribution  &&  use in source  &&  binary forms, with  ||  without modification, are
 // permitted provided that the following conditions are met:
 // 
 //    1. Redistributions of source code must retain the above copyright notice, this list of
 //       conditions  &&  the following disclaimer.
 // 
 //    2. Redistributions in binary form must reproduce the above copyright notice, this list
 //       of conditions  &&  the following disclaimer in the ){cumentation  && /or other materials
 //       provided with the distribution.
 // 
 // THIS SOFTWARE IS PROVIDED BY BART VAN STRIEN ""AS IS"" AND ANY EXPRESS OR IMPLIED
 // WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
 // FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL BART VAN STRIEN OR
 // CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 // CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 // SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
 // ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 // NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
 // ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 // 
 // The views  &&  conclusions contained in the software  &&  ){cumentation are those of the
 // authors  &&  should not be interpreted as representing official policies, either expressed
 //  ||  implied, of Bart van Strien.
 //
 // The above license is known as the Simplified BSD license.
 inifile = {}
 var readfile = function(name) ;
   return FileUtils.GetFileContents(name);
 }
 var public void trim(s){
   return (s:gsub("^%s*(.-)%s*$", "%1"));
 }
 public void inifile.parse(name){
 	local t = {}
 	local section;
   var data = readfile(name);
   for(line in data:gmatch("[^\r\n]+") ){	
 		local s = line:match("^%[([^%//+)%]$")
 		if s  ){ 
 			section = s;
 			t[section] = t[section]  ||  {}
 		end;
 		local key, value = line:match("^([_%w]+)%s-=%s-(.+)$");
 		if key  &&  value  ){             
       key = trim(key);
       value = trim(value);
 			if tonumber(value)  ){  value = tonumber(value) }
 			if value == "true"  ){  value = true }
 			if value == "false"  ){  value = false }
 			t[section][key] = value       ;
 		end;
 	end;
 	return t;
 }
 public void inifile.save(name, t){
 	local contents = "";
 	for section, s in pairs(t) ){
 		contents = contents .. ("[%s]\n"):format(section);
 		for key, value in pairs(s) ){
 			contents = contents .. ("%s=%s\n"):format(key, tostring(value));
 		end;
 		contents = contents .. "\n";
 	end;
 	write(name, contents);
 }
 }}