using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class resist :component{
 var ResistComponent = Class({__includes=Component, init = public void (self, entity){
       Component.init(self, entity);
       self:LoadResist();
       self:LoadSideResist();
       this.entity.resist = self;
 }})
 public void ResistComponent:LoadResist(resist){
   var resistString = resist  ||  this.entity.config:Get("resist");
   this.resist = {}
   if(resistString != null  ){ 
     for(dType, dValue in string.gmatch(resistString, "([%w%*]+)%:(%d+)%,?") ){
       this.resist[dType] = dValue/100.0;
     }
   }
   if(this.resist["*"] == null  ){ 
     this.resist["*"] = 1;
   }
 }
 public void ResistComponent:LoadSideResist(){
   var resistString = this.entity.config:Get("sideResist");
   this.sideResist = {}
   if(resistString != null  ){ 
     for(dType, dValue in string.gmatch(resistString, "([%w%*]+)%:(%d+)%,?") ){
       this.sideResist[dType] = dValue/100.0;
     }
   }
   if(this.sideResist["*"] == null  ){ 
     this.sideResist["*"] = 1;
   }
 }
 public void ResistComponent:GetResist(damageType){
   var value = this.resist[damageType];
   return value != null  &&  value  ||  this.resist["*"];
 }
 public void ResistComponent:GetSideResist(sideType){
   var value = this.sideResist[sideType];
   return value != null  &&  value  ||  this.sideResist["*"];
 }
 Component}}