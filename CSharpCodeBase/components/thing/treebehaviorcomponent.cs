using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class treebehaviorcomponent :component{
 public void init(self, entity);
       Component.init(self, entity);
       this.entity.treeBehavior = self;
       this.animator = this.entity.mecanim;
       this.deathTimeout = this.entity.config:Get("deathTimeout");
       this.fallenTreeTransformName = this.entity.config:Get("rigidbodyTransform");
       this.stumpTransformName = this.entity.config:Get("stumpTransform");
       
       this.stumpTransform = this.entity.gameObject.transform:Find(this.stumpTransformName);
       this.fallenTreeTransform = this.entity.gameObject.transform:Find(this.fallenTreeTransformName).gameObject.transform;
 }})
 public void OnDeath(damageData){
   if(this.entity.stateManager.stateName[this.entity.stateManager.currentState] == "fallenTree"   ){ 
     self:SaveTransform();
     Transform.SetColliderState(this.stumpTransform, true);
     RigidbodyUtils.MoveDown(this.fallenTreeTransform.gameObject, damageData.source.gameObject);
   }
 }
 public void SaveTransform(){
   this.localPosition = this.fallenTreeTransform.localPosition;
   this.localRotation = this.fallenTreeTransform.localRotation;
 }
 public void RestoreTransform(){
   if(this.localPosition  &&  this.localRotation  ){ 
     this.fallenTreeTransform.localPosition = this.localPosition;
     this.fallenTreeTransform.localRotation = this.localRotation;
   }
 }
 public void AfterFall(result){
   this.entity.stateManager:SetStateByNumber(#this.entity.stateManager.stateName);
   self:RestoreTransform();
   this.entity:Destroy();
   result:Complete();
 }
 public void OnFragileForce(){
     Transform.SetColliderState(this.stumpTransform, false);
 }
 }}