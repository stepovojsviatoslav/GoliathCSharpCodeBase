using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class sequence {
 public void init(self, behaviours);
       this._behaviours = behaviours;
       this.returnCode = B_FAILURE;
 }})
 public void Behave(creature){
   
   anyRunning = false;
   for(i = 1, #this._behaviours ){
     code = this._behaviours[i]:Behave(creature);
     if(code == B_FAILURE  ){ 
       this.returnCode = code;
       return this.returnCode;
     }else{if code == B_SUCCESS  ){ 
       // continue
     }else{if code == B_RUNNING  ){ 
       anyRunning = true;
       // continue
     }
   }
   this.returnCode = anyRunning  &&  B_RUNNING  ||  B_SUCCESS;
   return this.returnCode;
 }
 }}