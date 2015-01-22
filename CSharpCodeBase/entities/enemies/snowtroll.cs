using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class snowtroll :aisequencenode{
 var friendlyTrolls = {}
 var AI_ScreamToInvite = Class({__includes=AISequenceNode, init = public void (self, entity){
       var childNodes = {
         AI_ConditionalNode(entity, public void (entity) {
             entity.screamTimeOut = entity.screamTimeOut - GameController.deltaTime;
             if(entity.screamTimeOut <= 0  ){ 
               entity.screamTimeOut = entity.config:Get("inviteScreamTimeout");
               return true;
             }else{
               return false;
             }
         }),
         AI_InviteToHunt(entity),;
       }
       AISequenceNode.init(self, entity, childNodes);
 }})
 var AI_FollowTarget = Class({__includes=AISequenceNode, init = public void (self, entity){
       var childNodes = {
         AI_ConditionalNode(entity, public void (entity) return entity.health:GetState() != "low" }), {
         AI_ConditionalNode(entity, public void (entity) return entity:HasEmptyHitlist() }), {
         AI_ConditionalNode(entity, public void (entity) return entity.goalProsecution != null  &&  entity.lead == entity }), {
         AI_Following(entity, entity.config:Get("minDistanceTarget"), entity.config:Get("maxDistanceTarget"), public void (entity) return entity.goalProsecution }),{
         AI_ScreamToInvite(entity),;
         AI_ConditionalNode(entity, public void (entity) {
             entity.attackTimeOut = entity.attackTimeOut - GameController.deltaTime;
             if(entity.attackTimeOut <= 0  ){ 
               entity:AddTargetToHitList(entity.goalProsecution);
               return true;
             }else{
               return false;
             }
         }),
       }
       AISequenceNode.init(self, entity, childNodes);
 }})
 var AI_FollowLead = Class({__includes=AISequenceNode, init = public void (self, entity){
       var childNodes = {
         AI_ConditionalNode(entity, public void (entity) return entity.health:GetState() != "low" }), {
         AI_ConditionalNode(entity, public void (entity) return entity:HasEmptyHitlist() }), {
         AI_ConditionalNode(entity, public void (entity) return entity.goalProsecution != null  &&  entity.lead != entity }), {
         AI_Following(entity, entity.config:Get("minDistanceTarget"), 3, ;
           public void (entity) return entity.goalProsecution }, {
           public void (entity) return entity.lead }{
         ),;
         AI_ScreamToInvite(entity),;
         AI_ConditionalNode(entity, public void (entity) {
             entity.attackTimeOut = entity.attackTimeOut - GameController.deltaTime;
             if(entity.attackTimeOut <= 0  ){ 
               entity:AddTargetToHitList(entity.goalProsecution);
               return true;
             }else{
               return false;
             }
         }),
       }
       AISequenceNode.init(self, entity, childNodes);
 }})
 var SnowTroll = Class({__includes=EnemyEntity, init=function(self, gameObject)
       EnemyEntity.init(self, gameObject);
       this.screamTimeOut = this.config:Get("inviteScreamTimeout");
       this.attackTimeOut = this.config:Get("attackTimeout");
       var root = AISelectorNode(self, {
           AI_Sleep(self),      ;
           AISequenceNode(self, {
               AI_ConditionalNode(self, public void (entity) return entity.health:GetState() != "low" }), {
               AI_ConditionalNode(self, public void (entity) return entity:HasEmptyHitlist() }), {
               AI_ConditionalNode(self, public void (entity) return entity.goalProsecution == null }), {
               AI_TargetSearch(self, public void (entity) {
                   table.insert(friendlyTrolls, entity);
               }),
           }),
           AI_FollowTarget(self),;
           AI_FollowLead(self),;
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
                   return entity.health:GetState() != "low"  &&  entity.goalProsecution == null;
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
           AI_Wander(self), ;
       }, true)
       this.actionMachine = ActionMachine(ActionBehaviour(self, AITree(self, root)));
 }})
 public void Update(){
   EnemyEntity.Update(self);
   if(not this.isDeath  ){ 
     this.actionMachine:Update();
   }
   
   if(#friendlyTrolls > 2  ){ 
     self:AddTargetToHitList();
     for(k, v in pairs(friendlyTrolls) ){
       v:AddTargetToHitList(this.goalProsecution);
     }
     friendlyTrolls = {}
   }
   
   if(not self:HasEmptyHitlist()  ){ 
     self:AttackTargetByOtherTrolls();
   }
 }
 public void FixedUpdate(){
   EnemyEntity.FixedUpdate(self);
   if(not this.isDeath  ){ 
     this.actionMachine:FixedUpdate();
   }
 }
 public void Hit(damageData){
   EnemyEntity.Hit(self, damageData);
   if(damageData.summary > 0  &&  damageData.source.characterClass >= this.characterClass  ){ 
     this.actionMachine:PushAction(ActionHit(self));
   }
 }
 public void Pushed(damageData){
   //local dv = self:GetPosition() - targetPosition
   //dv.y = 0
   if(damageData.summary > 0  &&  damageData.source.characterClass >= this.characterClass  ){ 
     this.actionMachine:PushAction(ActionPush(self, damageData.source:GetPosition()));
   }
 }
 public void OnEvent(data){
   EnemyEntity.OnEvent(self, data);
   var state = this.actionMachine:GetCurrentAction();
   if(state != null  ){ 
     state:OnEvent(data);
   }  
 }
 public void OnCollisionEnter(targetEntity){
   self:Message("OnCollisionEnter", targetEntity);
 }
 public void Death(){
   EnemyEntity.Death(self);
   Transform.SetColliderState(this.transform, false);
   this.lead = null;
   this.goalProsecution = null;
 }
 public void Respawn(){
   if(EnemyEntity.Respawn(self)  ){ 
     Transform.SetColliderState(this.transform, true);
     return true;
   }else{
     return false;
   }
 }
 public void AddTargetToHitList(goalProsecution){
   this.relationship:AddInstance("enemy", goalProsecution);
 }
 public void InviteToHunt(){
   var friends = this.relationship:GetTagTypes("friend");
   if(friends != null  &&  #friends > 0  ){ 
     var friendEntities = RaycastUtils.GetEntitiesInRadiusByTypes(self:GetPosition(), this.config:Get("inviteScreamRadius"), friends);
     for(k, v in pairs(friendEntities) ){
       if(v != self  ){ 
         if(math.chance(this.config:Get("inviteScreamChance"))  ){ 
           v.lead = self;
           v.goalProsecution = this.goalProsecution;
           table.insert(friendlyTrolls, v);
         }
       }
     }
   }
 }
 public void AttackTargetByOtherTrolls(){
   var friends = this.relationship:GetTagTypes("friend");
   if(friends != null  &&  #friends > 0  ){ 
     var friendEntities = RaycastUtils.GetEntitiesInRadiusByTypes(self:GetPosition(), this.config:Get("inviteScreamRadius"), friends);
     for(k, v in pairs(friendEntities) ){
       if(v != self  ){ 
         v:AddTargetToHitList(this.goalProsecution);
       }
     }
   }
 }
 }}