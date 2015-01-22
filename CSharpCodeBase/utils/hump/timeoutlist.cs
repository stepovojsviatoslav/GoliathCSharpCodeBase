using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class timeoutlist {
 public void init (self, globaltimeout);
       this._items = {}
       this._times = {}
       this._globaltimeout = globaltimeout;
 }})
 public void Exists(item){
   var idx = Tables.Find(this._items, item);
   return idx > - 1;
 }
 public void Add(item, timeout){
   timeout = timeout  ||  this._globaltimeout;
   var idx = Tables.Find(this._items, item);
   if(idx > -1  ){ 
     this._times[idx] = os.clock() + timeout;
   }else{
     this._items[#this._items + 1] = item;
     this._times[#this._times + 1] = os.clock() + timeout;
   }
 }
 public void Remove(item){
   var idx = Tables.Find(this._items, item);
   if(idx > -1  ){ 
     table.remove(this._items, idx);
     table.remove(this._times, idx);
   }
 }
 public void SetGlobalTimeout(timeout){
   this._globaltimeout = timeout;
 }
 public void IsEmpty(){
   return #this._items == 0;
 }
 public void Update(){
   if(this._globaltimeout == -1  ){  return }
   for(k, v in pairs(this._times) ){
     if(os.clock() > v  ){ 
       table.remove(this._items, k);
       table.remove(this._times, k);
       break;
     }
   }
 }
 public void GetData(){
   return this._items;
 }
 }}