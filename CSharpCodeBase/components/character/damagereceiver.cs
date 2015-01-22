using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class damagereceiver :component{
 var DamageReceiverComponent = Class({__includes=Component, init=function(self, entity)
       Component.init(self, entity);
       this.entity.damageReceiver = self;
       this.overrideSideResist = {front=-1,side=-1,back=-1}
 }})
 public void DamageReceiverComponent:SetOverrideSideResist(side, resist){
   this.overrideSideResist[side] = resist;
 }
 public void DamageReceiverComponent:DropOverrideSideResist(side){
   this.overrideSideResist[side] = -1;
 }
 public void DamageReceiverComponent:OnDamageReceive(damageData){
   var source = damageData.source;
   var damage = damageData.damage;
   damage = self:ProcessResist(damage);
   damage = self:ProcessSideResist(damage, source:GetForwardVector());
   var summaryDamage = self:GetSummaryDamage(damage);
   this.entity:Message("OnApplyDamage", {summary=summaryDamage, damage=damage, effects=damageData.effects, source=source})
   //print("Apply damage: " .. summaryDamage)
 }
 public void DamageReceiverComponent:GetSummaryDamage(damage){
   var sum = 0;
   for(k, v in pairs(damage) ){
     sum = sum + v;
   }
   return sum;
 }
 public void DamageReceiverComponent:ProcessResist(damage){
   var resistDamage = {}
   for(k, v in pairs(damage) ){
     resistDamage[k] = v * this.entity.resist:GetResist(k);
   }
   return resistDamage;
 }
 public void DamageReceiverComponent:ProcessSideResist(damage, sourceForward){
   var sideValue = self:GetSideResistValue(sourceForward);
   var sideResist = this.overrideSideResist[sideValue] > -1  &&  this.overrideSideResist[sideValue]  ||  this.entity.resist:GetSideResist(sideValue);
   var resistDamage = {}
   for(k, v in pairs(damage) ){
     resistDamage[k] = v * sideResist;
   }
   return resistDamage;
 }
 public void DamageReceiverComponent:GetSideResistValue(sourceForward){
   var myForward = this.entity:GetForwardVector();
   var sf2 = Vector2(sourceForward.x, sourceForward.z);
   var mf2 = Vector2(myForward.x, myForward.z);
   var ){tProduct = sf2:Dot(mf2)
   var angle = math.acos(dotProduct/(sf2:Length() * mf2:Length()));
   var degrees = angle * (180/math.pi);
   if(degrees < 45  &&  degrees > -45  ){ 
     return "back";
   }else{if degrees > 45  &&  degrees < 90 + 45  ){ 
     return "side";
   }else{
     return "front";
   }
 }
 Component}}