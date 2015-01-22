using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class inventory {
 public void init(self);
       this.grids = {}
       this.grids.inventory = InventoryGrid(28, "inventory");
       //this.grids.gadgets = InventoryGrid(4, "gadgets")
       this.grids.equipment = InventoryGrid(5, "equipment");
 }})
 public void AddItem(item, count){
   if(count != null  ){ 
     item = InventoryItem(item, count);
   }
   var restItem = this.grids.AddItem(item);
   if(restItem.count > 0  ){ 
     restItem = this.grids.gadgets:AddItem(restItem);
   }
   return restItem;
 }
 public void GetItemCount(item){
   var sum = 0;
   for(k, v in pairs(this.grids) ){
     sum = sum + v:GetItemCount(item);
   }
   return sum;
 }
 public void SetSlotFromUI(container, idx, item, count){
   this.grids[container]:SetItem(idx, InventoryItem(item, count));
 }
 public void GetSlotCount(container, idx){
   return this.grids[container]:GetItem(idx).count;
 }
 public void RemoveFromSlot(container, idx, count){
   return this.grids[container]:RemoveFromSlot(idx, count);
 }
 public void RemoveItems(item, count){
   var rest = count;
   for(k, v in pairs(this.grids) ){
     rest = v:RemoveItem(item, rest);
     if(rest == 0  ){ 
       break;
     }
   }
 }
 }}