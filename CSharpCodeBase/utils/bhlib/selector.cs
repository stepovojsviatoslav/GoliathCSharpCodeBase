using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class selector {
 public void init(self, behaviours);
       this._behaviours = behaviours;
       this.returnCode = B_FAILURE;
 }})
 public void Behave(creature){
   for(i = 1, #this._behaviours ){
     code = this._behaviours[i]:Behave(creature);
     if(code == B_FAILURE  ){ 
       // just continue
     }else{if code == B_SUCCESS  ||  code == B_RUNNING  ){ 
       this.returnCode = code;
       return this.returnCode;
     }
   }
   this.returnCode = B_FAILURE;
   return this.returnCode;
 }
 }}