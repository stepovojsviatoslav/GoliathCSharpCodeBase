using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class  && conditional {
 public void init(self, conditionals);
     this.conditionals = conditionals;
     this.returnCode = B_FAILURE;
 }})
 public void Behave(creature){
   result = true;
   for(k, v in pairs(this.conditionals) ){
     if(v:Behave(creature) == B_FAILURE  ){ 
       result = false;
       break;
     }
   }
   this.returnCode = result  &&  B_SUCCESS  ||  B_FAILURE;
   return this.returnCode;
 }
 public void Bool(creature){
   result = true;
   for(k, v in pairs(this.conditionals) ){
     if(v:Behave(creature) == B_FAILURE  ){ 
       result = false;
       break;
     }
   }
   return result;
 }
 }}