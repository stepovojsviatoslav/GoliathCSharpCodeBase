using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class stack {
 public void init(self, maxsize);
   this.maxsize = maxsize  ||  100;
   this.top = 0;
   this.elems = {}
 }})
 public void Push(elem){
   if(this.top == this.maxsize  ){ 
     return false;
   }else{
     table.insert(this.elems, elem);
     this.top = this.top + 1;
     return true;
   }
 }
 public void Pop(){
   this.top = this.top - 1;
   return table.remove(this.elems);
 }
 public void Top(){
   return this.elems[this.top];
 }
 public void Size(){
   return this.top;
 }
 public void Dump()  {
   result = "Stack dump: ";
   for(k, v in pairs(this.elems) ){
     result = result .. v;
     result = result .. " ";
   }
   Console:Message(result);
 }
 public void FollowForTop(elem){
   if(this.top == 0  ){ 
     Console:Message("Undefined behaviour! Stack FollowForTop");
     return false;
   }else{
     table.insert(this.elems, this.top, elem);
     this.top = this.top + 1;
     return true;
   }  
 }
 public void ReplaceNext(elem){
   if(this.top > 1  ){ 
     table.insert(this.elems, this.top, elem);
     return table.remove(this.elems, this.top - 1);
   }else{
     Console:Message ("Undefined behavior! Stack ReplaceNext");
     return null;
   }
 }
 public void RemoveNext(){
   if(this.top > 1  ){ 
     this.top = this.top - 1;
     return table.remove(this.elems, this.top);
   }else{
     Console:Message ("Undefined behaviour! Stack RemoveNext");
     return null;
   }
 }
 }}