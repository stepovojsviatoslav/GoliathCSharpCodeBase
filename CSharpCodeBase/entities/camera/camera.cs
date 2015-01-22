using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class camera :unityexistsentity{
 var CameraEntity = Class({__includes=UnityExistsEntity, init = public void (self, gameObject){
       Transform.AddComponent(gameObject, "LuaMapper");
       UnityExistsEntity.init(self, gameObject);
       
       this.targetEntity = null;
       
       this.minHeight = 6;
       this.minDistance = 12;
       this.maxHeight = 30;
       this.maxDistance = 40;
       this.targetHeight = 1;
       
       this.currentZoom = 0.5;
       this._zoom = this.currentZoom;
       this.zoomSoftLimit = 0.05;
       this.lerpZoomSpeed = 5;
       this.lerpLimitSpeed = 20;
       
       this.angle = 0;
       this.currentAngle = 0;
       
       this.scrollSpeed = 3;
       
       this.luaMapper:SetAlwaysVisible(true);
 }})
 public void CameraEntity:LoadForEntity(target){
   this.targetHeight = target.height / 2;
   this.minHeight = target.config:Get("cameraMinHeight")  ||  6;
   this.minDistance = target.config:Get("cameraMinDistance")  ||  12;
   this.maxHeight = target.config:Get("cameraMaxHeight")  ||  30;
   this.maxDistance = target.config:Get("cameraMaxDistance")  ||  40 ;
 }
 public void CameraEntity:SetTargetEntity(entity){
   this.targetEntity = entity;
 }
 public void CameraEntity:Update(){
   var isGamepad = GameController.inputService:IsGamepad();
   var lookValue = GameController.inputService:GetLookValue();
   var scroll = 0;
   if(isGamepad  &&  GameController.inputService:RightBumperIsPressed()  ){ 
     if(math.abs(lookValue.z) > 0.5  ){ 
       scroll = lookValue.z;
     }
   }else{
     scroll = Input.GetMouseScrollValue();
   }
   if(scroll != 0  ){ 
     var sign = scroll > 0  &&  1  ||  -1;
     this._zoom = this._zoom - sign * this.scrollSpeed * GameController.deltaTime;
     this._zoom = math.clamp(0 - this.zoomSoftLimit, this._zoom, 1 + this.zoomSoftLimit);
   }
   if(isGamepad  ){ 
     if(GameController.inputService:RightBumperIsPressed()  ){ 
       var val = lookValue.x;
       if(math.abs(val) > 0.5  ){ 
         if(val < 0  &&  this.isGamepadRotateBlocked == null  ){ 
           this.currentAngle = this.currentAngle - math.rad(90);
         }else{if this.isGamepadRotateBlocked == null  ){ 
           this.currentAngle = this.currentAngle + math.rad(90);
         }
         this.isGamepadRotateBlocked = true;
       }else{
         this.isGamepadRotateBlocked = null;
       }
     }
   }else{
     if(Input.GetKeyDown(KeyCode.Z)  ){ 
       this.currentAngle = this.currentAngle - math.rad(90);
     }else{if Input.GetKeyDown(KeyCode.X)  ){ 
       this.currentAngle = this.currentAngle + math.rad(90);
     }
   }
   
   this.angle = math.lerp(this.angle, this.currentAngle, GameController.deltaTime * this.lerpLimitSpeed);
 }
 public void CameraEntity:FixedUpdate(){
 }
 public void CameraEntity:LateUpdate(){
   if(this.targetEntity != null  ){   
     // Align zoom
     if(this._zoom < 0  ){  
       this._zoom = math.lerp(this._zoom, 0, GameController.deltaTime * this.lerpLimitSpeed);
     }else{if this._zoom > 1  ){ 
       this._zoom = math.lerp(this._zoom, 1, GameController.deltaTime * this.lerpLimitSpeed);
     }
     this.currentZoom = math.lerp(this.currentZoom, this._zoom, this.lerpZoomSpeed * GameController.deltaTime);
     
     // Calculate current parameters
     var heightValue = math.lerp(this.minHeight, this.maxHeight, this.currentZoom);
     var distanceValue = math.lerp(this.minDistance, this.maxDistance, this.currentZoom);
     if(this.heightValue == null  ){ 
       this.heightValue = heightValue;
       this.distanceValue = distanceValue;
     }
     //heightValue = math.lerp(this.heightValue, heightValue, GameController.deltaTime * 50)
     //distanceValue = math.lerp(this.distanceValue, distanceValue, GameController.deltaTime * 50)
     this.heightValue = heightValue;
     this.distanceValue = distanceValue;
     var offsetVector = Vector3(0, heightValue, -distanceValue);
     offsetVector:RotateAroundY(this.angle);
     
     // Setup position  &&  rotation
     var position = this.targetEntity:GetPosition() + offsetVector;
     self:SetPosition(position);
     
     // Setup rotation
     offsetVector.y = offsetVector.y - this.targetHeight;
     var rotation = RotationUtils.LookRotation(offsetVector * -1);
     self:SetRotation(rotation);
   }
 }
 Entity}}