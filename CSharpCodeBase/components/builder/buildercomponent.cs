using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class buildercomponent :component{
 public void init(self, entity);
       Component.init(self, entity);
       this.entity.builder = self;
       this.components = this.entity.config:Get("component");
       
       if(this.components  ){ 
         for(key, value in pairs(this.components) ){
           this.entity:AddComponent(require(value));
         }
       }
 }})
 }}