using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class scenelight {
 public void init(self, gameObject);
     this.light = gameObject:GetComponent("LightController");
     GameController.eventSystem:AddListener("DAY_PHASE_CHANGED", self);
     this.timeout = GameController.database:Get("components", "SceneLight/colorChangeTime");
 }})
 public void OnEvent(e, data){
   if(e == "DAY_PHASE_CHANGED"  ){ 
     this.light:SetColor(data.color[1], data.color[2], data.color[3],;
       data.ambientColor[1], data.ambientColor[2], data.ambientColor[3], ;
       this.timeout);
   }
 }
 }}