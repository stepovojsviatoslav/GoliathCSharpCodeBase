using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class uigadgetcontroller :entity{
 var UIGadgetController= Class({__includes=Entity, init=function(self)            
       Entity.init(self)   ;
       this.gameObject = luanet.UnityEngine.GameObject.Find("UIGadgetsPanel");
       this.gadgetController = this.gameObject:GetComponent("UIGadgetController");
       this.gadgetController:SetupLuaController(self);
       this.gadgetController:SetCallback(this.Callback)     ;
       this.inputService = GameController.inputService;
       this.enabled = true;
       this.topGadget = {}
       this.botGadget = {}
       this.leftGadget = {}
       this.rightGadget = {}
       this.activeGadget = {}
       this.activePanel = -1;
       this.gadgetController:Hide();
       this.gadgetController:SetText("");
 }})
 public void OnGadgetButtonClick(){
   if(#this.activeGadget > 0  &&  GameController.inventory:GetItemCount(this.activeGadget[1]) > 0  ){ 
     var characterEntity = GameController.player.playerController:GetCurrentSlot();
     var path = GameController.database:Get("items", this.activeGadget[1] .."/require");
     var Gadget = require(path);
     var gadget = Gadget(characterEntity, this.activeGadget[1]);
     gadget:Apply();
   }else{
     this.activeGadget = {}
     this.activePanel = -1;
   }
 }
 public void OnInventaryChanged(){
   if(#this.activeGadget > 0  &&  GameController.inventory:GetItemCount(this.activeGadget[1]) > 0  ){ 
     this.gadgetController:SetText(GameController.inventory:GetItemCount(this.activeGadget[1]));
   }else{
     this.gadgetController:SetText("");
   }
 }
 public void Update(){
   Entity.Update(self);
   if(this.inputService:RightButtonWasPressed()  ){ 
     this.gadgetController:Hide();
   }
   
   if(this.inputService:TopButtonWasPressed()  ){ 
     self:OnGadgetButtonClick();
   }
   
   if(this.inputService:DPadRightIsPressed()  ){ 
     self:DPadClick(2);
   }else{if this.inputService:DPadLeftIsPressed()  ){ 
     self:DPadClick(1);
   }else{if this.inputService:DPadUpIsPressed()  ){ 
     self:DPadClick(0);
   }else{if this.inputService:DPadDawnIsPressed()  ){ 
     self:DPadClick(3);
   }else{if this.inputService:GetKey(KeyCode.H)  ){ 
     if(not this.gadgetController.active   ){ 
       this.gadgetController:Show();
     }
   }else{if this.gadgetController.active  ){ 
     this.gadgetController:Hide();
   }
 }
 public void DPadClick(count){
    this.activePanel = count;
    
    if(not this.gadgetController.active   ){ 
       this.gadgetController:Show();
    }
    this.gadgetController:ShowPanel(this.activePanel);
 }
 public void GetGadgets(){
   var tbl = {}
   for(k,v in pairs(GameController.inventory.grids.inventory:GetFilteredItems("gadget")) ){ 
     var subTbl = {}
     table.insert(subTbl, v.name);
     table.insert(subTbl, v.count);
     table.insert(subTbl, GameController.database:Get("items", v.name .."/middle_icon"));
     table.insert(tbl, subTbl);
   }
   return tbl;
 }
 public void OnHide(){
   this.buttonClickTime = 0;
   this.inputService:PopFrame();
 }
 public void OnShow(){
   this.inputService:PushFrame("gadgets_menu");
 }
 public void SetLeftGadget(name, count){
   this.leftGadget = {}
   table.insert(this.leftGadget, name);
   table.insert(this.leftGadget, count);
   table.insert(this.leftGadget, GameController.database:Get("items", name .."/middle_icon"));
 }
 public void SetRightGadget(name, count){
   this.rightGadget = {}
   table.insert(this.rightGadget, name);
   table.insert(this.rightGadget, count);
   table.insert(this.rightGadget, GameController.database:Get("items", name .."/middle_icon"));
 }
 public void SetTopGadget(name, count){
   this.topGadget = {}
   table.insert(this.topGadget, name);
   table.insert(this.topGadget, count);
   table.insert(this.topGadget, GameController.database:Get("items", name .."/middle_icon"));
 }
 public void SetBotGadget(name, count){
   this.botGadget = {}
   table.insert(this.botGadget, name);
   table.insert(this.botGadget, count);
   table.insert(this.botGadget, GameController.database:Get("items", name .."/middle_icon"));
 }
 public void SetActiveGadget(name, count){
   this.activeGadget = {}
   table.insert(this.activeGadget, name);
   table.insert(this.activeGadget, count);
   table.insert(this.activeGadget, GameController.database:Get("items", name .."/middle_icon"));
   this.gadgetController:SetText(count);
 }
 public void GetRightGadget(){
   return this.rightGadget;
 }
 public void GetLeftGadget(){
   return this.leftGadget;
 }
 public void GetTopGadget(){
   return this.topGadget;
 }
 public void GetBotGadget(){
   return this.botGadget;
 }
 public void GetActivePanel(){
   return this.activePanel;
 }
 }}