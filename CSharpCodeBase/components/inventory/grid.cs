using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class grid {
 var InventoryGrid = Class({init=function(self, count, name)
       this._count = count  ||  4;
       this._grid = {}
       this._name = name;
       for(i = 1, this._count ){
         this._grid[i] = null;
       }
 }})
 public void InventorySetItem(index, item){
   this._grid[index] = item;
 }
 public void InventoryGetItem(index){
   return this._grid[index];
 }
 public void InventoryGetItemCount(item){
   var sum = 0;
   for(k, v in pairs(this._grid) ){
     if(v.name == item  ){ 
       sum = sum + v.count;
     }
   }
   return sum;
 }
 // Return count of removed elements
 public void InventoryRemoveFromSlot(idx, count){
   var slotCount = this._grid[idx].count;
   var returnCount = 0;
   slotCount = slotCount - count;
   if(slotCount < 0  ){ 
     returnCount = -slotCount;
     slotCount = 0;
   }
   this._grid[idx].count = slotCount;
   GameController.eventSystem:Event("INVENTORY_SLOT_CHANGED_CORE", {this._name, idx, this._grid[idx].name, this._grid[idx].count})
   return returnCount;
 }
 public void InventoryRemoveItem(item, count){
   var rest = count;
   for(i = 1, this._count ){
     if(this._grid[i] != null  &&  this._grid[i].name == item  ){ 
       if(rest > this._grid[i].count  ){ 
         rest = rest - this._grid[i].count;
         this._grid[i].count = 0;
         GameController.eventSystem:Event("INVENTORY_SLOT_CHANGED_CORE", {this._name, i, this._grid[i].name, this._grid[i].count})
       }else{
         this._grid[i].count = this._grid[i].count - rest;
         rest = 0;
         GameController.eventSystem:Event("INVENTORY_SLOT_CHANGED_CORE", {this._name, i, this._grid[i].name, this._grid[i].count})
       }
     }
     if(rest == 0  ){ 
       break;
     }
   }
   return rest;
 }
 public void InventoryGetItems(){
   return this._grid;
 }
 public void InventoryGetFilteredItems(tag){
   var tempTable = {}
   for(k, v in pairs(this._grid) ){ 
     if(v.name  &&  GameController.database:Get("items", v.name .."/tag") == tag  &&  v.count > 0)  ){ 
       table.insert(tempTable, v);
     }
   }
   return tempTable;
 }
 // Return rest item (if count is 0,  ){  item added)
 public void InventoryAddItem(newItem){
   // try to find item,  &&  add as counter
   var addItem = newItem:Clone();
   var stackLimit = GameController.database:Get("items", addItem.name .. "/stackLimit");
   for(i = 1, this._count ){
     var item = this._grid[i];
     if(item != null  &&  item.name == addItem.name  &&  item.count > 0  ){ 
       var restItemsToStack = stackLimit - item.count;
       if(restItemsToStack >= addItem.count  ){ 
         item.count = item.count + addItem.count;
         GameController.eventSystem:Event("INVENTORY_SLOT_CHANGED_CORE", {this._name, i, item.name, item.count})
         addItem.count = 0;
         break;
       }else{
         addItem.count = addItem.count - restItemsToStack;
         item.count = item.count + restItemsToStack;
         GameController.eventSystem:Event("INVENTORY_SLOT_CHANGED_CORE", {this._name, i, item.name, item.count})
       }
     }
   }
   
   // try to find empty place,  &&  insert item
   for(i = 1, this._count ){
     if(this._grid[i] == null  ||  this._grid[i].count == 0  ){ 
       if(this._grid[i] == null  ){ 
         this._grid[i] = InventoryItem(nil, 0);
       }
       this._grid[i].name = addItem.name;
       if(addItem.count > stackLimit  ){ 
         this._grid[i].count = stackLimit;
       }else{
         this._grid[i].count = addItem.count;
       }
       addItem.count = addItem.count - this._grid[i].count;
       GameController.eventSystem:Event("INVENTORY_SLOT_CHANGED_CORE", {this._name, i, this._grid[i].name, this._grid[i].count})
       if(addItem.count == 0  ){ 
         break;
       }
     }
   }
   
   // Have no place
   return addItem;
 }
 return InventoryGrid}}