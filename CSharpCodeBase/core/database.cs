using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class database {
 var Class = require("utils.hump.class");
 var inifile = require("utils.inifile");
 var inspect = require("utils.inspect");
 var public void }swith(s, send){
 return #s >= #send  &&  s:find(send, #s-#send+1, true)  &&  true  ||  false;
 }
 var public void split(inputstr, sep){
   if(sep == null  ){ 
           sep = "%s";
   }
   var t={} ; var i=1
   for(str in string.gmatch(inputstr, "([^"..sep.."]+)") ){
           t[i] = str;
           i = i + 1;
   }
   return t;
 }
 var Database = Class({init = public void (self){
      this.data = {}
 }})
 public void Load(path, table){
   if(table == null  ){ 
     table = string.gmatch(path, "%/(%w+)%.%w+")();
   }  
   Console:Message("Load " .. path)  ;
   var abspath = luanet.PathUtils.GetPath(path)  ;
   this.data[table] = inifile.parse(abspath);
   
   // collapse indexes to array
   var tbl = this.data[table];
   for(section, values in pairs(tbl) ){
     var addTable = {}
     var parentTable = null;
     for(key, value in pairs(values) ){
       // Process include
       if(key == "include"  ){ 
         parentTable = this.data[table][value];
       }
       // Process arrays
       if(endswith(key, "_0")  ){ 
         var newArray = {}
         var valueName = string.sub(key, 0, -3);
         for(i = 0,100 ){
           var currentName = valueName .. "_" .. tostring(i);
           if(values[currentName] != null  ){ 
             newArray[#newArray + 1] = values[currentName];
           }else{
             break;
           }
         }
         addTable[valueName] = newArray;
       }
     }
     if(parentTable != null  ){ 
       for(k, v in pairs(parentTable) ){
         if(values[k] == null  ){ 
           values[k] = v;
         }
       }
     }
     for(k, v in pairs(addTable) ){
       values[k] = v;
     }
   }
 }
 public void Get(table, path){
   var tbl = this.data[table];
   var chunks = split(path, "/") ;
   var t = tbl;
   var cur = t;
   for(k,v in pairs(chunks) ){
     if(cur == null  ){  break }
     cur = cur[v];
   }
   return cur;
 }
 public void Set(table, path, value){
   var chunks = split(path, "/") ;
   var t = this.data[table];
   var cur = t;
   for(k,v in pairs(chunks) ){
     if(cur == null  ){  break }
     if(k == #chunks  ){ 
       cur[v] = value;
     }else{
       cur = cur[v];
     }
   }
 }
 }}