using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class actioninteract :action{
 var STATE_MOVING = 0;
 var STATE_REQUEST_INTERACT = 1;
 var STATE_INTERACT = 2;
 public void init (self, entity, target);
       Action.init(self, 1, false, "interact");
       this.entity = entity;
       this.target = target;
       this.state = STATE_MOVING;
 }})
 public void OnMoveComplete(){
   this.state = STATE_REQUEST_INTERACT;
   this.entity.combat:Attack(this.target, this.entity.weaponContainer.interactWeapon);
 }
 public void OnStartRunning(){
   this.state = STATE_MOVING;
   this.entity.mover:SetGoal(this.target, null, this.entity.weaponContainer:GetAttackDistance(this.entity.weaponContainer.interactWeapon));
 }
 public void OnStopRunning(){
   if(this.state == STATE_MOVING  ){ 
     this.entity.mover:Stop();
   }
 }
 public void Update(){
   if(this.state == STATE_MOVING  &&  not this.target.interactable  ){ 
     return true ;
   }
   if(this.state == STATE_MOVING  &&  not this.entity.mover:IsHaveGoal()  ){ 
     self:OnMoveComplete();
   }
   
   return this.state == STATE_INTERACT  &&  not this.entity.mecanim:CheckStateName("Action");
 }
 public void OnEvent(data){
   if(data == "punch"  ){             
     this.state = STATE_INTERACT;
     if(this.target.interactable  &&  this.entity.weaponContainer:CanAttack(this.target)then;
       this.target:Message("OnInteract", this.entity);
     }    
   }
 }
 public void GetPriority(){
   return this.state == STATE_MOVING  &&  1  ||  3;
 }
 public void IsContinuous(){
   return this.state == STATE_MOVING;
 }
 }}