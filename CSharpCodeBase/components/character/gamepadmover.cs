using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using MainGame.core;


namespace MainGame {
    public class GamepadMover : MonoBehaviour {
        private float angle = 15;
        private float stickDirAngle = 0;
        private float radius = 20;
        private float time = 0;
        private GameObject target;
        private GameObject deniedTarget;
        private GameObject oldTarget;
        private GameObject lineRenderer;
        private GameObject border;
        private bool active = true;
        private Dictionary<float, GameObject> sortedEntityList = new Dictionary<float, GameObject>();


        public void Awake() {
            //FIXME вытащить лайн рендерер из пула 
            LineRenderer mLineRenderer = lineRenderer.GetComponent<LineRenderer>();
            mLineRenderer.SetVertexCount(2);
            mLineRenderer.SetColors(Color.red, Color.red);
            mLineRenderer.SetWidth(0.05f, 0.05f);
        }

        public void DropTarget() {
            this.target = null;
            this.border.SetActive(false);
            this.time = 0;
            this.lineRenderer.SetActive(false);
            this.deniedTarget = null;
        }
        public GameObject GetTarget() {
            return this.target;
        }
        public void SetTarget(GameObject target) {
            this.target = target;
            this.border.SetActive(true);
        }
        //FIXME
        public Dictionary<float, GameObject> GetEntitiesInRadius() {
            var tempEntityList = RaycastUtils.GetEntitiesInRadius(transform.position, this.radius);
            var entityList = new Dictionary<float, GameObject>();
            foreach (var v in tempEntityList) {
                if (v.tag != "Player" && v.tag == "Enemy") {
                    entityList.Add(GetAngle(v.transform.position - transform.position), v);
                }
            }
            //FIX ME SORT
            //entityList.s
            //table.sort(entityList, function(a,b) return a[1]<b[1] })
            return entityList;
        }
        public float GetAngle(Vector3 vec) {
            var mAngle = Math.Atan2(vec.z, vec.x) * 180 / Mathf.PI;
            mAngle = (180 + mAngle) % 360;
            return (float)mAngle;
        }

        //FIXME
        public Dictionary<float, GameObject> GetTargetFromList(float stickDirAngle, float angle) {
            var bound1 = new Vector2(stickDirAngle, stickDirAngle + angle);
            var bound2 = new Vector2(stickDirAngle, stickDirAngle - angle);
            var bound3 = Vector2.zero;
            var bound4 = Vector2.zero;

            if (bound1.y > 360) {
                bound1.x = 0;
                bound1.y = bound1.y - 360;
                bound3.x = 360 - bound1.y;
                bound3.y = 360;
            }

            if (bound2.y < 0) {
                bound2.x = bound2.y + 360;
                bound2.y = 360;
                bound4.x = 0;
                bound4.y = stickDirAngle;
            }

            var bounds = new List<Vector2>();
            bounds.Add(bound1);
            bounds.Add(bound2);
            if (bound3.y > 0) {
                bounds.Add(bound3);
            }
            if (bound4.y > 0) {
                bounds.Add(bound4);
            }

            var entities = new Dictionary<float, GameObject>();
            foreach (var item in bounds) {
                var targets = GetEntitiesBetweenAngles(item);
                foreach (var targetValue in targets) {
                    if (targetValue.visible) {
                        entities.Add(Vector3.Distance(targetValue.transform.position, transform.position), targetValue);
                    }
                }
            }
            return entities;
        }

        //FIXME
        public void Update()  {
       if(this.active  ){ 
         if(GetComponent<GrenadeModeVisualizer>().active){ 
             var rightBumper = GameController.inputService.RightBumperIsPressed();
             this.sortedEntityList = GetEntitiesInRadius();
             if(rightBumper  &&  (Mathf.Abs(GameController.inputService.GetLookValue().x) > 0.4f  ||  Mathf.Abs(GameController.inputService.GetLookValue().z) > 0.4f)  ){ 
               if(this.deniedTarget == null  ){  this.deniedTarget = this.oldTarget }
               this.stickDirAngle = GetAngle(GameController.inputService.GetLookValue());
               var avalibleTargets = GetTargetFromList(this.stickDirAngle, angle);
               if(avalibleTargets.Count == 0  ){ 
                 var temp1 = GetTargetFromList(ConvertAngle(this.stickDirAngle - angle*2), angle);
                 var temp2 = GetTargetFromList(ConvertAngle(this.stickDirAngle + angle*2), angle);
                 foreach(var val in temp1){
                   avalibleTargets.Add(val.Key, val.Value);
                 }
                 foreach(var val in temp2)){
                   avalibleTargets.Add(val.Key, val.Value);
                 }
               }
               if(avalibleTargets.Count > 0  ){ 
                   //FIXME SORT
                 //table.sort(this.avalibleTargets, function(a,b) return a[1]<b[1] })
                 if(this.deniedTarget == avalibleTargets[0].Value ){ 
                   if(avalibleTargets.Count > 1  ){ 
                     this.target = avalibleTargets[1].Value;
                   }
                 }else{
                   this.target =  avalibleTargets[0].Value;
                 }
                 this.border.SetActive(true);
                 this.lineRenderer.SetActive(true);
               }
             }else{
               this.deniedTarget = null;
             }
           if(this.target  ){ 
             border.transform.position = target.transform.position;
             //FIXME
             //RigidbodyUtils.SetPointPositionToLineRenderer(this.lineRendererComponent, this.target:GetPosition().x,this.target:GetPosition().y + 0.5,this.target:GetPosition().z, 0);
             //RigidbodyUtils.SetPointPositionToLineRenderer(this.lineRendererComponent, this.entity:GetPosition().x,this.entity:GetPosition().y + 0.5,this.entity:GetPosition().z, 1);
           }
        
           if((this.target  &&  this.target.visible == false)  ||  GameController.inputService.RightStickButtonWasPressed()  ||  (this.target  &&  this.target.isDeath)){ 
             this.target = null;
             this.border.SetActive(false);
             this.time = 0;
             this.lineRenderer.SetActive(false);
             this.deniedTarget = null;
           }
       
           if(this.target  &&  GameController.inputService.GetLookValue().x ==0  &&  GameController.inputService.GetLookValue().z ==0  ){ 
             if(this.time > 1  ){ 
               this.time = 0;
               this.lineRenderer.SetActive(false);
             }
             this.time = this.time + Time.deltaTime;
           }
           this.oldTarget = this.target;
         }
       }
    }

        public List<GameObject> GetEntitiesBetweenAngles(Vector2 vec2) {
            var entities = new List<GameObject>();
            foreach (var v in sortedEntityList) {
                if (vec2.x > vec2.y) {
                    if (v.Key >= vec2.y && v.Key <= vec2.x) {
                        entities.Add(v.Value);
                    }
                } else {
                    if (v.Key >= vec2.x && v.Key <= vec2.y) {
                        entities.Add(v.Value);
                    }
                }
            }
            return entities;
        }
        public float ConvertAngle(float angle) {
            var mAngle = angle;
            if (mAngle < 0) {
                mAngle = 360 + mAngle;
            } else if (mAngle > 360) {
                mAngle = mAngle - 360;
            }
            return mAngle;
        }
        public void Disable() {
            this.target = null;
            this.border.SetActive(false);
            this.time = 0;
            this.lineRenderer.SetActive(false);
            this.deniedTarget = null;
            this.active = false;
        }
        public void Enable() {
            this.active = true;
        }
    }
}