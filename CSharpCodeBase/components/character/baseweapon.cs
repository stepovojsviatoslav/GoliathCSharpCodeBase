using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class baseweapon {
 public void init (self, entity, name);
   this.name = name;
   this.entity = entity;
   this.config = ConfigComponent("weapons", this.name);
   this.minDistance = this.config:Get("minDistance")  ||  0;
   this.maxDistance = this.config:Get("maxDistance");
   this.midDistance = this.config:Get("midDistance")  ||  ((this.maxDistance - this.minDistance) / 2);
   this.attackTimeout = this.config:Get("attackTimeout");
   this.attackPrepareTimeout = this.config:Get("attackPrepareTimeout");
   this.currentComboState = 1;
   this.remote = this.config:Get("remote")  ||  0;
   
   var actionTypes = {}
   var combo = this.config:Get("combo");
   var timing = this.config:Get("combotimeout");
   var effects = this.config:Get("comboeffect")  ||  {}
   
   for(k, v in pairs(combo) ){
     actionTypes[#actionTypes + 1] = {v, timing[k], effects[k]}
   }
   this.actionTypes = actionTypes;
   
   // Load damage parameters
   self:LoadDamage();
 }})
 public void LoadDamage(){
   this.criticalChance = this.config:Get("criticalChance");
   this.damage = {}
   var damageString = this.config:Get("damage");
   for(dType, dValue in string.gmatch(damageString, "(%w+)%:(%d+)%,?") ){
     this.damage[dType] = dValue;
   }
 }
 public void GetCriticalMultiplier(){
   return math.chance(this.criticalChance)  &&  2  ||  1;
 }
 public void GetDamage(){
   var damage = {}
   var critMul = self:GetCriticalMultiplier();
   for(k, v in pairs(this.damage) ){
     damage[k] = v * critMul;
   }
   var effect = this.actionTypes[this.currentComboState][3];
   return {damage=damage, effects={critical=(critMul > 1  &&  true  ||  false), punch=effect}}
 }
 public void CanAttack(target){
   var effectiveDistance = this.entity:GetEffectiveDistance(target);
   // return effectiveDistance > this.minDistance  &&  effectiveDistance < this.maxDistance
   return effectiveDistance < this.maxDistance  &&  (this.minDistance <= 0  ||  effectiveDistance > this.minDistance  ||  this.remote);
 }
 public void GetActionTypes(target){
   return this.actionTypes;
 }
 public void GetAttackDistance(){
   return this.midDistance //(this.maxDistance - this.minDistance) / 2
 }
 public void GetAttackTimeout(){
   return this.attackTimeout  ||  0;
 }
 public void GetAttackPrepareTimeout(){
   return this.attackPrepareTimeout  ||  0;
 }
 public void OnActivate(){
   var transforms = this.config:Get("transform");
   if(transforms != null  ){ 
     for(k, v in pairs(transforms) ){
       Transform.SetRendererState(this.entity.transform, v, true);
     }
   }
 }
 public void OnDeactivate(){
   var transforms = this.config:Get("transform");
   if(transforms != null  ){ 
     for(k, v in pairs(transforms) ){
       Transform.SetRendererState(this.entity.transform, v, false);
     }
   }
 }
 public void Attack(target){
   if(this.remote > 0  ){ 
     // throw bullet
     var bullet = BulletEntity("bullet", this.entity, target.GetPosition  &&  target:GetPosition()  ||  target, self);
     GameController:AddEntity(bullet);
     //print("Throw bullet!")
     // create bullet weapon
   }else{
     this.entity.damageProcessor:SendDamage(target, self:GetDamage())  ;
   }
 }
 }}