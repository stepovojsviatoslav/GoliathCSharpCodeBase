using System;
 
 using System.Collections.Generic;
 
 using System.Linq;
 
 using System.Text;
 
 using UnityEngine;
 
 using System.Collections;
 
 using MainGame.core;
 
 using UnityEngine.EventSystems;
 
 namespace MainGame{
 public class mockupentity :unityexistsentity{
 public void init (self, gameObject);
     UnityExistsEntity.init(self, gameObject);
     this.count = 0;
     this.oldCount = 0;
     this.enable = false;
     this.lerpEnd = true;
     this.luaMapper:SetAlwaysVisible(true);
     this.oldPosition = Input.RaycastMouseOnTerrain();
     Transform.SetMaterialColor(this.gameObject, 0, 0.5, 0, 0.5);
 }})
 public void Enable(){
   this.enable = true;
 }
 public void Disable(){
   this.enable = false;
   self:Destroy();
 }
 public void Update(){
   UnityExistsEntity.Update(self);
   
   if(this.enable == true  ){ 
     this.position = Input.RaycastMouseOnTerrain();
     if(math.abs(this.oldPosition.x - this.position.x) > 0.05  ||  math.abs(this.oldPosition.z - this.position.z) > 0.05  ){ 
       Transform.Lerp(this.gameObject, this.position.x, this.position.y, this.position.z);
       this.lerpEnd = false;
     }else{if this.lerpEnd == false  ){ 
       if(math.abs(this.oldPosition.x - this.gameObject.transform.position.x)<0.1  &&  math.abs(this.gameObject.transform.position.z - this.oldPosition.z)<0.1  ){ 
         this.lerpEnd = true;
       }else{
         Transform.Lerp(this.gameObject, this.oldPosition.x, this.oldPosition.y, this.oldPosition.z) ;
       }
     }
     this.oldPosition = this.position;
   }
   if(this.oldCount != this.count  ){ 
       this.oldCount = this.count;
       if(this.count == 0  ){ 
         Transform.SetMaterialColor(this.gameObject, 0, 0.5, 0, 0.5);
       }else{
         Transform.SetMaterialColor(this.gameObject, 0.5, 0, 0, 0.5);
       }
   }
 }
 public void Rotate(angle){
   RigidbodyUtils.Rotate(this.gameObject, angle);
 }
 public void MoveUpTransform(speed){
   RigidbodyUtils.MoveTransform(this.gameObject, speed, 0);
 }
 public void GetPosition(){
   return Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
 }
 public void MoveRightTransform(speed){
   RigidbodyUtils.MoveTransform(this.gameObject, 0, speed);
 }
 public void OnCollisionEnter(targetEntity){
     this.count = this.count + 1;
 }
 public void OnCollisionExit(targetEntity){
     this.count = this.count - 1;
 }
 }}