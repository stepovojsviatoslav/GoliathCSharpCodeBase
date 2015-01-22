using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class inspect {
 var inspect ={
   _VERSION = "inspect.lua 3.0.0",;
   _URL     = "http://github.com/kikito/inspect.lua",
   _DESCRIPTION = "human-readable representations of tables",;
   _LICENSE = //
     MIT LICENSE;
     Copyright (c) 2013 Enrique García Cota;
     Permission is hereby granted, free of charge, to any person obtaining a;
     copy of this software  &&  associated ){cumentation files (the
     "Software"), to deal in the Software without restriction, including;
     without limitation the rights to use, copy, modify, merge, publish,;
     distribute, sublicense,  && /or sell copies of the Software,  &&  to;
     permit persons to whom the Software is furnished to ){ so, subject to
     the following conditions:;
     The above copyright notice  &&  this permission notice shall be included;
     in all copies  ||  substantial portions of the Software.;
     THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS;
     OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF;
     MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.;
     IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY;
     CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,;
     TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE;
     SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.;
   //
 }
 inspect.KEY       = setmetatable({}, {__tostring = function() return "inspect.KEY" }})
 inspect.METATABLE = setmetatable({}, {__tostring = function() return "inspect.METATABLE" }})
 // Apostrophizes the string if(it has quotes, but not aphostrophes
 // Otherwise, it returns a regular quoted string
 var public void smartQuote(str){
   if(str:match(""")  &&  not str:match(""")  ){ 
     return """ .. str .. """;
   }
   return """ .. str:gsub(""", "\\"") .. """;
 }
 var controlCharsTranslation = {
   ["\a"] = "\\a",  ["\b"] = "\\b", ["\f"] = "\\f",  ["\n"] = "\\n",;
   ["\r"] = "\\r",  ["\t"] = "\\t", ["\v"] = "\\v";
 }
 var public void escapeChar(c) return controlCharsTranslation[c] }{
 var public void escape(str){
   var result = str:gsub("\\", "\\\\"):gsub("(%c)", escapeChar);
   return result;
 }
 var public void isIdentifier(str){
   return type(str) == "string"  &&  str:match( "^[_%a][_%a%d]*$" );
 }
 var public void isSequenceKey(k, length){
   return type(k) == "number";
       &&  1 <= k;
       &&  k <= length;
       &&  math.floor(k) == k;
 }
 var defaultTypeOrders = {
   ["number"]   = 1, ["boolean"]  = 2, ["string"] = 3, ["table"] = 4,;
   ["function"] = 5, ["userdata"] = 6, ["thread"] = 7;
 }
 var public void sortKeys(a, b){
   var ta, tb = type(a), type(b);
   // strings  &&  numbers are sorted numerically/alphabetically
   if(ta == tb  &&  (ta == "string"  ||  ta == "number")  ){  return a < b }
   var dta, dtb = defaultTypeOrders[ta], defaultTypeOrders[tb];
   // Two default types are compared according to the defaultTypeOrders table
   if(dta  &&  dtb  ){  return defaultTypeOrders[ta] < defaultTypeOrders[tb]
   }else{if dta      ){  return true  // default types before custom ones
   }else{if dtb      ){  return false // custom types after default ones
   }
   // custom types are sorted out alphabetically
   return ta < tb;
 }
 var public void getNonSequentialKeys(t){
   var keys, length = {}, #t
   for(k,_ in pairs(t) ){
     if(not isSequenceKey(k, length)  ){  table.insert(keys, k) }
   }
   table.sort(keys, sortKeys);
   return keys;
 }
 var public void getToStringResultSafely(t, mt){
   var __tostring = type(mt) == "table"  &&  rawget(mt, "__tostring");
   var str, ok;
   if(type(__tostring) == "function"  ){ 
     ok, str = pcall(__tostring, t);
     str = ok  &&  str  ||  "error: " .. tostring(str);
   }
   if(type(str) == "string"  &&  #str > 0  ){  return str }
 }
 var maxIdsMetaTable = {
   __index = function(self, typeName);
     rawset(self, typeName, 0);
     return 0;
   }
 }
 var idsMetaTable = {
   __index = public void (self, typeName){
     var col = setmetatable({}, {__mode = "kv"})
     rawset(self, typeName, col);
     return col;
   }
 }
 var public void countTableAppearances(t, tableAppearances){
   tableAppearances = tableAppearances  ||  setmetatable({}, {__mode = "k"})
   if(type(t) == "table"  ){ 
     if(not tableAppearances[t]  ){ 
       tableAppearances[t] = 1;
       for(k,v in pairs(t) ){
         countTableAppearances(k, tableAppearances);
         countTableAppearances(v, tableAppearances);
       }
       countTableAppearances(getmetatable(t), tableAppearances);
     }else{
       tableAppearances[t] = tableAppearances[t] + 1;
     }
   }
   return tableAppearances;
 }
 var copySequence = function(s);
   var copy, len = {}, #s
   for(i=1, len ){ copy[i] = s[i] }
   return copy, len;
 }
 var public void makePath(path, ...){
   var keys = {...}
   var newPath, len = copySequence(path);
   for(i=1, #keys ){
     newPath[len + i] = keys[i];
   }
   return newPath;
 }
 var public void processRecursive(process, item, path){
   if(item == null  ){  return null }
   var processed = process(item, path);
   if(type(processed) == "table"  ){ 
     var processedCopy = {}
     var processedKey;
     for(k,v in pairs(processed) ){
       processedKey = processRecursive(process, k, makePath(path, k, inspect.KEY));
       if(processedKey != null  ){ 
         processedCopy[processedKey] = processRecursive(process, v, makePath(path, processedKey));
       }
     }
     var mt  = processRecursive(process, getmetatable(processed), makePath(path, inspect.METATABLE));
     setmetatable(processedCopy, mt);
     processed = processedCopy;
   }
   return processed;
 }
 //////////////////////////////////////////////////////////////////-
 var Inspector = {}
 var Inspector_mt = {__index = Inspector}
 public void Inspector:puts(...){
   var args   = {...}
   var buffer = this.buffer;
   var len    = #buffer;
   for(i=1, #args ){
     len = len + 1;
     buffer[len] = tostring(args[i]);
   }
 }
 public void Inspector:down(f){
   this.level = this.level + 1;
   f();
   this.level = this.level - 1;
 }
 public void Inspector:tabify(){
   self:puts(this.newline, string.rep(this.indent, this.level));
 }
 public void Inspector:alreadyVisited(v){
   return this.ids[type(v)][v] != null;
 }
 public void Inspector:getId(v){
   var tv = type(v);
   var id = this.ids[tv][v];
   if(not id  ){ 
     id              = this.maxIds[tv] + 1;
     this.maxIds[tv] = id;
     this.ids[tv][v] = id;
   }
   return id;
 }
 public void Inspector:putKey(k){
   if(isIdentifier(k)  ){  return self:puts(k) }
   self:puts("[");
   self:putValue(k);
   self:puts("]");
 }
 public void Inspector:putTable(t){
   if(t == inspect.KEY  ||  t == inspect.METATABLE  ){ 
     self:puts(tostring(t));
   }else{if self:alreadyVisited(t)  ){ 
     self:puts("<table ", self:getId(t), ">");
   }else{if this.level >= this.depth  ){ 
     self:puts("{...}")
   }else{
     if(this.tableAppearances[t] > 1  ){  self:puts("<", self:getId(t), ">") }
     var nonSequentialKeys = getNonSequentialKeys(t);
     var length            = #t;
     var mt                = getmetatable(t);
     var toStringResult    = getToStringResultSafely(t, mt);
     self:puts("{")
     self:down(function();
       if(toStringResult  ){ 
         self:puts(" // ", escape(toStringResult))
         if(length >= 1  ){  self:tabify() }
       }
       var count = 0;
       for(i=1, length ){
         if(count > 0  ){  self:puts(",") }
         self:puts(" ");
         self:putValue(t[i]);
         count = count + 1;
       }
       for(_,k in ipairs(nonSequentialKeys) ){
         if(count > 0  ){  self:puts(",") }
         self:tabify();
         self:putKey(k);
         self:puts(" = ");
         self:putValue(t[k]);
         count = count + 1;
       }
       if(mt  ){ 
         if(count > 0  ){  self:puts(",") }
         self:tabify();
         self:puts("<metatable> = ");
         self:putValue(mt);
       }
     })
     if(#nonSequentialKeys > 0  ||  mt  ){  // result is multi-lined. Justify closing }
       self:tabify();
     }else{if length > 0  ){  // array tables have one extra space before closing }
       self:puts(" ");
     }
     self:puts("}")
   }
 }
 public void Inspector:putValue(v){
   var tv = type(v);
   if(tv == "string"  ){ 
     self:puts(smartQuote(escape(v)));
   }else{if tv == "number"  ||  tv == "boolean"  ||  tv == "nil"  ){ 
     self:puts(tostring(v));
   }else{if tv == "table"  ){ 
     self:putTable(v);
   }else{
     self:puts("<",tv," ",self:getId(v),">");
   }
 }
 //////////////////////////////////////////////////////////////////-
 public void inspect.inspect(root, options){
   options       = options  ||  {}
   var depth   = options.depth    ||  math.huge;
   var newline = options.newline  ||  "\n";
   var indent  = options.indent   ||  "  ";
   var process = options.process;
   if(process  ){ 
     root = processRecursive(process, root, {})
   }
   var inspector = setmetatable({
     depth            = depth,;
     buffer           = {},
     level            = 0,;
     ids              = setmetatable({}, idsMetaTable),
     maxIds           = setmetatable({}, maxIdsMetaTable),
     newline          = newline,;
     indent           = indent,;
     tableAppearances = countTableAppearances(root);
   }, Inspector_mt)
   inspector:putValue(root);
   return table.concat(inspector.buffer);
 }
 setmetatable(inspect, { __call = function(_, ...) .inspect(...) } })
 }}