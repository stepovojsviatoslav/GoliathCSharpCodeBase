using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class ainode {
 public void init (self, entity, childNodes){
       this.childNodes = childNodes;
       this.entity = entity;
       this.status = NODE_READY;
       this.lastStatus = NODE_READY;
       this.parent = null;
       if(this.childNodes != null  ){ 
         for(k, v in pairs(this.childNodes) ){
           v.parent = self;
         }
       }
       this._lockstart = 0;
       this._locktime = 0;
 }})
 public void Visit(){
   this.status = NODE_FAILURE;
 }
 public void Save(){
   this.lastStatus = this.status;
   if(this.childNodes != null  ){ 
     for(k, v in pairs(this.childNodes) ){
       v:Save();
     }
   }
 }
 public void Process(){
   if(this.status != NODE_RUNNING  ){ 
     self:Reset();
   }else{if this.childNodes != null  ){ 
     for(k, v in pairs(this.childNodes) ){
       v:Process();
     }
   }
 }
 public void Reset(){
   if(this.status != NODE_READY  ){ 
     this.status = NODE_READY;
     if(this.childNodes != null  ){ 
       for(k, v in pairs(this.childNodes) ){
         v:Reset();
       }
     }
   }
 }
 public void Sleep(seconds){
   this._lockstart = os.clock();
   this._locktime = seconds;
 }
 public void IsSleeping(){
   return os.clock() - this._lockstart < this._locktime;
 }
 public void OnEvent(data){
   if(this.childNodes != null  ){ 
     for(k, v in pairs(this.childNodes) ){
       if(v.status == NODE_RUNNING  ){ 
         v:OnEvent(data);
       }
     }
   }  
 }
 }}