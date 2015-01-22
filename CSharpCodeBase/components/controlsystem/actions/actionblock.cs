using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class actionblock :action{
 public void init (self, entity, target);
       Action.init(self, 1, false, "block");
       this.entity = entity;
       this.inputService = GameController.inputService;
 }})
 public void OnPushed(){
   //this.priority = 1
 }
 public void OnStartRunning(){
   Action.OnStartRunning(self);
   this.entity.mecanim:SetBool("block", true);
   this.priority = 1;
   this.entity.damageReceiver:SetOverrideSideResist("front", 0);
   this.entity.damageReceiver:SetOverrideSideResist("side", 0);
 }
 public void OnStopRunning(){
   this.entity.mecanim:SetBool("block", false);
   this.entity.damageReceiver:DropOverrideSideResist("front");
   this.entity.damageReceiver:DropOverrideSideResist("side");
 }
 public void Update(){
   // Look at
   var input = Vector3();
   input = this.inputService:LeftStickValues() ;
   
   if(this.inputService:GetMouseButton(1)  ){ 
     input = Input.RaycastMouseOnTerrain();
   }else{if input:Length() > 0  ){ 
     input:Normalize();
     input:RotateAroundY(GameController.camera.angle) ;
     input = input + this.entity:GetPosition();
   }
   //if input:Length() > 0  ){ 
     //this.entity.mover:LookAt(input)
   //end
   
   return not this.inputService:RightTriggerIsPressed()//this.isBlock  &&  not this.entity.mecanim:CheckStateName("Block")
 }
 }}