using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class IronSpellJump :action{
 var SpellAction = Class({__includes=Action, init = public void (self, name){
       Action.init(self, 3, false, name);
       this.inState = false;
       
       this.radius = GameController.database:Get("spells", name .. "/punch_radius");
       this.effect = GameController.database:Get("spells", name .. "/punch_effect");
       this.damage = {}
       var damageString = GameController.database:Get("spells", name .. "/punch_damage");
       for(dType, dValue in string.gmatch(damageString, "(%w+)%:(%d+)%,?") ){
         this.damage[dType] = dValue;
       }            
 }})
 public void SpellAction:OnStartRunning()  {
   // call spell animation
   this.entity.mover:Stop();
   var pos = Input.RaycastTerrain(this.position);
   this.entity.mecanim:ForceSetState("SpellDownkick");
   var dv = pos - this.entity:GetPosition();
   //this.speed = 1.26 / dv:Length()
   this.speed = dv:Length() / 1;
   this.direction = dv:Normalize();
   this.moving = true;
 }
 public void SpellAction:OnStopRunning(){
   // stop spell animation?
   this.entity.mover:Stop();
 }
 public void SpellAction:ProcessMoving(){
   RigidbodyUtils.Move(this.entity.rigidbody, this.direction * this.speed);
 }
 public void SpellAction:Punch(){
   var entities = RaycastUtils.GetEntitiesInRadius(this.entity:GetPosition(), this.radius);
   for(k, v in pairs(entities) ){
     if(v != this.entity  ){ 
       var effects = {}
       effects[this.effect] = true;
       this.entity.damageProcessor:SendDamage(v, {damage=this.damage, effects=effects})
     }
   }  
 }
 public void SpellAction:FixedUpdate(){
   if(this.moving  ){ 
     self:ProcessMoving();
   }
 }
 public void SpellAction:Update(){
   if(not this.inState  ){ 
     this.inState = this.entity.mecanim:CheckStateName("SpellDownkick");
   }
   return this.inState  &&  not this.entity.mecanim:CheckStateName("SpellDownkick");
 }
 public void SpellAction:OnEvent(data){
   // Cast spell by animation here
   if(data == "punch"  ){ 
     this.moving = false;
     this.entity.mover:Stop();
     self:Punch();
   }
 }
 public void SpellAction:IsContinuous(){
   return false;
 }
 public void SpellAction:BeginDraw(){
 }
 public void SpellAction:StopDraw(){
 }
 public void SpellAction.CanApply(){
   return true;
 }
 return SpellAction}}