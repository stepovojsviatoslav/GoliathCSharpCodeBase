using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class grenademodevisualizer :component{
 var alpha = 45;
 var dt = 0;
 var g = 9.81;
 var speed = 15;
 var vectorForward = Vector3(0, 0, 1);
 var vectorRight = Vector3(1, 0, 0);
 var GrenadeModeVisualizerComponent = Class({__includes=Component, init=function(self, entity)
     Component.init(self, entity);
     this.entity.grenadeModeVisualizer = self;
     this.pointList = {}
     this.targetPosition = null;
      
     this.lineManager = GameController.pooler:Fetch("line");
     this.lineRendererComponent = this.lineManager:GetComponent("LineRenderer");
     this.expRadius = GameController.pooler:Fetch("explosionRadius");
     this.flightRadius = GameController.pooler:Fetch("distance");
     this.lineRendererComponent:SetColors(luanet.UnityEngine.Color.green, luanet.UnityEngine.Color.red);
     this.lineRendererComponent:SetWidth(0.07, 0.07);
       
     this.oldDistance = 0;
     dt = 0.15;
       
     if(not GameController.inputService:IsGamepad()  ){ 
       this.targetPosition = Input.RaycastMouseOnTerrain() - this.entity:GetPosition();
     }else{
       var position = this.entity:GetPosition() + this.entity:GetForwardVector()*5 + Vector3(0,0.05,0);
       Transform.SetPosition(this.expRadius.transform, position);
       this.targetPosition =  Transform.GetPosition(this.expRadius.transform) - this.entity:GetPosition();
     }
     
     self:Disable();
 }})
 public void GrenadeModeVisualizerComponent:CalculateTrajectory(){
   this.pointList = {}
   var speedY = this.speed.y;
   var pos = Vector3(this.entity.gameObject.transform.position.x, this.entity.gameObject.transform.position.y + this.entity.height / 2, this.entity.gameObject.transform.position.z);
   table.insert(this.pointList, pos);
   while((pos.y - 0.1 > this.entity.gameObject.transform.position.y) ){
     speedY = speedY - (g * dt);
     var position = Vector3(pos.x + this.speed.x *dt, ;
                     pos.y + speedY * dt, ;
                     pos.z + this.speed.z * dt);
     pos = position;
     table.insert(this.pointList, position);
   } 
   this.lineRendererComponent:SetVertexCount(#this.pointList);
 }
 public void GrenadeModeVisualizerComponent:FixedUpdate(){
   if(not this.active  ){ 
     return;
   }
   
   if(GameController.inputService:IsGamepad() == false  ){ 
     this.targetPosition = Input.RaycastMouseOnTerrain()- this.entity:GetPosition()  ;
   }else{
     Transform.SetPosition(this.expRadius.transform, this.targetPosition + this.entity:GetPosition());
   }
   
   var vec1 = Vector3(0,0,0);
   var vec2 = this.targetPosition;
   vec2.y = 0;
   this.distance = Vector3.Distance(vec1, vec2);
   this.direction = (vec2 - vec1);
   this.direction = this.direction:Normalize();
  
   if(GameController.inputService:IsGamepad()  ){ 
     self:GamepadExplotionRadiusMoving();
   }else{
     if(this.distance > this.radius  ){ 
       this.distance = this.radius;
     }else{
       this.oldDistance = this.distance;
     }
   }
   
   this.speedScalar = math.sqrt(this.distance*g/ (math.sin(2*alpha)));
   this.speed = Vector3(this.speedScalar* math.sin(alpha)*this.direction.x, this.speedScalar* math.cos(alpha), this.speedScalar* math.sin(alpha)*this.direction.z);
   this.time = 2* this.speedScalar* math.cos(alpha)/g;
   
   self:CalculateTrajectory();
   self:DrawLine();
   
   if(GameController.inputService:IsGamepad() == false  ){ 
     var vec = Vector3(this.pointList[#this.pointList].x, this.entity:GetPosition().y + 0.05, this.pointList[#this.pointList].z);
     Transform.SetPosition(this.expRadius.transform, vec);
   } 
   Transform.SetPosition(this.flightRadius.transform, Vector3(0,0.05,0) + this.entity:GetPosition());
 }
 public void GrenadeModeVisualizerComponent:DrawLine(){
   for(i=1, #this.pointList, 1 ){
     RigidbodyUtils.SetPointPositionToLineRenderer(this.lineRendererComponent, this.pointList[i].x,this.pointList[i].y,this.pointList[i].z, i-1);
   }
 }
 public void GrenadeModeVisualizerComponent:GamepadExplotionRadiusMoving(){
   var dir = GameController.inputService:GetLookValue();
   if(GameController.inputService:GetLookValue().x !=0  ||  GameController.inputService:GetLookValue().z !=0  ){ 
     self:MoveTransform(dir.x*speed, dir.z *speed);
     this.targetPosition =  Transform.GetPosition(this.expRadius.transform) - this.entity:GetPosition();
   }
 }
 public void GrenadeModeVisualizerComponent:MoveTransform(speedX, speedZ){
   var position = Transform.GetPosition(this.expRadius.transform) + vectorForward*speedZ*GameController.deltaTime + vectorRight*speedX*GameController.deltaTime;
   var currentDistance = Vector3.Distance(position, this.entity:GetPosition());
   if(currentDistance < this.radius   ){ 
       Transform.SetPosition(this.expRadius.transform, position);
   }
 }
 public void GrenadeModeVisualizerComponent:Enable(name){
   this.name = name;
   this.config = ConfigComponent("gadget", name);
   this.active = true;
   this.lineManager:SetActive(true);
   this.flightRadius:SetActive(true);
   this.expRadius:SetActive(true);
   this.radius = this.config:Get("radius");
   this.radiusExp = this.config:Get("radius_exp");
   Transform.SetScale(this.flightRadius.transform, Vector3(this.radius*2, 0.1, this.radius*2));
   Transform.SetScale(this.expRadius.transform, Vector3(this.radiusExp*2, 0.1, this.radiusExp*2));
 }
 public void GrenadeModeVisualizerComponent:Disable(){
   this.active = false;
   this.lineManager:SetActive(false);
   this.flightRadius:SetActive(false);
   this.expRadius:SetActive(false);
 }
 public void GrenadeModeVisualizerComponent:GetSpeed(){
   self:Disable();
   return this.speed;
 }
 Component}}