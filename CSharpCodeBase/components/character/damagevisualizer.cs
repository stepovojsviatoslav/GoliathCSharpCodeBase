using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class damagevisualizer :component{
 var DamageVisualizerComponent = Class({__includes=Component, init=function(self, entity)
       Component.init(self, entity);
       this.entity.damageReceiver = self;
 }})
 public void DamageVisualizerComponent:OnApplyDamage(damageData){
   var text = "";
   
   var maxType;
   var maxDamage;
   
   for(k, v in pairs(damageData.damage) ){
     if(maxType == null  ){ 
       maxType = k;
       maxDamage = v;
     }else{
       if(v > maxDamage  ){ 
         maxType = k;
         maxDamage = v;
       }
     }
   }
   
   // Get color
   var currentColor = self:GetColor(maxType);
   if(damageData.summary > 0  ){ 
     text = "-" .. damageData.summary;
     
     var critical = damageData.effects.critical  ||  false;
     if(critical  ){  
       text = text .. GameController.database:Get("damagecolors", "global/criticalAppend");
     }
   }else{
     text = "0"//GameController.database:Get("damagecolors", "global/blockedHit")
   }
   
   var pos = this.entity:GetPosition();
   luanet.GameFacade.uiDamageManager:Show(pos.x, pos.y + this.entity.height, pos.z, currentColor[1], currentColor[2], currentColor[3], text);
 }
 public void DamageVisualizerComponent:GetColor(dType){
   var defaultSection = GameController.database.data.damagecolors.default;
   var section = GameController.database.data.damagecolors[dType]  ||  defaultSection;
   return {section.r, section.g, section.b}
 }
 Component}}