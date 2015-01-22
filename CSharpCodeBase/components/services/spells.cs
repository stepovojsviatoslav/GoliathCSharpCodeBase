using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class spells {
 var SpellSystem = Class({init=function(self)
    this.currentSpellAction = null;
    this.statuses = {}
 }})
 public void SpellSystem:GetSpell(spellName){
   var spell = GameController.database:Get("spells", spellName);
   spell.name = spellName  ;
   return spell;
 }
 public void SpellSystem:StartSpell(spellName){
   var spell = self:GetSpell(spellName);
   var spellActionClass = require(spell.class);
   this.currentSpellAction = spellActionClass(spellName);
   this.currentSpellAction:BeginDraw();
 }
 public void SpellSystem:DropSpell(){
   if(this.currentSpellAction != null  ){ 
     this.currentSpellAction:StopDraw();
   }
 }
 public void SpellSystem:ApplySpell(spellName, position)   {
   if(spellName == ""  &&  spellName == null  ){  return false }
   var spell = self:GetSpell(spellName);
   var spellActionClass = require(spell.class);
   if(spellActionClass.CanApply(position)  ){ 
     //if this.currentSpellAction == null  ){ 
       this.currentSpellAction = spellActionClass(spellName)      ;
     //end
     if(position  ){                
       this.currentSpellAction.position = position;
     }else{      
       this.currentSpellAction.position = null;
     }
     GameController.player.playerController:GetCurrentSlot():OnSpellCast(this.currentSpellAction)        ;
     return true;
   }else{    
     return false;
   }
 }
 public void SpellSystem:SaveStatuses(){
   var currentHero = GameController.player.playerController:GetCurrentSlot();
   if(not this.statuses[currentHero.name]  ){ 
     this.statuses[currentHero.name] = {}    
   }
   for(i=1, 4 ){        
     var status = GameController.ui.mainInterface:GetSpellStatus(i);
     this.statuses[currentHero.name][i] = status;
     if(status.mode == "active"  ||  status.mode == "recharge"  ){       
       currentHero:SetTimer(i, status.timeout);
     }
   }
 }
 public void SpellSystem:LoadStatuses(){
   var currentHero = GameController.player.playerController:GetCurrentSlot();
   if(this.statuses[currentHero.name]  ){ 
     for(i=1, 4 ){
       GameController.ui.mainInterface:SetSpellStatus(i, this.statuses[currentHero.name][i]);
       this.statuses[currentHero.name][i] = {}
     }
     currentHero.timeManager:Clear();
     this.statuses[currentHero.name] = null;
   }  
 }
 public void SpellSystem:TimerUpdate(hero, action, timeout)  {
   if(this.statuses[hero]  ){ 
     this.statuses[hero][action]["timeout"] = timeout;
   }
 }
 public void SpellSystem:SetupSpells(spellNames)  {
   GameController.ui.mainInterface:ResetSpells()  ;
   if(spellNames != null  ){     
     for(k, v in pairs(spellNames) ){
       var spell = GameController.database:Get("spells", v);
       GameController.ui.mainInterface:SetupSpell(spell.action, v, spell.icon, spell.recharge_timeout  ||  1, spell.useCursor, spell.action_timeout)      ;
     }
   }
   self:LoadStatuses();
 }
 ystem}}