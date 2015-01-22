using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class events {
 var EventSystem = Class({init = public void (self){
       this.handlers = {}
 }})
 public void EventSystem:AddListener(event, listener){
   if(this.handlers[event] == null  ){ 
     this.handlers[event] = {}
   }
   var idx = Tables.Find(this.handlers[event], listener);
   if(idx < 0  ){ 
     this.handlers[event][#this.handlers[event] + 1] = listener;
   }
 }
 public void EventSystem:RemoveListener(event, listener){
   if(this.handlers[event] == null  ){ 
     this.handlers[event] = {}
   }
   var idx = Tables.Find(this.handlers[event], listener);
   table.remove(this.handlers[event], listener);
 }
 public void EventSystem:Event(e, params){
   if(this.handlers[e] != null  ){ 
     var handlers = this.handlers[e];
     for(k, listener in pairs(handlers) ){
       if(listener.OnEvent != null  ){ 
         listener:OnEvent(e, params);
       }else{
         listener(e, params);
       }
     }
   }
 }
 ystem}}