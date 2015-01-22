using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class caterpillar :aisequencenode{
 var AI_AttackEnemiesCaterpillar = Class({__includes=AISequenceNode, init = public void (self, entity){
       var childNodes = {
         AI_SelectEnemy(entity),;
         AI_SelectWeapon(entity),;
         AI_ConditionalNode(entity, public void (entity, self){
             if(this.entity.weaponContainer.currentWeapon.config:Get("canDash")  ){ 
               var canAttack = this.entity.weaponContainer:CanAttack(this.parent.selected_target);
               if(not canAttack  ){ 
                 // try to jump
                 this.entity:JumpToEnemy(this.parent.selected_target);
                 return false;
               }else{
                 return true;
               }
             }else{
               return true;
             }
         }),
         AI_MoveToEnemy(entity),;
         AI_Timeout(entity, entity.weaponContainer:GetAttackPrepareTimeout()),;
         AI_AttackEnemy(entity),;
       }
       AISequenceNode.init(self, entity, childNodes);
 }})
 var CaterpillarEntity = Class({__includes=EnemyEntity, init=function(self, gameObject)
       EnemyEntity.init(self, gameObject);
       
       var root = AISelectorNode(self, {
           //AI_Scream(self),
           AI_Sleep(self),          ;
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
           AI_AttackEnemiesCaterpillar(self),;
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
       this.mover:SetCurve("speed_curve");
       self:AddComponent(FragileThingComponent);
 }})
 public void CaterpillarEntity:Update(){
   EnemyEntity.Update(self);
   if(not this.isDeath  ){ 
     this.actionMachine:Update();
   }
 }
 public void CaterpillarEntity:FixedUpdate(){
   EnemyEntity.FixedUpdate(self);
   if(not this.isDeath  ){ 
     this.actionMachine:FixedUpdate();
   }
 }
 public void CaterpillarEntity:Hit(damageData){
   EnemyEntity.Hit(self, damageData);
   if(damageData.summary > 0  ){ 
     this.actionMachine:PushAction(ActionHit(self));
   }
 }
 public void CaterpillarEntity:Pushed(damageData){
   //local dv = self:GetPosition() - targetPosition
   //dv.y = 0
   if(damageData.summary > 0  ){ 
     this.actionMachine:PushAction(ActionPush(self, damageData.source:GetPosition()));
   }
 }
 public void CaterpillarEntity:OnEvent(data){
   EnemyEntity.OnEvent(self, data);
   var state = this.actionMachine:GetCurrentAction();
   if(state != null  ){ 
     state:OnEvent(data);
   }  
 }
 public void CaterpillarEntity:OnFragile(){
   if(not this.isDeath  ){ 
     self:Death();
   }
 }
 public void CaterpillarEntity:OnCollisionEnter(targetEntity){
   self:Message("OnCollisionEnter", targetEntity);
 }
 public void CaterpillarEntity:Death(){
   EnemyEntity.Death(self);
   Transform.SetColliderState(this.transform, false);
 }
 public void CaterpillarEntity:Respawn(){
   if(EnemyEntity.Respawn(self)  ){ 
     Transform.SetColliderState(this.transform, true);
     return true;
   }else{
     return false;
   }
 }
 public void CaterpillarEntity:JumpToEnemy(target){
   this.actionMachine:PushAction(ActionDashPunch(self, target));
 }
 Entity}}