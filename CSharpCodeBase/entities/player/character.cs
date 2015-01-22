using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using MainGame.core;
//using UnityEngine.EventSystems;

namespace MainGame
{
    public class Character : UnityEntity
    {
        Character (GameObject gameObject)
        {
            base(gameObject);
            ConfigComponent config = ConfigComponent("heroes", name);
        }
        //      this.rigidbody = this.gameObject:GetComponent("Rigidbody");
        //      self:AddComponent(HealthComponent);
        //      self:AddComponent(CharacterDamageReceiver);
        //      self:AddComponent(DamageVisualizerComponent);
        //      self:AddComponent(MecanimComponent)      ;
        //      self:AddComponent(Mover)        ;
        //      self:AddComponent(CharacterWeaponContainer)      ;
        //      self:AddComponent(CombatComponent);
        //      self:AddComponent(ResistComponent);
        //      self:AddComponent(DamageProcessorComponent);
        //      self:AddComponent(DamageReceiverComponent);
        //      self:AddComponent(PlayerControllerComponent);
        //      self:AddComponent(PossibleActionsComponent);
        //      self:AddComponent(GrenageVisualizer);
        //      if(GameController.inputService:IsGamepad()  ){ 
        //        self:AddComponent(GamePadMover);
        //      }
        //      self:AddComponent(TimeManagerComponent);
        //      this.luaMapper:SetAlwaysVisible(true);
        //      this.isDeath = false;
        //      this.deathTimeout = 0;
        //      this.characterClass = this.config:Get("characterClass")  ||  1;
        //      this.static = false;
        //      this.mover.autoLook = false;
        //}})
        //public void CharacterEntity:Update(){
        //  //// for(left trigger
        //  if(this.gamepadRightStickController  ){ 
        //    if(GameController.inputService:LeftTriggerIsPressed()  ){ 
        //      this.gamepadRightStickController.active = false;
        //      this.gamepadRightStickController:Disable();
        //    }else{
        //      this.gamepadRightStickController.active = true;
        //    }
        //  }
        //  //  
        //  if(not this.isDeath  ){ 
        //    UnityExistsEntity.Update(self);
        //  }else{
        //    this.deathTimeout = this.deathTimeout - GameController.deltaTime;
        //    if(this.deathTimeout <= 0  ){ 
        //      self:Respawn();
        //    }
        //  }
        //}
        //public void CharacterEntity:FixedUpdate(){
        //  if(not this.isDeath  ){ 
        //    UnityExistsEntity.FixedUpdate(self);
        //  }
        //}
        //public void CharacterEntity:CanAttack(targetEntity){
        //  // check weapon for(target entity
        //  return this.weaponContainer:CanAttack(targetEntity);
        //}
        //public void CharacterEntity:OnEvent(data){
        //  UnityExistsEntity.OnEvent(self, data);
        //}
        //public void CharacterEntity:Hit(damageData){
        //  //if damageData.summary != 0  ){ 
        //    self:Message("Hit", damageData);
        //  //end
        //}
        //public void CharacterEntity:Pushed(damageData){
        //  //if damageData.summary != 0  ){ 
        //    self:Message("Pushed", damageData);
        //  //end
        //}
        //public void CharacterEntity:Death(){
        //  if(this.characterClass > 1  ){ 
        //    // mech
        //    this.mover:Stop();
        //    this.isDeath = true;
        //    this.interactable = false;
        //    this.enabled = false;
        //    GameController.player.playerController:DeleteCurrentSlot();
        //  }else{
        //    this.mecanim:ForceSetState("Death");
        //    this.isDeath = true;
        //    this.interactable = false;
        //    this.deathTimeout = 5;
        //    this.mover:Stop();
        //  }
        //}
        //public void CharacterEntity:Respawn(){
        //  self:Message("OnRespawn");
        //  self:SetPosition(GameController.player.homePosition) ;
        //  GameController.worldController:TeleportTargetTransform(this.transform, ;
        //    GameController.player.homePosition.x,;
        //    GameController.player.homePosition.y,;
        //    GameController.player.homePosition.z);
        //  this.mecanim:ForceSetState("Idle");
        //  this.interactable = true;
        //  this.isDeath = false;
        //}
        //public void CharacterEntity:OnSelectCharacter(){
        //  GameController.ui.SetupIcon(this.config:Get("cpanel_icon"));
        //  GameController.ui.SetupAmount(this.health:GetPercentAmount());
        //  //GameController.ui.UpdateHealth(this.health:GetPercentAmount())
        //  print("Spell system initialization!");
        //  GameController.spellSystem:SetupSpells(this.config:Get("spell"));
        //  //GameController.ui.slotsController:SetContainerSubTag("weapon", this.config:Get("weaponTag"))  
        //  this.interactable = true;
        //  this.enabled = true;
        //}
        //public void CharacterEntity:OnDeselectCharacter()  {
        //  GameController.spellSystem:SaveStatuses();
        //  this.interactable = false;
        //  this.enabled = false;
        //  if(this.gamepadRightStickController  ){ 
        //    this.gamepadRightStickController:DropTarget();
        //  }  
        //}
        //public void CharacterEntity:OnSpellCast(spell){
        //  return this.playerController:OnSpellCast(spell);
        //}
        //public void CharacterEntity:OnHealthChanged(){
        //  GameController.ui.SetupAmount(this.health:GetPercentAmount());
        //  //GameController.ui.UpdateHealth(this.health:GetPercentAmount())
        //}
        //public void CharacterEntity:OnCollisionEnter(targetEntity){
        //}
        //public void CharacterEntity:SetTimer(name, value)  {
        //  this.timeManager:Add("TimerHandler", value, name, "extended");
        //}
        //public void CharacterEntity:TimerHandler(tResult)    {
        //  if(tResult.timeLeft <= 0  ){ 
        //    tResult.complete = true;
        //    GameController.spellSystem:TimerUpdate(this.name, tResult.name, 0)    ;
        //  }else{
        //    GameController.spellSystem:TimerUpdate(this.name, tResult.name, tResult.timeLeft);
        //  }  
        //}
        //Entity
    }
}