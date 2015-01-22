using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class gameloop {
 public void init (self, gameObject);
       this.items = {}
       this.affectQueue = {}
 }})
 public void FixedUpdate(){
   for(k, v in pairs(this.items) ){
     if(v.enabled  ){ 
       v:FixedUpdate();
     }
   }
   self:ProcessAffectQueue();
 }
 public void LateUpdate(){
   for(k, v in pairs(this.items) ){
     if(v.enabled  ){ 
       v:LateUpdate();
     }
   }
   self:ProcessAffectQueue();
 }
 public void Update(){
   //local updateCount = 0
   for(k, v in pairs(this.items) ){
     if(v.enabled  ){ 
       //updateCount = updateCount + 1
       v:Update();
     }
   }    
   self:ProcessAffectQueue();
   //print(updateCount)
 }
 public void Add(item){
   table.insert(this.affectQueue, {item, 1})
   //this.affectQueue[#this.affectQueue + 1] = {item, 1}
 }
 public void AddForce(item){
   var idx = Tables.Find(this.items, item);
   if(idx < 0  ){ 
     table.insert(this.items, item);
     //this.items[#this.items + 1] = item
   }
 }
 public void Remove(item){
   item.enabled = false;
   this.affectQueue[#this.affectQueue + 1] = {item, 0}
 }
 public void ProcessAffectQueue(){
   if(#this.affectQueue > 0  ){ 
     for(k, v in pairs(this.affectQueue) ){
       if(v[2] == 0  ){ 
         self:RemoveForce(v[1]);
       }else{
         self:AddForce(v[1]);
       }
     }
     this.affectQueue = {}
   }
 }
 public void RemoveForce(item){
   var idx = Tables.Find(this.items, item);
   if(idx > -1  ){ 
     table.remove(this.items, idx);
   }
 }
 }}