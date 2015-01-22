using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class droppable :unityexistsentity{
 var STATE_MOVING = 0;
 var STATE_STAYING = 1;
 public void init (self, gameObject);
       UnityExistsEntity.init(self, gameObject);
       this.config = ConfigComponent("items", this.name);
       self:AddComponent(PossibleActionsComponent);
       this.item = null;
       this.count = 0;
       this.position = Vector3();
       this.rigidBody = this.gameObject:GetComponent("Rigidbody");
       this.state = STATE_MOVING;
       this.stateTimeout = 1;
 }})
 public void SetupDrop(item, count, sourceEntity, forceVec){
   // calculate position from prefab
   if(forceVec != null  ){ 
     forceVec:Randomize(0.4, 0, 0.4);
   }
   var forceVector = forceVec  ||  Vector3.GetRandom(5, 0, 5);
   forceVector:Normalize();
   var position = sourceEntity:GetPosition() + forceVector * sourceEntity.radius + Vector3(0, 1, 0) * (sourceEntity.height / 2.0);
   self:SetPosition(position);
   
   this.item = item;
   this.count = count;
   if(sourceEntity != null  ){ 
     RigidbodyUtils.IgnoreCollision(sourceEntity.transform, this.transform);
   }
   RigidbodyUtils.ApplyImpulse(this.rigidBody, forceVector * 5);
 }
 public void Message(funcname, data){
   if(funcname == "OnInteract"  ){ 
     self:OnInteract(data);
   }
   UnityExistsEntity.Message(self, funcname, data);
 }
 public void OnInteract(sourceEntity){
   var restItem = GameController.inventory:AddItem(this.item, this.count);
   if(restItem.count > 0  ){ 
     GameController.dropManager:DropItem(restItem.name, restItem.count, self:GetPosition());
   }
   self:Destroy();
 }
 public void Update(){
   UnityExistsEntity.Update(self);
   if(this.state == STATE_MOVING  ){ 
     this.stateTimeout = this.stateTimeout - GameController.deltaTime;
     if(this.stateTimeout < 0  ){ 
       this.state = STATE_STAYING;
       this.rigidBody.isKinematic=true;
       this.gameObject.collider.isTrigger = true;
     }
   }
 }
 public void Save(storage){
   storage:SetBool("isLua", true);
   storage:SetTransform(this.transform);
 }
 public void Load(storage){
   storage:LoadTransform(this.transform);
   this.spawnPosition = self:GetPosition()  ;
 }
 public void Droppable.Create(storage, x, y, z, _type){
   storage:SetBool("isLua", true);
   storage:SetPosition(x, y, z);
 }
 }}