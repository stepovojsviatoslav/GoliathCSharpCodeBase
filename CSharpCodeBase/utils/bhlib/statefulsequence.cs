using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class statefulsequence {
 public void init(self, behaviours);
       this._behaviours = behaviours;
       this.returnCode = B_FAILURE;
       this.lastBehaviour = 1;
 }})
 public void Behave(creature){
   while(this.lastBehaviour <= #this._behaviours ){
     code = this._behaviours[this.lastBehaviour]:Behave(creature);
     if(code == B_FAILURE  ){ 
       this.lastBehaviour = 1;
       this.returnCode = code;
       return this.returnCode;
     }else{if code == B_SUCCESS  ){ 
       // continue
     }else{if code == B_RUNNING  ){ 
       this.returnCode = code;
       return this.returnCode;
     }
     this.lastBehaviour = this.lastBehaviour + 1;
   }
   
   this.lastBehaviour = 1;
   this.returnCode = B_SUCCESS;
   return this.returnCode;
 }
 }}