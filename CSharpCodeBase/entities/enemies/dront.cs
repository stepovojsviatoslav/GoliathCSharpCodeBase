using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class dront :enemyentity{
 var DrontEntity = Class({__includes=EnemyEntity, init=function(self, gameObject)
       EnemyEntity.init(self, gameObject);
       
       var root = AISelectorNode(self, {
           AI_Sleep(self),          ;
           AI_Scream(self),;
           AI_Scared(self),;
           AI_ScaredClass(self),;
           AISequenceNode(self, {
               AI_ConditionalNode(self, public void (entity){
                   return entity.starvation:GetState()  &&  entity:HasEmptyHitlist();
               }),
               AI_Fooding(self),;
           }),
           AISequenceNode(self, {
               AI_ConditionalNode(self, public void (entity){
                   return entity.health:GetState() != "low";
               }),
               AI_ScanEnemies(self),;
           }),
           AI_AttackEnemies(self),;
           AISequenceNode(self, {
               AI_ConditionalNode(self, public void (entity){
                   return entity.health:GetState() != "low";
               }),
               AI_TargetZone(self),;
           }),          
           //AI_Child(self),
           AI_ReturnHome(self),;
           AI_Wander(self), ;
       }, true)
       this.actionMachine = ActionMachine(ActionBehaviour(self, AITree(self, root)));
       self:AddComponent(FragileThingComponent);
 }})
 public void DrontEntity:Update(){
   EnemyEntity.Update(self);
   if(not this.isDeath  ){ 
     this.actionMachine:Update();
   }
 }
 public void DrontEntity:FixedUpdate(){
   EnemyEntity.FixedUpdate(self);
   if(not this.isDeath  ){ 
     this.actionMachine:FixedUpdate();
   }
 }
 public void DrontEntity:Hit(damageData){
   EnemyEntity.Hit(self, damageData);
   if(damageData.summary > 0  &&  damageData.source.characterClass >= this.characterClass  ){ 
     this.actionMachine:PushAction(ActionHit(self));
   }
 }
 public void DrontEntity:Pushed(damageData){
   //local dv = self:GetPosition() - targetPosition
   //dv.y = 0
   if(damageData.summary > 0  &&  damageData.source.characterClass >= this.characterClass  ){ 
     this.actionMachine:PushAction(ActionPush(self, damageData.source:GetPosition()));
   }
 }
 public void DrontEntity:OnEvent(data){
   EnemyEntity.OnEvent(self, data);
   var state = this.actionMachine:GetCurrentAction();
   if(state != null  ){ 
     state:OnEvent(data);
   }  
 }
 public void DrontEntity:OnFragile(){
   if(not this.isDeath  ){ 
     self:Death();
   }
 }
 public void DrontEntity:OnCollisionEnter(targetEntity){
   self:Message("OnCollisionEnter", targetEntity);
 }
 public void DrontEntity:Death(){
   EnemyEntity.Death(self);
   Transform.SetColliderState(this.transform, false);
 }
 public void DrontEntity:Respawn(){
   if(EnemyEntity.Respawn(self)  ){ 
     Transform.SetColliderState(this.transform, true);
     return true;
   }else{
     return false;
   }
 }
 Entity}}