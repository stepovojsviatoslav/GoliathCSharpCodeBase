using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class playercontroller :component{
 public void init(self, entity);
       Component.init(self, entity);
       this.actionMachine = ActionMachine();
       this.luaPool = GameController.luaPooler;
       this.luaPool:CreatePool(ActionMove);
       this.luaPool:CreatePool(ActionBlock);
       this.luaPool:CreatePool(ActionInteract);
       this.luaPool:CreatePool(ActionHit);
       this.luaPool:CreatePool(ActionPush);
       this.luaPool:CreatePool(ActionTake);
       this.luaPool:CreatePool(ActionAttackMouse);
       this.spaceCommandTimer = 0;
       this.raycastScreenTimer = 0;
       this.currentLabelEntity = null;
       this.mouseLabel = luanet.GameFacade.uiMouseUnderLabel;
       this.mouseLabel:Hide();
       this.entity.playerController = self;
       this.inputService = GameController.inputService;
 }})
 public void IsMoveKeyPressed(){
   return this.inputService:LeftStickYIsPressed()  ||  this.inputService:LeftStickXIsPressed();
 }
 public void FixedUpdate(){
   this.actionMachine:FixedUpdate();
 }
 public void GetInteractEntityUnderMouse(){
   var target = Input.RaycastTargetEntity();
   if(target != null  &&  target.possibleActions != null  &&  target.interactable  ){ 
     if(target.possibleActions:GetAction(1) == "take"  ||  target.possibleActions:GetAction(1) == "interact"  ){ 
       return target;
     }
   }  
   return null;
 }
 public void GetNearInteractEntity(radius){
   var entities = RaycastUtils.GetEntitiesInRadius(this.entity:GetPosition(), radius);
   var min = radius * 10;
   var minEntity = null;
   
   for(k, v in pairs(entities) ){    
     if(v != null  &&  v.possibleActions != null  &&  v.interactable  ){ 
       if(v.possibleActions:GetAction(1) == "take"  ||  v.possibleActions:GetAction(1) == "interact"  ){ 
         var dst = v:GetSimpleDistance(this.entity);
         if(dst < min  ){ 
           min = dst;
           minEntity = v;
         }
       }
     }
   }  
   return minEntity;
 }
 public void Update(){
   self:SearchingThings() ;
   self:Interact();
   self:SpecialAction();
   self:Move();
   self:Attack();
   this.actionMachine:Update();
 }
 public void SpecialAction(){
   if((this.inputService:RightTriggerWasPressed()  &&  not this.actionMachine:IsActionExists("block")) ;
      ||  (this.inputService:RightTriggerWasPressed()  &&  this.actionMachine:IsEmpty())then;
     this.actionMachine:PushAction(this.luaPool:Fetch(ActionBlock, this.entity));
   }
  }
  
  public void SearchingThings(){
     this.raycastScreenTimer = this.raycastScreenTimer - GameController.deltaTime;
     if(this.raycastScreenTimer < 0  &&  not this.actionMachine:IsActionExists("take")  ){ 
       this.raycastScreenTimer = 0.2;
       var previousLabelEntity = this.currentLabelEntity;
       this.currentLabelEntity = self:GetInteractEntityUnderMouse()  ||  self:GetNearInteractEntity(3);
       if(this.currentLabelEntity != null  ){ 
         var pos = this.currentLabelEntity:GetPosition();
         this.mouseLabel:Show(pos.x, pos.y + this.currentLabelEntity.height, pos.z, ""E" to take");
       }else{if previousLabelEntity == null  ){ 
         this.mouseLabel:Hide();
       }
     }
  }
  
  public void Interact(){
     if(this.inputService:BottomButtonWasPressed()  &&  this.currentLabelEntity != null  ){ 
       this.mouseLabel:Hide();
       this.actionMachine:PushAction(this.luaPool:Fetch(ActionTake, this.entity, this.currentLabelEntity));
     }
  }
  
  public void Move(){
   this.spaceCommandTimer = this.spaceCommandTimer - GameController.deltaTime;
   if(not this.actionMachine:IsActionExists("block")  &&  (self:IsMoveKeyPressed()  ||  this.inputService:GetMouseButton(1))  ){ 
     if(not this.actionMachine:IsActionExists("move")  ){ 
       if(this.inputService:GetMouseButtonDown(1)  ){ 
         this.actionMachine:PushAction(this.luaPool:Fetch(ActionMove, this.entity, Input.RaycastMouseOnTerrain()));
       }else{
         this.actionMachine:PushAction(this.luaPool:Fetch(ActionMove, this.entity));
       }
     }
   }
  }
  
  public void Attack(){
   var needAttack = this.inputService:LeftButtonWasPressed();
   if(not this.inputService:IsGamepad()  ){ 
     if(Input.IsPointerOverUI()  ){ 
       needAttack = false;
     }
   }
   needAttack = needAttack  &&  GameController.ui.mainInterface:IsAttackPossible();
   if(needAttack  ){ 
     if(this.entity.grenadeModeVisualizer.active  ){ 
       var grenade = Grenade(this.entity.grenadeModeVisualizer.name, this.entity, this.entity.grenadeModeVisualizer:GetSpeed());
     }
     
     var vec3;
     if(this.entity.gamepadRightStickController  &&  this.entity.gamepadRightStickController:GetTarget()  ){ 
       vec3 = this.entity.gamepadRightStickController:GetTarget():GetPosition();
       this.entity.mover:LookAt(this.entity.gamepadRightStickController:GetTarget():GetPosition());
     }else{
       vec3 = this.entity:GetPosition();
     }
     this.actionMachine:PushAction(this.luaPool:Fetch(ActionAttackMouse, this.entity, vec3));
   }
  }
  
 public void Hit(damageData)  {
   if(damageData.summary != 0  &&  this.entity.characterClass <= damageData.source.characterClass  ){ 
     this.entity.mecanim:ForceSetState("Empty", 1);
     this.actionMachine:PushAction(this.luaPool:Fetch(ActionHit, this.entity));
   }
 }
 public void Pushed(damageData){
   if(this.entity.characterClass <= damageData.source.characterClass  ){ 
     this.entity.mecanim:ForceSetState("Empty", 1);
     this.actionMachine:PushAction(ActionPush(this.entity, damageData.source:GetPosition()));
   }
 }
 public void OnDeselectCharacter(){
   this.actionMachine:Flush();
 }
 public void OnSelectCharacter(){
 }
 public void OnRespawn(){
   this.actionMachine:Flush();
 }
 public void OnEvent(data){
   var state = this.actionMachine.buffer[#this.actionMachine.buffer];
   if(state != null  ){ 
     state:OnEvent(data);
   }
 }
 public void OnSpellCast(spellAction)  {
   spellAction.entity = this.entity;
   this.actionMachine:PushAction(spellAction);
   return true;
 }
 }}