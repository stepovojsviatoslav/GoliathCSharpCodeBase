using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class enemy :unityexistsentity{
 var EnemyEntity = Class({__includes=UnityExistsEntity, init = public void (self, gameObject){
       UnityExistsEntity.init(self, gameObject);
       this.config = ConfigComponent("enemies", this.name);
       this.storage = {}
       this.rigidbody = this.gameObject:GetComponent("Rigidbody");
       
       self:AddComponent(HealthComponent);
       self:AddComponent(StarvationComponent);
       self:AddComponent(MecanimComponent);
       self:AddComponent(MoverComponent);
       self:AddComponent(CharacterWeaponContainer);
       self:AddComponent(VisionComponent);
       self:AddComponent(AIWorldMapComponent);
       self:AddComponent(RelationshipComponent);
       self:AddComponent(ResistComponent);
       self:AddComponent(DamageReceiverComponent);
       self:AddComponent(DamageProcessorComponent);
       self:AddComponent(PossibleActionsComponent);
       self:AddComponent(CharacterDamageReceiver);
       self:AddComponent(EnemySupportComponent);
       self:AddComponent(CombatComponent);
       self:AddComponent(DamageVisualizerComponent);
       self:AddComponent(WeaponSequencerComponent);
       self:AddComponent(TimeManagerComponent);
       
       this.isChild = this.config:Get("isChild");
       this.parent = null;
       this.isAgressive = true;
       this.static = false;
       
       //this.luaMapper:EnableComplexVisibilityControl()
       
       this.isDeath = false;
       this.deathTime = 0;
       this.deathHideTimer = 0;
       this.characterClass = this.config:Get("characterClass")  ||  1;
 }})
 public void EnemyEntity:CanAttack(targetEntity){
   // check weapon for(target entity
   return this.weaponContainer:CanAttack(targetEntity);
 }
 public void EnemyEntity:Update(){
   if(this.isDeath  &&  this.deathHideTimer > 0  ){ 
     this.deathHideTimer = this.deathHideTimer - GameController.deltaTime;
     if(this.deathHideTimer <= 0  ){ 
       self:SetPosition(this.worldmap:GetLocation("home") + Vector3(0, -10, 0));
       var homeVec = this.worldmap:GetLocation("home");
       this.luaMapper:SetFrustumSpherePosition(homeVec.x, homeVec.y, homeVec.z);
       this.timeManager:Add("Respawn", 3) //this.config:Get("respawnTimeout")  ||  3)
     }
   }
   if(not this.isDeath  ){ 
     UnityExistsEntity.Update(self);
     ////
   }else{if not this.visible  ){  
     if(GameController.gameTime - this.deathTime > (this.config:Get("respawnTimeout")  ||  3)  ){ 
       print("Respawn: " .. this.name);
       self:Respawn();
     }
   //
   }
 }
 public void EnemyEntity:Save(storage){
   storage:SetBool("isDeath", this.isDeath);
   storage:SetFloat("deathTime", this.deathTime);
   storage:SetBool("isLua", true);
   storage:SetTransform(this.transform);
   self:Message("Save", storage);
 }
 public void EnemyEntity:Load(storage){
   storage:LoadTransform(this.transform);
   this.isDeath = storage:GetBool("isDeath", false);
   this.deathTime = storage:GetFloat("deathTime", 0);
   self:Message("Load", storage);
   this.mecanim:ForceSetState("Idle");
 }
 public void EnemyEntity.Create(storage, x, y, z){
   storage:SetBool("isLua", true);
   storage:SetPosition(x, y, z);
 }
 public void EnemyEntity:HasEmptyHitlist(){
   return this.relationship:GetInstance("enemy") == null;
 }
 public void EnemyEntity:ScanEnemies(){
   var targets = this.vision:GetVisibleEntitiesByRSTag("enemy");
   for(k, v in pairs(targets) ){
     if(v.interactable  ){ 
       this.relationship:AddInstance("enemy", v);
     }
   }
 }
 public void EnemyEntity:GetPriorityFood(){
   var targets = this.vision:GetVisibleEntitiesByRSTag("food");
   if(#targets > 0  &&  targets[1].CanBeFood  &&  targets[1]:CanBeFood()  ){ 
     return targets[1];
   }
   return null;
 }
 public void EnemyEntity:GetPriorityTarget(){
   var targets = this.relationship:GetInstances("enemy");
   if(#targets > 0  ){ 
     for(i = #targets, 1, -1 ){
       var v = targets[i];
       if(v.interactable  ){ 
         return v;
       }
     }
       ////
     for(k, v in pairs(targets) ){
       if(v.interactable  ){ 
         return v;
       }
       //targets[1]
     }
     //
     return null;
   }else{
     return null;
   }
 }
 public void EnemyEntity:Hit(damageData){
   var friends = this.relationship:GetTagTypes("friend");
   if(friends != null  &&  #friends > 0  ){ 
     var friendEntities = RaycastUtils.GetEntitiesInRadiusByTypes(self:GetPosition(), this.config:Get("supportRadius"), friends);
     for(k, v in pairs(friendEntities) ){
       if(v != self  ){ 
         v:Message("OnFriendAttacked", {source=self, target=damageData.source})
       }
     }
   }
 }
 public void EnemyEntity:OnEvent(data){
   UnityExistsEntity.OnEvent(self, data);
 }
 public void EnemyEntity:Message(...){
   var args = {...}
   if(#args > 0  ){ 
     if(args[1] == "Respawn"  ){ 
       return self:Respawn();
     }
   }
   UnityExistsEntity.Message(self, ...);
 }
 public void EnemyEntity:Respawn(){
   if(this.visible  ){  
     return false;
   }
   if(this.isDeath  ){ 
     self:SetPosition(this.worldmap:GetLocation("home"));
     this.interactable = true;
     this.isDeath = false;
     this.mecanim:ForceSetState("Idle");
     this.luaMapper:ResetFrustumSphere();
     self:Message("OnRespawn");
   }
   return true;
 }
 public void EnemyEntity:Death(){
   if(not this.isDeath  ){ 
     this.interactable = false;
     this.isDeath = true;
     this.mover:Stop();
     this.mecanim:ForceSetState("Death");
     //self:SetPosition(this.worldmap:GetLocation("home") + Vector3(0, -1 - this.height, 0))
     this.deathHideTimer = 3;
     this.deathTime = GameController.gameTime;
   }
 }
 public void EnemyEntity:Sleep(){
   if(this.visibilityTime < 0.5  ){ 
     this.mecanim:ForceSetState("Sleep");
   }else{
     this.mecanim:ForceSetState("SleepStart");
   }
   this.mecanim:SetBool("sleep", true);
   this.mover:Stop();
   this.isSleeping = true;
 }
 public void EnemyEntity:WakeUp(){
   if(this.visibilityTime < 0.5  ){ 
     this.mecanim:ForceSetState("Idle");
     this.mecanim:SetBool("sleep", false);
   }else{
     this.mecanim:SetBool("sleep", false);
   }
   this.isSleeping = false;
 }
 Entity}}