using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class spider :aisequencenode{
 var AI_AttackEnemiesSpider = Class({__includes=AISequenceNode, init = public void (self, entity){
       var childNodes = {
         AI_SelectEnemy(entity),;
         AI_ConditionalNode(entity, public void (entity, self){
             if(entity:GetEffectiveDistance(this.parent.selected_target) > this.entity.config:Get("throwMinDistance")  ){ 
               this.entity.weaponContainer:SetWeapon("spider_throw");
             }else{
               var nextWeapon = this.entity.weaponSequencer:GetNextWeapon();
               this.entity.weaponContainer:SetWeapon(nextWeapon)            ;
             }
             return true;
         }),        
         AI_MoveToEnemy(entity),;
         AI_Timeout(entity, entity.weaponContainer:GetAttackPrepareTimeout()),;
         AI_AttackEnemy(entity),;
       }
       AISequenceNode.init(self, entity, childNodes);
 }})
 var SpiderEntity = Class({__includes=EnemyEntity, init=function(self, gameObject)
       EnemyEntity.init(self, gameObject);
       
       var root = AISelectorNode(self, {
           AI_Sleep(self, {GameController.daytime.PHASE_MORNING, GameController.daytime.PHASE_DAY_START, GameController.daytime.PHASE_DAY_PROGRESS, GameController.daytime.PHASE_DAY_END}),          
           AI_Scream(self),;
           AI_Scared(self),;
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
           AI_AttackEnemiesSpider(self),;
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
       this.remoter = math.chance(50);
       this.weaponContainer.remoteWeapon = this.weaponContainer:LoadWeapon("spider_throw");
 }})
 public void SpiderEntity:Update(){
   EnemyEntity.Update(self);
   if(not this.isDeath  ){ 
     this.actionMachine:Update();
   }
 }
 public void SpiderEntity:FixedUpdate(){
   EnemyEntity.FixedUpdate(self);
   if(not this.isDeath  ){ 
     this.actionMachine:FixedUpdate();
   }
 }
 public void SpiderEntity:Hit(damageData){
   EnemyEntity.Hit(self, damageData);
   if(damageData.summary > 0  &&  not this.isSleeping  ){ 
     this.actionMachine:PushAction(ActionHit(self));
   }
 }
 public void SpiderEntity:Pushed(damageData){
   //local dv = self:GetPosition() - targetPosition
   //dv.y = 0
   if(damageData.summary > 0  ){ 
     this.actionMachine:PushAction(ActionPush(self, damageData.source:GetPosition()));
   }
 }
 public void SpiderEntity:OnCollisionEnter(target){
   if(this.isSleeping  ){ 
     this.relationship:AddInstance("enemy", target);
     self:WakeUp();
   }
 }
   
 public void SpiderEntity:OnEvent(data){
   EnemyEntity.OnEvent(self, data);
   var state = this.actionMachine:GetCurrentAction();
   if(state != null  ){ 
     state:OnEvent(data);
   }  
 }
 Entity}}