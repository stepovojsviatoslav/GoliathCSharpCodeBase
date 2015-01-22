using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class actiondashpunch :action{
 public void init (self, entity, target);
       Action.init(self, 4, false, "dashpunch");
       this.entity = entity;
       this.target = target;
 }})
 public void OnStartRunning(){
   this.entity.mover:Stop();
   this.entity.combat:Attack(this.target);
   this.isAnimationStarted = false;
   this.jumping = false;
   this.canpunch = false;
   this.dv = this.target:GetPosition() - this.entity:GetPosition();
   this.dv:Normalize();
 }
 public void Update(){
   if(not this.isAnimationStarted  ){ 
     this.isAnimationStarted = this.entity.mecanim:CheckStateName("Action");
   }
   return this.isAnimationStarted  &&  not this.entity.mecanim:CheckStateName("Action");
 }
 public void FixedUpdate(){
   if(not this.jumping  ){ 
     this.entity.mover:LookAt(this.target:GetPosition());
   }
   var speedCurve = this.entity.mecanim:GetFloat("speed_curve");
   //local dv = this.target:GetPosition() - this.entity:GetPosition()
   //dv:Normalize()
   if(this.jumping  ){ 
     RigidbodyUtils.MoveNotRotate(this.entity.rigidbody, this.dv * speedCurve * 10);
   }
   if(this.canpunch  ){ 
     if(this.target.interactable  &&  this.entity.weaponContainer:CanAttack(this.target)  ){ 
       this.entity.weaponContainer:Attack(this.target);
       //this.entity.damageProcessor:SendDamage(this.target, this.entity.weaponContainer.currentWeapon:GetDamage())
       this.canpunch = false;
     }
   }
 }
 public void OnEvent(data){
   if(data == "punch"  ){         
     //if this.target.interactable  &&  this.entity.weaponContainer:CanAttack(this.target)  ){ 
     //  this.entity.damageProcessor:SendDamage(this.target, this.entity.weaponContainer.currentWeapon:GetDamage())
     //end
   }else{if data == "jumping"  ){ 
     this.jumping = true;
     this.canpunch = true;
     this.dv = this.target:GetPosition() - this.entity:GetPosition();
     this.dv:Normalize()    ;
   }else{if data == "stoppunch"  ){ 
     this.canpunch = false;
   }
   
 }
 public void OnStopRunning(){
   this.entity.mover:Stop();
 }
 }}