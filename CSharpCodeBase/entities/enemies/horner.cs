using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class horner :ainode{
 // Custom AI cases
 var AI_PlayAnimationStateAndLookAtTarget = Class({__includes=AINode, init=function(self, entity, state)
   AINode.init(self, entity);
   this.state = state;
 }})
 public void AI_PlayAnimationStateAndLookAtTarget:Visit(){
   if(this.status == NODE_READY  ){ 
     this.status = NODE_RUNNING;
     this.entity.mecanim:ForceSetState(this.state);
     this.isAnimationPlaying = false;
     this.entity.mover:Stop();
   }
   if(this.status == NODE_RUNNING  ){ 
     if(this.parent.selected_target != null  ){ 
       this.entity.mover:LookAt(this.parent.selected_target:GetPosition());
     }
     var statePlaying = this.entity.mecanim:CheckStateName(this.state);
     if(not this.isAnimationPlaying  &&  statePlaying  ){ 
       this.isAnimationPlaying = true;
     }
     if(this.isAnimationPlaying  &&  not statePlaying  ){ 
       this.status = NODE_SUCCESS;
     }
   }
   return this.status;
 }
 var AI_RunToTarget = Class({__includes=AINode, init=function(self, entity)
   AINode.init(self, entity);
   this.toDistance = this.entity.config:Get("angryLockTargetDistance")  ||  3;
 }})
 public void AI_RunToTarget:Visit(){
   if(this.status == NODE_READY  ){ 
     this.status = NODE_RUNNING;
   }
   if(this.status == NODE_RUNNING  ){ 
     if(this.entity:GetEffectiveDistance(this.parent.selected_target) < this.toDistance  ||  not this.parent.selected_target.interactable  ){ 
       this.status = NODE_SUCCESS;
     }else{
       this.entity.mover._useLoa = false;
       this.entity.mover:SetSpeed(this.entity.config:Get("angryRunSpeed"));
       this.entity.mover:SetInputToVec(this.parent.selected_target:GetPosition(), false, this.entity.config:Get("angryRunType"), true);
     }
   }
   return this.status;
 }
 var AI_RunToVector = Class({__includes=AINode, init=function(self, entity)
   AINode.init(self, entity);
 }})
 public void AI_RunToVector:Visit(){
   if(this.status == NODE_READY  ){ 
     this.status = NODE_RUNNING;
     this.vector = this.parent.selected_target:GetPosition() - this.entity:GetPosition();
     this.parent.vector = this.vector;
     this.maxDistance = (this.entity.config:Get("angryLockTargetDistance")  ||  3) * 1.2;
   }
   if(this.status == NODE_RUNNING  ){ 
     var distance = this.entity:GetEffectiveDistance(this.parent.selected_target);
     if(distance > this.maxDistance  ||  not this.parent.selected_target.interactable  ){ 
       this.parent.faststop = false;
       this.status = NODE_SUCCESS;
     }else{if distance < this.entity.config:Get("angryDistancePunch")  ){ 
       // punch!
       this.entity.weaponContainer.pushWeapon:Attack(this.parent.selected_target);
       this.parent.faststop = true;
       this.status = NODE_SUCCESS;
     }else{
       this.entity.mover:SetSpeed(this.entity.config:Get("angryRunSpeed"));
       this.entity.mover:SetInput(this.vector, this.entity.config:Get("angryRunType"), true);
     }
   }
   return this.status;
 }
 var AI_Stop = Class({__includes=AINode, init=function(self, entity)
   AINode.init(self, entity);
 }})
 public void AI_Stop:Visit(){
   if(this.status == NODE_READY  ){ 
     this.entity.angryState = false;
     this.status = NODE_RUNNING;
     this.vector = this.parent.vector;
     this.vector:Normalize();
     this.entity.mover:Stop();
     if(this.parent.faststop  ){ 
       this.entity.mecanim:ForceSetState("RunStopFast");
     }else{
       this.entity.mecanim:ForceSetState("RunStop");
     }
     this.isAnimationStarted = false;
   }
   if(this.status == NODE_RUNNING  ){ 
     var animatorState = this.entity.mecanim:CheckStateName("RunStop")  ||  this.entity.mecanim:CheckStateName("RunStopFast");
     
     if(animatorState  &&  not this.isAnimationStarted  ){ 
       this.isAnimationStarted = true;
     }
     
     if(this.isAnimationStarted  &&  not animatorState  ){ 
       this.status = NODE_SUCCESS;
       this.entity.mover:Stop();
     }
     
     var curveValue = this.entity.mecanim:GetFloat("speed_curve");
     RigidbodyUtils.Move(this.entity.rigidbody, this.vector * this.entity.config:Get("angryRunSpeed") * curveValue);
   }
   return this.status;
 }
 var AI_AngryRun = Class({__includes=AISequenceNode, init = public void (self, entity){
       var childNodes = {
         AI_SelectEnemy(entity),;
         AI_ConditionalNode(entity, public void (entity, self){
             var dst = this.entity:GetEffectiveDistance(this.parent.selected_target);
             var result = dst > this.entity.config:Get("angryRunMinDistance")  &&  dst < this.entity.config:Get("angryRunMaxDistance");
             this.entity.angryState = result;
             return result;
         }),
         AI_PlayAnimationStateAndLookAtTarget(entity, "RunPrepare"), // prepare to run
         AI_RunToTarget(entity),;
         AI_RunToVector(entity),;
         AI_Stop(entity);
       }
       AISequenceNode.init(self, entity, childNodes);
 }})
 // Class description
 var HornerEntity = Class({__includes=EnemyEntity, init=function(self, gameObject)
       EnemyEntity.init(self, gameObject);
       
       var root = AISelectorNode(self, {
           AI_Scream(self),;
           AI_Scared(self),;
           AI_AngryRun(self),;
           AISequenceNode(self, {
               AI_ConditionalNode(self, public void (entity){
                   return entity.starvation:GetState()  &&  entity:HasEmptyHitlist();
               }),
               AI_Fooding(self),;
           }),
           AI_Sleep(self),;
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
       this.weaponContainer.pushWeapon = this.weaponContainer:LoadWeapon(this.config:Get("pushWeapon"));
 }})
 public void HornerEntity:Update(){
   EnemyEntity.Update(self);
   if(not this.isDeath  ){ 
     this.actionMachine:Update();
   }
 }
 public void HornerEntity:FixedUpdate(){
   EnemyEntity.FixedUpdate(self);
   if(not this.isDeath  ){ 
     this.actionMachine:FixedUpdate();
   }
 }
 public void HornerEntity:Hit(damageData){
   EnemyEntity.Hit(self, damageData);
   if(damageData.summary > 0  &&  damageData.source.characterClass >= this.characterClass  ){ 
     this.actionMachine:PushAction(ActionHit(self));
   }
 }
 public void HornerEntity:Pushed(damageData){
   //local dv = self:GetPosition() - targetPosition
   //dv.y = 0
   if(damageData.summary > 0  &&  damageData.source.characterClass >= this.characterClass  ){ 
     this.actionMachine:PushAction(ActionPush(self, damageData.source:GetPosition()));
   }
 }
 public void HornerEntity:OnEvent(data){
   EnemyEntity.OnEvent(self, data);
   var state = this.actionMachine:GetCurrentAction();
   if(state != null  ){ 
     state:OnEvent(data);
   }  
 }
 public void HornerEntity:OnCollisionEnter(target){
   if(this.angryState  ){ 
     //print("Collision with " ..target.name)
     if(target.OnFragileForce  ){ 
       target:OnFragileForce({source=self})
     }
   }
 }
 Entity}}