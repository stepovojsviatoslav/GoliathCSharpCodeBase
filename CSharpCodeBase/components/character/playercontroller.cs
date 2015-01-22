using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 //public class playercontroller :component{
 public void init (self, entity)      ;
       Component.init(self, entity);
       this.entity.playerController = self;
       this.slots = {}      
       this.countSlots = 0;
       this.maxCountSlots = GameController.database:Get("heroes", "Common/maxCountSlots");
       this.currentSlot = 0                  ;
       this.characterGear = luanet.GameFacade.characterGear;
       this.characterGear:SetupFinalCallback(function (idx) self:OnUIFinalSelectionCallback(idx) }){
       this.characterGearEnabled = false;
 }})
 public void OnUIFinalSelectionCallback(idx){
   print("Select slot: " .. idx);
   if(idx + 1 != currentSlot  ){ 
     self:SelectSlot(idx + 1);
   }
 }
 public void LoadAndAdd(entityName){
   var characterEntity = self:LoadSlot(entityName);
   self:AddSlot(characterEntity);
 }
 public void LoadSlot(entityName)      {
   var bundleName = GameController.database:Get("heroes", entityName .. "/bundle");
   var character = BundleUtils.CreateFromBundle(bundleName);
   character.name = entityName;
   Transform.AddComponent(character, "LuaMapper");
   var characterEntity = CharacterEntity(character);
   return characterEntity;
 }
 public void AddSlot(entity){
   if(this.countSlots < this.maxCountSlots  ){         
     entity.gameObject:SetActive(false);
     table.insert(this.slots, entity);
     this.countSlots = this.countSlots + 1    ;
     return true    ;
   }
   return false;
 }
 public void DeleteSlot(number){
   if(number > 1  &&  number <= this.countSlots  ){ 
     var deletedSlot = this.currentSlot;
     self:SelectSlot (1);
     deletedSlot = table.remove(this.slots, deletedSlot);
     deletedSlot:Destroy ();
     this.countSlots = this.countSlots - 1;
     return true;
   }
   return false;
 }
 public void DeleteCurrentSlot(){
   var deletedSlot = this.currentSlot;
   self:SelectSlot (1);
   deletedSlot = table.remove(this.slots, deletedSlot);
   deletedSlot:Destroy ();
   this.countSlots = this.countSlots - 1;
   return true;
 }
 public void GetSlot(number){
   if(number > 0  &&  number <= this.countSlots  ){ 
     return this.slots[number];
   }else{
     return null;
   }
 }
 public void SelectSlot(number){
   if(number != this.currentSlot  &&  number > 0  &&  number <= this.countSlots  ){ 
     if(this.currentSlot != 0  ){ 
       var oldSlot = self:GetCurrentSlot();
       oldSlot.gameObject:SetActive(false);
       oldSlot:OnDeselectCharacter();
       oldSlot:Message("OnDeselectCharacter");
       GameController:RemoveEntity(oldSlot);
       this.currentSlot = number;
     }else{      
       this.currentSlot = 1;
     }
     var newSlot = self:GetCurrentSlot();
     newSlot:SetPosition(this.entity:GetPosition());
     newSlot:SetRotation(this.entity:GetRotation());
     newSlot.gameObject:SetActive(true);
     GameController:AddEntity(newSlot);
     newSlot:OnSelectCharacter();
     newSlot:Message("OnSelectCharacter");
     GameController.camera:LoadForEntity(newSlot);
     return true;
   }else{
     return false;
   }
 }
 public void GetCurrentSlot(){
   if(this.currentSlot > 0  ){ 
     return self:GetSlot(this.currentSlot);
   }else{
     return null;
   }
 }
 public void Update(){
   Component.Update(self);
   var slot = self:GetCurrentSlot();
   if(slot  ){ 
     this.entity:SetPosition(slot:GetPosition());
     this.entity:SetRotation(slot:GetRotation());
   }
   if(not this.characterGearEnabled  &&  GameController.inputService:LeftBumperIsPressed()  ){ 
     this.characterGearEnabled = true;
     for(i = 1,5 ){ 
       var islot = this.slots[i];
       if(islot != null  ){ 
         this.characterGear:SetupSlot(i - 1, islot.config:Get("cgear_icon_left"), ;
           islot.config:Get("cgear_icon_top"),;
           islot.config:Get("cgear_icon_bottom"));
       }else{
         this.characterGear:SetupSlot(i - 1, "", "", "");
       }
     }
     this.characterGearSelected = this.currentSlot - 1;
     this.characterGear:Show(this.currentSlot - 1);
     GameController.inputService:PushFrame("character_gear");
   }
   if(this.characterGearEnabled  &&  not GameController.inputService:LeftBumperIsPressed("character_gear")  ){ 
     this.characterGearEnabled = false;
     this.characterGear:Hide();
     GameController.inputService:PopFrame();
   }
   if(this.characterGearEnabled  ){ 
     if(GameController.inputService:IsGamepad()  ){ 
       var lookVector = GameController.inputService:GetLookValue("character_gear");
       var selected = this.characterGearSelected;
       //selected = this.currentSlot - 1
       if(lookVector:Length() > 0.4  ){ 
         var angle = math.atan2(lookVector.z, lookVector.x) * 180 / math.pi;
         while(angle < 0 ){ angle = angle + 360 }
         if(angle > 315  ||  angle <= 45  ){ 
           selected = 1;
         }else{if angle > 225  &&  angle <= 315  ){ 
           selected = 0;
         }else{if angle > 135  &&  angle <= 225  ){ 
           selected = 3;
         }else{ 
           selected = 2;
         }
       }
       if(this.slots[selected + 1] == null  ){ 
         selected = this.characterGearSelected;
         //selected = this.currentSlot - 1
       }
       if(selected != this.characterGearSelected  ){ 
         // update
         this.characterGear:UpdateSelected(selected);
         this.characterGearSelected = selected;
       }
     }
   }
 }
 }}