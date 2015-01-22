using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class recipelist {
 var readfile = function(name) ;
   return FileUtils.GetFileContents(name);
 }
 var RecipeList = Class({init=function(self, filename)
   this._filename = luanet.PathUtils.GetPath(filename);
   if(not this._filename  ){ 
     Console:Message("File " .. filename .. " not found!");
   }
   this.hashes = {}
   this.backhashes = {}
   this.descriptions = {}
   this.hides = {}
   self:LoadList();
 }})
 public void LoadList(){
   var data = readfile(this._filename)   ;
   for(line in data:gmatch("[^\r\n]+") ){    
     var recipe, description, parameter = unpack(StringUtils.split(line, ":"));
     var leftSide, result = recipe:match("^([a-zA-Z0-9_]+[+[a-zA-Z0-9_]+]*)=([a-zA-Z0-9_]+)$")    ;
     
     if(leftSide  &&  result  ){          
       var ingredients = StringUtils.split(leftSide, "+")             ;
       if(#ingredients > 0  ){                 
         table.sort(ingredients)        ;
         this.hashes[leftSide] = result    ;
         this.backhashes[result] = ingredients      ;
       }
       if(description  ){         
         this.descriptions[result] = description;
       }else{
         this.descriptions[result] = "";
       }
       if(parameter  &&  parameter == "hide"  ){ 
         this.hides[result] = true;
       }else{
         this.hides[result] = false;
       }      
     }    
   }
 }
 public void GetResult(...)  {
   var arg = {...}
   table.sort(arg)  ;
   if(#arg > 0  ){ 
     var hash = "";
     for(_, item in pairs(arg) ){
       hash = hash .. item .. "+";
     }
     hash = hash:sub(1, #hash - 1);
     return this.hashes[hash];
   }else{
     return null;
   }
 }
 public void GetIngredients(item){
   if(this.backhashes[item]  &&  #this.backhashes[item] > 0  ){     
     var unicalItems = {}
     for(k, v in pairs(this.backhashes[item]) ){
       var containing = false;
       for(_, v1 in pairs(unicalItems) ){
         if(v1 == v  ){ 
           containing = true;
           break;
         }
       }
       if(not containing  ){ 
         table.insert(unicalItems, v);
       }
     }
     return unicalItems;
   }else{
     return {}
   }
 }
 public void GetResultOnlyComponents(...)  {
   var arg = {...}
   var haveInRecipe = false  ;
   var result = null;
   
   if(#arg > 0  ){         
     for(k, v in pairs(this.backhashes) ){         
       if(#self:GetIngredients(k) == #arg  ){         
         for(_, item in pairs(arg) ){                
           haveInRecipe = false        ;
           for(k1, ingredient in pairs(self:GetIngredients(k)) ){
             if(ingredient == item  ){ 
               haveInRecipe = true;
               break;
             }            
           }
           if(not haveInRecipe  ){ 
             break                  ;
           }
         }      
         if(haveInRecipe  ){         
           result = k;
           break;
         }
       }
     }    
   }
   return result;
 }
 public void GetBackhashResult(result)  {
   return this.backhashes[result];
 }
 public void GetNames(){
   var result = {}
   for(k, _ in pairs(this.backhashes) ){
     table.insert(result, {k, GameController.locale[GameController.database:Get("items", k.."/description")]})
   }
   return result;
 }
 public void GetDescriptions(){
   var result = {}
   for(k, v in pairs(this.descriptions) ){    
     result[k] = GameController.locale[v];
   }
   return result;
 }
 public void GetHides(){
   return this.hides;
 }  
 }}