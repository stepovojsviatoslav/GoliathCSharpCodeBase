using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class uichestcontroller :unityexistsentity{
 public void init(self, gameObject)            ;
       UnityExistsEntity.init(self, gameObject)         ;
       GameController.eventSystem:AddListener("CHEST_PANEL_ACTIVATE", self);
       this.luaMapper:SetAlwaysVisible(true);
       this.chestController = gameObject:GetComponent("UIChestController")            ;
       this.chestController:Toggle(false);
       this.chestController:SetupLuaController(self);
       this.chestController:SetCallback(this.Callback)          ;
 }})
 public void Update(){
   UnityExistsEntity.Update(self)  ;
   if(Input.GetKeyDown(KeyCode.Escape)  ){ 
     self:OnHide();
   }
 }
 public void OnShow(){
   this.chestController:Toggle(true);
   var mainInterface = GameController.ui.mainInterface;
   mainInterface:AddContainer("chest", this.chestEntity);
   mainInterface.unityInterface:ToggleInventory(true)  ;
 }
 public void OnHide(){
   this.chestController:Toggle(false);
   var mainInterface = GameController.ui.mainInterface;
   mainInterface:RemoveContainer("chest");
   mainInterface.unityInterface:ToggleInventory(false);
 }
 public void OnEvent(event, source){
   if(event == "CHEST_PANEL_ACTIVATE"  ){ 
     self:Initiate(source);
     self:OnShow()    ;
   }
 }
 public void Initiate(entity){
   this.chestEntity = entity;
 }
 // Callbacks
 public void Callback(method, ...){
   if(self[method] != null  ){ 
     self[method](self, ...);
   }
 }
 }}