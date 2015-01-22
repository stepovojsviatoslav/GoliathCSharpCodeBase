using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class actionpush :action{
 public void init (self, entity, targetPosition);
       Action.init(self, 4, false, "push");
       this.entity = entity;
       this.targetPosition = targetPosition;
 }})
 public void OnStartRunning(){
   //Action.OnStart(self)
   this.entity.mover:Stop();
   this.entity.mecanim:SetFloat("action_type", 1);
   this.entity.mecanim:ResetTrigger("action");
   this.entity.mecanim:ForceSetState("Hit");
   this.dv = this.entity:GetPosition() - this.targetPosition;
   this.dv.y = 0;
   this.dv:Normalize();
   //print(this.targetPosition.x .. "," .. this.targetPosition.z)
   this.isHitStarted = false;
 }
 public void Update(){
   if(not this.isHitStarted  ){ 
     this.isHitStarted = this.entity.mecanim:CheckStateName("Hit");
   }
   
   return this.isHitStarted  &&  not this.entity.mecanim:CheckStateName("Hit");
 }
 public void FixedUpdate(){
   this.entity.mover:LookAt(this.targetPosition);
   var speedCurve = this.entity.mecanim:GetFloat("speed_curve");
   //this.entity.mover:SetInput(this.dv * speedCurve)
   RigidbodyUtils.MoveNotRotate(this.entity.rigidbody, this.dv * speedCurve * 10);
 }
 public void OnStopRunning(){
   this.entity.mover:Stop();
 }
 }}