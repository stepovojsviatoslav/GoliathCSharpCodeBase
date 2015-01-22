using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class actionmove :action{
 public void init (self, entity, target);
       Action.init(self, 1, true, "move");
       this.isKeyboardControl = true;
       this.currentInput = Vector3();
       this.entity = entity;
       this.target = target;
 }})
 public void SwitchToKeyboard(){
   this.isKeyboardControl = true;
 }
 public void SwitchToMouse(){
   this.isKeyboardControl = false;
 }
 public void OnStartRunning(){
   if(this.target != null  ){ 
     self:SwitchToMouse();
     this.entity.mover:SetGoal(this.target);
   }
   this.entity.mover:ResetSpeed();
 }
 public void Update(){
   var actionResult = true;
   if(GameController.inputService:LeftStickYIsPressed()  ||   GameController.inputService:LeftStickXIsPressed()  ){ 
     this.currentInput = GameController.inputService:LeftStickValues();
     this.currentInput:RotateAroundY(GameController.camera.angle) ;
     this.entity.mover:SetInput(this.currentInput);
     actionResult = false;
   }
   if(this.isKeyboardControl  ){ 
     if(Input.GetMouseButtonDown(1)  ){ 
       this.entity.mover:SetGoal(Input.RaycastMouseOnTerrain());
       self:SwitchToMouse();
       actionResult = false;
     }
     
     if(GameController.inputService:GetMouseButton(1)  &&  this.currentInput:Length() == 0  ){ 
       self:SwitchToMouse();
       actionResult = false;
     }
   }
   
   if(not this.isKeyboardControl  ){ 
     if(GameController.inputService:GetMouseButton(1)  ){         
       this.entity.mover:SetGoal(Input.RaycastTargetEntity()  ||  Input.RaycastMouseOnTerrain());
       actionResult = false;
     }    
     
     if(not this.entity.mover:IsHaveGoal()  ||  GameController.inputService:LeftStickYIsPressed()  ||  GameController.inputService:LeftStickXIsPressed()  ){ 
       self:SwitchToKeyboard();
     }
     
     if(this.entity.mover:IsHaveGoal()  ){ 
       actionResult = false;
     }
   }
   return actionResult;
 }
 public void OnStopRunning(){
   this.entity.mover:Stop();
 }
 }}