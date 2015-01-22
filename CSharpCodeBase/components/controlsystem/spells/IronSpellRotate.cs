using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class IronSpellRotate :action{
 var STATE_MOVING = 0;
 var STATE_REQUEST_ATTACK = 1;
 var STATE_ATTACK = 2;
 var SpellAction = Class({__includes=Action, init = public void (self, name){
       Action.init(self, 3, false, name);
       this.inState = false;
       this.punch_timeout = GameController.database:Get("spells", name .. "/punch_timeout");
       this.radius = GameController.database:Get("spells", name .. "/punch_radius");
       this.timeout = this.punch_timeout;
       this.damage = {}
       var damageString = GameController.database:Get("spells", name .. "/punch_damage");
       for(dType, dValue in string.gmatch(damageString, "(%w+)%:(%d+)%,?") ){
         this.damage[dType] = dValue;
       }      
 }})
 public void SpellAction:OnStartRunning()  {
   // call spell animation
   this.entity.mecanim:ForceSetState("SpellRotate");
 }
 public void SpellAction:OnStopRunning(){
   // stop spell animation?
 }
 public void SpellAction:ProcessMoving(){
   if(GameController.inputService:LeftStickYIsPressed()  ||   GameController.inputService:LeftStickXIsPressed()  ){ 
     this.currentInput = GameController.inputService:LeftStickValues();
     this.currentInput:RotateAroundY(GameController.camera.angle) ;
     this.entity.mover:SetInput(this.currentInput);
   }
 }
 public void SpellAction:Punch(){
   var entities = RaycastUtils.GetEntitiesInRadius(this.entity:GetPosition(), this.radius);
   for(k, v in pairs(entities) ){
     if(v != this.entity  ){ 
       this.entity.damageProcessor:SendDamage(v, {damage=this.damage, effects={}})
     }
   }
 }
 public void SpellAction:Update(){
   self:ProcessMoving();
   this.timeout = this.timeout - GameController.deltaTime;
   if(this.timeout < 0  ){ 
     self:Punch();
     this.timeout = this.punch_timeout;
   }
   if(not this.inState  ){ 
     this.inState = this.entity.mecanim:CheckStateName("SpellRotate");
   }
   return this.inState  &&  not this.entity.mecanim:CheckStateName("SpellRotate");
 }
 public void SpellAction:OnEvent(data){
   // Cast spell by animation here
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