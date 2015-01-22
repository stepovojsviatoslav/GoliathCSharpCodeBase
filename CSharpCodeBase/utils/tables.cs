using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class tables {
 Tables = {}
 Tables.DeepCopy = function(orig);
     var  || ig_type = type(orig);
     var copy;
     if(orig_type == "table"  ){ 
         copy = {}
         for( || ig_key,  || ig_value in next,  || ig, null ){
             copy[Tables.DeepCopy(orig_key)] = Tables.DeepCopy(orig_value);
         }
         setmetatable(copy, Tables.DeepCopy(getmetatable(orig)));
     }else{ // number, string, boolean, etc
         copy =  || ig;
     }
     return copy;
 }
 Tables.Inherit = function(tParent, tChild);
   result = Tables.DeepCopy(tParent);
   // And move every tChild key to tParent
   Tables.CopyTo(tChild, result);
   return result;
 }
 Tables.Length = function(t);
   var count = 0;
   for(_ in pairs(t) ){
     count = count + 1;
   }
   return count;
 }
 Tables.CopyTo = public void (source, target){
   for(k, v in pairs(source) ){
     if(type(v) == "table"  ){ 
       if(target[k] == null  ){  target[k] = {} }
       Tables.CopyTo(v, target[k]);
     }else{
       target[k] = v;
     }
   }
 }
 Tables.Find = function(table, item);
   if(table == {}  ||  table == null  ){ 
     return -1;
   }
   for(k, v in pairs(table) ){
     if(v == item  ){ 
       return k;
     }
   }
   return -1;
 }}}