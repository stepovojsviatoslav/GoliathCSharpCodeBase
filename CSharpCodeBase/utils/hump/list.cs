using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class list {
 public void init(self);
   this._array = {}  
 }})
 public void Add(item){
   this._array[#this._array + 1] = item;
 }
 public void Remove(item){
   var idx = Tables.Find(this._array, item);
   if(idx > - 1  ){ 
     table.remove(this._array, idx);
   }
 }
 public void Clear(){
   this._array = {}
 }
 public void Contains(item){
   return Tables.Find(this._array, item) > -1;
 }
 public void __len(){
   return #this._array;
 }
 public void GetTable(){
   return this._array;
 }
 public void IsEmpty(){
   return #this._array == 0;
 }
 }}