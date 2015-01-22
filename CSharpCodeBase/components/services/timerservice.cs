using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class timerservice {
 var TimeService = Class({init=function(self)
   this.timers = {}
 }})
 public void TimeService:Update(){
   for(key, value in pairs(this.timers) ){
     value:StableUpdate();
   }
 }
 public void TimeService:Add(component){
   table.insert(this.timers, component);
 }
 public void TimeService:Remove(component){
   for(key, value in pairs(this.timers) ){
     if(value == component  ){ 
       table.remove(this.timers, key);
       return;
     }
   }
 }
 return TimeService}}