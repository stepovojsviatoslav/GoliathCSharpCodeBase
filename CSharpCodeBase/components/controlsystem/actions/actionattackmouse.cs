using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class actionattackmouse :action{
 var STATE_MOVING = 0;
 var STATE_REQUEST_ATTACK = 1;
 var STATE_ATTACK = 2;
 var ActionFastAttack = Class({__includes=Action, init = public void (self, entity, target){
       Action.init(self, 2, false, "fastattack");
       this.entity = entity;
       this.target = target;
       this.state = STATE_REQUEST_ATTACK;
 }})
 public void ActionFastAttack:OnPushed(){
   this.priority = 3;
 }
 public void ActionFastAttack:OnStartRunning(){
   print("Start attacking!");
   //this.priority = 3
   this.state = STATE_REQUEST_ATTACK;
   this.entity.combat:AttackVector(this.target);
   this.entity.mover:SetSpeed(this.entity.config:Get("moverSpeed") / 4);
 }
 public void ActionFastAttack:OnStopRunning(){
   this.entity.mover:ResetSpeed();
   Action.OnStopRunning(self);
   //this.entity.mecanim:ForceSetState("Empty", 1)
 }
 public void ActionFastAttack:Update(){
   self:ProcessMoving();
   return this.state == STATE_ATTACK ;
 }
 public void ActionFastAttack:ProcessMoving(){
   if(GameController.inputService:LeftStickYIsPressed()  ||   GameController.inputService:LeftStickXIsPressed()  ){ 
     this.currentInput = GameController.inputService:LeftStickValues();
     this.currentInput:RotateAroundY(GameController.camera.angle) ;
     this.entity.mover:SetInput(this.currentInput);
   }
 }
 public void ActionFastAttack:OnEvent(data){
   if(data == "punch"  ){   
     this.state = STATE_ATTACK;
     // Get damage objects
     var miss = true;
     if(this.entity.weaponContainer.currentWeapon.remote > 0  ){ 
       // shoot
       var sourceForward = Transform.GetForwardVector(this.entity.transform);
       this.entity.weaponContainer:Attack(this.entity:GetPosition() + this.target);
     }else{
       var nearEntities = RaycastUtils.GetEntitiesInRadius(this.entity:GetPosition(), 5);
       var sourceForward = Transform.GetForwardVector(this.entity.transform);
       for(k, v in pairs(nearEntities) ){
         if(v != this.entity  &&  v.interactable  ){ 
           if(HitTester.CheckHitEntity(this.entity, sourceForward, v, 45, 1.5)  ){ 
             this.entity.damageProcessor:SendDamage(v, this.entity.weaponContainer.currentWeapon:GetDamage());
             if(GameController.inputService:IsGamepad()  &&  this.entity.gamepadRightStickController:GetTarget() == null  &&  v.gameObject.tag != "Player"  &&  v.gameObject.tag == "Enemy"  ){ 
               this.entity.gamepadRightStickController:SetTarget(v);
             }
             miss = false;
           }
         }
       }
     }
     if(miss  ){ 
       this.entity.combat:ResetCombo();
     }
   }
 }
 public void ActionFastAttack:IsContinuous(){
   return false;
 }
 return ActionFastAttack}}