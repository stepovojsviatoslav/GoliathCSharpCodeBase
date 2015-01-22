using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class uibenchcontroller :unityexistsentity{
 public void init(self, gameObject)            ;
       UnityExistsEntity.init(self, gameObject)            ;
       this.benchController = gameObject:GetComponent("UIBenchController") ;
       this.benchController:SetupLuaController(self);
       this.luaMapper:SetAlwaysVisible(true);
       GameController.eventSystem:AddListener("BENCH_PANEL_ACTIVATE", self)      ;
       this.recipeList = RecipeList("data/config/recipes_bench.dat")   ;
       self:AddRecipes(this.recipeList:GetNames());
       this.benchController:Toggle(false)        ;
       this.benchController:SetCallback(this.Callback);
 }})
 public void Update(){
   UnityExistsEntity.Update(self)  ;
   if(GameController.inputService:RightButtonWasPressed("bench")  ){   
     self:OnHide();
   }
 }
 public void OnShow(){
   this.benchController:Toggle(true);
   var mainInterface = GameController.ui.mainInterface;
   mainInterface:AddContainer("bench", this.benchEntity);
   mainInterface.unityInterface:ToggleInventory(true);
   GameController.inputService:PushFrame("bench");
 }
 public void OnHide(){
   this.benchController:Toggle(false);
   var mainInterface = GameController.ui.mainInterface;
   mainInterface:RemoveContainer("bench");
   mainInterface.unityInterface:ToggleInventory(false);
   GameController.inputService:PopFrame();
 }
 public void OnEvent(event, source){
   if(event == "BENCH_PANEL_ACTIVATE"  ){     
     self:Initiate(source);
     self:OnShow()    ;
   }
 }
 public void GetCraftRecipe(item)  {
   this.select = this.recipeList:GetBackhashResult(item);
   if(this.select  ){  
     return this.select;
   }else{        
     return {}
   }  
 }
 public void GetDescription(item){
   var descriptions = this.recipeList:GetDescriptions()  ;
   if(descriptions[item]  ){     
     return descriptions[item];
   }else{
     return "";
   }
 }
 public void GetMaxCraftValue(resultItem)  {
   var items = this.recipeList:GetBackhashResult(resultItem);
   var counts = {}
   for(k, v in pairs(items) ){
     counts[k] = GameController.inventory:GetItemCount(v);
   }  
   return math.min(unpack(items))  ;
 }
 public void CraftRequest(item, count){
   if(this.select  ){ 
     for(k, v in pairs(this.select) ){      
       GameController.inventory:RemoveItems(v, 1);
     }
     GameController.inventory:AddItem(item, count);
     Console:Message("Craft");
   }
 }
 public void AddRecipes(recipes){
   var hides = this.recipeList:GetHides();
   for(_, v in pairs(recipes) ){
     if(not hides[v[1//  ){ 
       this.benchController:AddRecipe(v[1], v[2]);
     }
   }
 }
 public void AddRecipe(recipe)  {
   var recipes = this.recipeList:GetNames()  ;
   for(k, v in pairs (recipes) ){
     if(v[1] == recipe  ){ 
       this.benchController:AddRecipe(v[1], v[2]);
       break;
     }
   }
 }
 public void Remove(ingredients){
   for(k, v in ingredients ){
     var count, item = unpack(v);
     //Console:Message("Quantity: "..count..", Item: "..item)
   }
 }
 public void GetCountAvailableItem(item)  {
   return tonumber(GameController.inventory:GetItemCount(item));
 }
 public void GetResult(request){
   var result = this.recipeList:GetResultOnlyComponents(unpack(request));
   if(result  ){ 
     return {result, this.recipeList:GetHides()[result]}
   }else{
     return {result, null}
   }
 }
 public void Initiate(entity){
   this.benchEntity = entity;
 }
 // Callbacks
 public void Callback(method, ...)  {
   if(self[method] != null  ){       
     var result = self[method](self, ...);
     return result;
   }else{
     return {}  
   }
 }
 }}