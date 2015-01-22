using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class gamepadmover :component{
 var angle = 15;
 var GamePadRightStickController = Class({__includes=Component, init = public void (self, entity){
       Component.init(self, entity);
       this.entity.gamepadRightStickController = self;
       this.radius = 20;
       this.sortedEntityList = {}
       this.stickDirAngle = 0;
       this.target = null;
       this.border = GameController.pooler:Fetch("target");
       this.lineManager = GameController.pooler:Fetch("line");
       this.lineRendererComponent = this.lineManager:GetComponent("LineRenderer");
       this.lineRendererComponent:SetVertexCount(2);
       this.lineRendererComponent:SetColors(luanet.UnityEngine.Color.red, luanet.UnityEngine.Color.red);
       this.lineRendererComponent:SetWidth(0.05, 0.05);
       this.time = 0;
       this.currentCount = 0;
       this.active = true;
 }})
 public void GamePadRightStickController:Update()  {
   if(this.active  ){ 
     if(not this.entity.grenadeModeVisualizer.active  ){ 
         var rightBumper = GameController.inputService:RightBumperIsPressed();
         this.sortedEntityList = self:GetEntitiesInRadius();
         if(not rightBumper  &&  (math.abs(GameController.inputService:GetLookValue().x) > 0.4  ||  math.abs(GameController.inputService:GetLookValue().z) > 0.4)  ){ 
           if(this.deniedTarget == null  ){  this.deniedTarget = this.oldTarget }
           this.stickDirAngle = self:GetAngle(GameController.inputService:GetLookValue());
           this.avalibleTargets = self:GetTargetFromList(this.stickDirAngle, angle);
           if(#this.avalibleTargets == 0  ){ 
             var temp1 = self:GetTargetFromList(self:ConvertAngle(this.stickDirAngle - angle*2), angle);
             var temp2 = self:GetTargetFromList(self:ConvertAngle(this.stickDirAngle + angle*2), angle);
             for(key, value in pairs(temp1) ){
               this.avalibleTargets[key] = value;
             }
             for(key, value in pairs(temp2) ){
               this.avalibleTargets[key] = value;
             }
           }
           if(#this.avalibleTargets > 0  ){ 
             table.sort(this.avalibleTargets, function(a,b) return a[1]<b[1] })
             if(this.deniedTarget == this.avalibleTargets[1][2]  ){ 
               if(this.avalibleTargets[2]  ){ 
                 this.target = this.avalibleTargets[2][2];
               }
             }else{
               this.target = this.avalibleTargets[1][2];
             }
             this.border:SetActive(true);
             this.lineManager:SetActive(true);
           }
         }else{
           this.deniedTarget = null;
         }
       if(this.target  ){ 
         Transform.SetPosition(this.border.transform, Vector3(this.target:GetPosition().x, this.target:GetPosition().y + 0.1, this.target:GetPosition().z));
         RigidbodyUtils.SetPointPositionToLineRenderer(this.lineRendererComponent, this.target:GetPosition().x,this.target:GetPosition().y + 0.5,this.target:GetPosition().z, 0);
         RigidbodyUtils.SetPointPositionToLineRenderer(this.lineRendererComponent, this.entity:GetPosition().x,this.entity:GetPosition().y + 0.5,this.entity:GetPosition().z, 1);
       }
        
       if((this.target  &&  this.target.visible == false)  ||  ;
       GameController.inputService:RightStickButtonWasPressed()  ||  ;
         (this.target  &&  this.target.isDeath) ;
        ){ 
         this.target = null;
         this.border:SetActive(false);
         this.time = 0;
         this.lineManager:SetActive(false);
         this.deniedTarget = null;
       }
       
       if(this.target  &&  GameController.inputService:GetLookValue().x ==0  &&  GameController.inputService:GetLookValue().z ==0  ){ 
         if(this.time > 1  ){ 
           this.time = 0;
           this.lineManager:SetActive(false);
         }
         this.time = this.time + GameController.deltaTime;
       }
       this.oldTarget = this.target;
     }
   }
 }
 public void GamePadRightStickController:DropTarget(){
   this.target = null;
   this.border:SetActive(false);
   this.time = 0;
   this.lineManager:SetActive(false);
   this.deniedTarget = null  ;
 }
 public void GamePadRightStickController:GetTarget(){
   return this.target;
 }
 public void GamePadRightStickController:SetTarget(target){
   this.target = target;
   this.border:SetActive(true);
 }
 public void GamePadRightStickController:GetEntitiesInRadius(){
   var tempEntityList = RaycastUtils.GetEntitiesInRadius(this.entity:GetPosition(), this.radius);
   var entityList = {}
   for(k, v in pairs(tempEntityList) ){
     if(v.gameObject.tag != "Player"  &&  v.gameObject.tag == "Enemy"  ){ 
       var tbl = {self:GetAngle(v:GetPosition() - this.entity:GetPosition()), v}
       table.insert(entityList, tbl);
     }
   }
   table.sort(entityList, function(a,b) return a[1]<b[1] })
   return entityList;
 }
 public void GamePadRightStickController:GetAngle(vec){
   var mAngle = math.atan2(vec.z, vec.x)*180/math.pi;
   mAngle = (180 + mAngle)%360;
   return mAngle;
 }
 public void GamePadRightStickController:GetTargetFromList(stickDirAngle, angle){
   var bound1 = Vector2(stickDirAngle, stickDirAngle + angle);
   var bound2 = Vector2(stickDirAngle, stickDirAngle - angle);
   var bound3 = Vector2();
   var bound4 = Vector2();
   
   if(bound1.y > 360  ){ 
     bound1.x = 0;
     bound1.y = bound1.y - 360;
     bound3.x = 360 - bound1.y;
     bound3.y = 360;
   }
   
   if(bound2.y < 0  ){ 
     bound2.x = bound2.y + 360;
     bound2.y = 360;
     bound4.x = 0;
     bound4.y = stickDirAngle;
   }
   
   var bounds = {}
   table.insert(bounds, bound1);
   table.insert(bounds, bound2);
   if(bound3.y > 0  ){ 
     table.insert(bounds, bound3);
   }
   if(bound4.y > 0  ){ 
     table.insert(bounds, bound4);
   }
   
   var entities = {}
   for(k, v in pairs(bounds) ){
     var targets = self:GetEntitiesBetweenAngles(v);
       for(targetKey, targetValue in pairs(targets) ){
         if(targetValue.visible  ){ 
           var entity = {Vector3.Distance(targetValue:GetPosition(), this.entity:GetPosition()), targetValue}
           table.insert(entities, entity);
         }
       }
   }
   return entities;
 }
 public void GamePadRightStickController:GetEntitiesBetweenAngles(vec2){
   var entities = {}
   for(k, v in pairs(this.sortedEntityList) ){
     if(vec2.x > vec2.y  ){ 
       if(v[1] >= vec2.y  &&  v[1] <= vec2.x  ){ 
         table.insert(entities, v[2]);
       }
     }else{
       if(v[1] >= vec2.x  &&  v[1] <= vec2.y  ){ 
         table.insert(entities, v[2]);
       }
     }
   }
   return entities;
 }
 public void GamePadRightStickController:ConvertAngle(angle){
   var mAngle = angle;
   if(mAngle < 0  ){ 
     mAngle = 360 + mAngle;
   }else{if mAngle > 360  ){ 
     mAngle = mAngle - 360;
   }
   return mAngle;
 }
 public void GamePadRightStickController:Disable(){
   this.target = null;
   this.border:SetActive(false);
   this.time = 0;
   this.lineManager:SetActive(false);
   this.deniedTarget = null;
   this.active = false;
 }
 public void GamePadRightStickController:Enable(){
   this.active = true;
 }
 return GamePadRightStickController}}