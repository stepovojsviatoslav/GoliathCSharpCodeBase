using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class fragilethingcomponent :component{
 public void init(self, entity);
       Component.init(self, entity);
 }})
 public void OnCollisionEnter(targetEntity){
   if(targetEntity.characterClass != null  &&  targetEntity.characterClass > 1  ){ 
     this.entity:OnFragile(targetEntity);
   }
 }
 }}