using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class mover :component{
 public void init (self, entity);
       Component.init(self, entity);
       this.entity.mover = self;
       this.input = Vector3();
       this.speed = this.entity.config:Get("moverSpeed");
       this.target = null;
       this._isStop = false;
       this._epsilon = this.entity.config:Get("moverEpsilon");
       this.moveType = 0;
       this.newDirection = Vector3(0,0,0);
       this._useLoaDefault = this.entity.config:Get("moverLoa");
       this._useLoa = this._useLoaDefault;
       this.raycastSphereRadius = this.entity.config:Get("raycastSphereRadius");
       this.raycastForwardDistance = this.entity.config:Get("raycastForwardDistance");
       this.raycastSphericalDistance = this.entity.config:Get("raycastSphericalDistance");
       this.offsetAngle = this.entity.config:Get("offsetAngle");
       this.speedCurve = null;
       this.autoLook = true;
       
       this.count = 0;
       this.countmax = 3;
 }})
 public void ResetSpeed(){
   this.speed = this.entity.config:Get("moverSpeed");
 }
 public void OnChangeVisibility(state){
   self:Stop();
 }
 public void SetCurve(curve){
   this.speedCurve = curve;
 }
 public void SetInput(vec3, moveType, ignoreLoa){
   if(ignoreLoa  ){ 
     this._useLoa = false;
   }else{
     this._useLoa = this._useLoaDefault;
   }
   this.target = null;
   this.input.x = vec3.x  ||  0;
   this.input.z = vec3.z  ||  0;
    if(this.input:Length() > 1  ){ 
     this.input:Normalize();
   }
   self:CheckStop();
   this.moveType = moveType  ||  0;
 }
 public void Stop(){
   self:SetInput(Vector3())  ;
   RigidbodyUtils.Move(this.entity.rigidbody, this.input * this.speed * GameController.gameTime);
 }
 public void CheckStop(){
   var inputLength = this.input:Length() * this.speed;
   if(not this._isStop  &&  inputLength == 0  ){ 
     this.newDirection = Vector3();
     this._isStop = true;
     RigidbodyUtils.Move(this.entity.rigidbody, Vector3());
     //RigidbodyUtils.ResetVelocity(this.entity.rigidbody)
   }else{if this._isStop  &&  inputLength != 0  ){ 
     this._isStop = false;
   }  
 }
 public void SetSpeed(speed){
   this.speed = speed;
 }
 public void SetInputToVec(vec3, stayTarget, moveType, ignoreLoa){
   if(ignoreLoa  ){ 
     this._useLoa = false;
   }else{
     this._useLoa = this._useLoaDefault;
   }
   if(not stayTarget  ){  this.target = null }
   var dv = vec3 - this.entity:GetPosition();
   this.input.x = dv.x;
   this.input.z = dv.z;
    if(this.input:Length() > 1  ){ 
     this.input:Normalize();
   }
   self:CheckStop();
   this.moveType = moveType  ||  0;
 }
 public void SetInputFromVec(vec3, stayTarget, moveType, ignoreLoa){
   if(ignoreLoa  ){ 
     this._useLoa = false;
   }else{
     this._useLoa = this._useLoaDefault;
   }
   if(not stayTarget  ){  this.target = null }
   var dv = vec3 - this.entity:GetPosition();
   this.input.x = -dv.x;
   this.input.z = -dv.z;
   if(this.input:Length() > 1  ){ 
     this.input:Normalize();
   }
   self:CheckStop();
   this.moveType = moveType  ||  0;
 }
 public void LookAt(vec3){
   RigidbodyUtils.LookAt(this.entity.rigidbody, vec3);
 }
 public void Update(){
   if(this.target != null  ){ 
     // check if(we reach target
     var pos = this.target.GetPosition  &&  this.target:GetPosition()  ||  this.target;
     //print(this.entity:GetEffectiveDistance(this.entity, this.target))
     if(this.target.GetPosition  &&  (this.entity:GetEffectiveDistance(this.target) < this.targetDistance  ||  not this.target.interactable)then;
       this.target = null;
       self:Stop();
     }else{if not this.target.GetPosition  &&  self:IsReachDestination(pos)  ){ 
       this.target = null;
       self:Stop();
     }else{
       self:SetInputToVec(pos, true);
     }
   }
   var moveSpeed = (this.input * this.speed):Length();
   if(this._latestMoveSpeed != moveSpeed  ){ 
     this.entity.mecanim:SetFloat("move_speed", moveSpeed);
     this._latestMoveSpeed = moveSpeed;
   }
   if(this._latestMoveType != this.moveType  ){ 
     this.entity.mecanim:SetFloat("move_type", this.moveType);
     this._latestMoveType = this.moveType;
   }
 }
 public void FixedUpdate(){
   if(not this._isStop  ){ 
     if(this._useLoa == true  ){ 
       if(this.count >= this.countmax  ){ 
         this.count = 0;
         var result = RigidbodyUtils.AvoidMove(this.entity.gameObject, ;
                                                 this.input, ;
                                                 this.newDirection, ;
                                                 this.raycastSphereRadius , ;
                                                 this.raycastForwardDistance, ;
                                                 this.raycastSphericalDistance ,;
                                                 this.offsetAngle);
       this.input = Vector3(result.x1, 0, result.z1);
       this.newDirection = Vector3(result.x2, 0, result.z2);
       }
     }
     if(this.speedCurve != null  ){ 
       this.input = this.input * this.entity.mecanim:GetFloat("speed_curve");
     }
     if(this.newDirection  &&  this.newDirection.x != 0  &&  this.newDirection.y !=0  &&  this.newDirection.z !=0  ){ 
       this.input = this.newDirection;
     }
   }
   if(this.input:Length() > 0  ){ 
     if(this.autoLook  ){ 
         RigidbodyUtils.Move(this.entity.rigidbody, this.input * this.speed, this._useLoa  &&  5  ||  10);
     }else{
       if(GameController.inputService:IsGamepad() == false  ){ 
         RigidbodyUtils.MoveLookAtMouse(this.entity.rigidbody, this.input * this.speed, this.entity.height / 3);
       }else{
         if(this.entity.gamepadRightStickController  &&   this.entity.gamepadRightStickController:GetTarget()  ){ 
           RigidbodyUtils.MoveNotRotate(this.entity.rigidbody, this.input * this.speed);
           self:LookAt((this.entity.gamepadRightStickController:GetTarget():GetPosition()));
         }else{
           RigidbodyUtils.Move(this.entity.rigidbody, this.input * this.speed);
         }
       }
     }
   }
   this.count = this.count + 1;
 }
 public void SetGoal(target, moveType, distance){
   this.target = target;
   this.targetDistance = distance  ||  this._epsilon;
   this.moveType = moveType  ||  0;
 }
 public void IsHaveGoal(){
   return this.target != null;
 }
 public void IsReachDestination(vec3){
   var localPos = this.entity:GetPosition();
   var v1 = Vector2(localPos.x, localPos.z);
   var v2 = Vector2(vec3.x, vec3.z);
   return v1:Dist(v2) < this._epsilon;
 }
 }}