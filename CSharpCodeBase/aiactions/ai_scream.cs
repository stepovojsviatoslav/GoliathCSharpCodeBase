using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class ai_scream :ainode{
 // Select enemy using ai vision
 public void init(self, entity);
   AINode.init(self, entity);
 }})
 public void Visit(){
   if(this.status == NODE_READY  ){ 
     if(this.entity._screamTrigger  ){ 
       this.entity._screamTrigger = false;
       this.status = NODE_RUNNING;
       this.entity.mover:Stop();
       if(this.entity.mecanim:CheckStateName("action")  ){ 
         this.entity.mecanim:SetFloat("action_type", 3);
         this.entity.mecanim:ForceSetState("action");
       }else{
         this.entity.mecanim:SetFloat("action_type", 3);
         this.entity.mecanim:SetTrigger("action");
       }
       this.isAnimStarted = false;
     }else{
       this.status = NODE_FAILURE;
     }
   }
   
   if(this.status == NODE_RUNNING  ){ 
     this.entity.mover:LookAt(this.entity._screamTarget:GetPosition());
     if(not this.isAnimStarted  ){ 
       this.isAnimStarted = this.entity.mecanim:CheckStateName("Action");
     }
     if(this.isAnimStarted  &&  not this.entity.mecanim:CheckStateName("Action")  ){ 
       this.status = NODE_SUCCESS;
     }
   }
   return this.status;
 }
 }}