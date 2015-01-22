using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class stringutils {
 StringUtils = {}
 public void StringUtils.split(str, sep){
     if(sep == null  ){ 
         sep = ",";
     }
     
     if(str == null  ){ 
         return {}
     }
  
     var parts = {} //parts array
     var first = 1;
     var ostart, oend = string.find(str, sep, first, true) //regexp disabled search
  
     while(ostart ){
         var part = string.sub(str, first, ostart - 1);
         table.insert(parts, part);
         first = oend + 1;
         ostart, oend = string.find(str, sep, first, true);
     }
  
     var part = string.sub(str, first);
     table.insert(parts, part);
  
     return parts;
 }}}