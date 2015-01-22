using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class dialoguesystem {
 public void init(self);
   this.says = {}
   this.answers = {}
   this.dialogueUnity = luanet.GameFacade.dialogueSystem;
   this.dialogueUnity:SetAnswerCallback(function (id){
       self:OnAnswerCallback(id);
   })
 }})
 public void LoadDialogue(dialogue){
   self:ResetBuffer();
   this.current = coroutine.create(require(dialogue));
   this.isRunning = false;
 }
 public void Show(dialogue){
   self:LoadDialogue(dialogue);
   self:Step();
   this.dialogueUnity:Show(this.says[1], this.answers[1], this.answers[2], this.answers[3], this.answers[4]);
   luanet.TimeUtils.SetTimeScale(0);
   GameController.inputService:PushFrame("dialogue");
 }
 public void ResetBuffer(){
   this.says = {}
   this.answers = {}
 }
 public void Say(phrase){
   this.says[#this.says + 1] = phrase;
 }
 public void Answer(phrase){
   this.answers[#this.answers + 1] = phrase;
 }
 public void OnAnswerCallback(id){
   self:Step(id);
   if(not self:IsFinished()  ){ 
     this.dialogueUnity:Show(this.says[1], this.answers[1], this.answers[2], this.answers[3], this.answers[4]);
   }else{
     this.dialogueUnity:Hide();
     GameController.inputService:PopFrame();
     luanet.TimeUtils.SetTimeScale(1);
     this.current = null;
   }
 }
 public void Update(){
   if(this.current != null  ){ 
     var lookVector = GameController.inputService:GetLookValue("dialogue");
     lookVector.x = 0;
     if(lookVector:Length() > 0.3  ){ 
       if(not this.switch  ){ 
         if(lookVector.z > 0  ){ 
           this.dialogueUnity:SelectDown();
         }else{  
           this.dialogueUnity:SelectUp();
         }
       }
       this.switch = true;
     }else{
       this.switch = false;
     }
     
     if(GameController.inputService:LeftButtonWasPressed("dialogue")  ){ 
       this.dialogueUnity:JoysticApprove();
     }
   }
 }
 public void Step(data){
   self:ResetBuffer();
   if(this.current != null  ){ 
     if(not this.isRunning  ){ 
       coroutine.resume(this.current, self) ;
       this.isRunning = true;
     }else{
       coroutine.resume(this.current, data) ;
     }
     return coroutine.status(this.current) != "dead";
   }else{
     return false;
   }
 }
 public void IsFinished(){
   return this.current == null  ||  coroutine.status(this.current) == "dead";
 }
 }}