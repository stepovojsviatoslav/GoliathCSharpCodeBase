using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using MainGame.core;
//using UnityEngine.EventSystems;
 
 namespace MainGame
 {
    public class Camera : UnityEntity
    {
        protected GameObject targetEntity = null;
        protected float minHeight = 6;
        protected float minDistance = 12;
        protected float maxHeight = 30;
        protected float maxDistance = 40;
        protected float targetHeight = 1;
        protected float currentZoom = 0.5f;
        protected float zoomSoftLimit = 0.05f;
        protected float lerpZoomSpeed = 5;
        protected float lerpLimitSpeed = 20;
        protected float angle = 0;
        protected float currentAngle = 0;
        protected float scrollSpeed = 3;
        protected float heightValue;
        protected float distanceValue;

        private float zoom;

        public void Awake()
        {            
            zoom = currentZoom;
        }         
         
        public void LoadForEntity(GameObject target)
        {
            targetHeight = target.GetComponent<UnityEntity>().height / 2;
            //minHeight = target.GetComponent<Config>().Get("cameraMinHeight")  ||  6;
            //minDistance = target.GetComponent<Config>().Get("cameraMinDistance")  ||  12;
            //maxHeight = target.GetComponent<Config>().Get("cameraMaxHeight")  ||  30;
            //maxDistance = target.GetComponent<Config>().Get("cameraMaxDistance")  ||  40 ;
        }

        public void SetTargetEntity(GameObject entity)
        {
            targetEntity = entity;
        }

        public void Update()
        {
            //var isGamepad = GameController.inputService:IsGamepad();
            //var lookValue = GameController.inputService:GetLookValue();
            int scroll = 0;
            //if(isGamepad  &&  GameController.inputService:RightBumperIsPressed())
            //{ 
                //if(math.abs(lookValue.z) > 0.5)
                //{ 
                    //scroll = lookValue.z;
                //}
            //}else
            //{
                //scroll = Input.GetMouseScrollValue();
            //}
            if(scroll != 0)
            { 
                int sign = scroll > 0  ? 1  : -1;
                zoom = zoom - sign * scrollSpeed * Time.deltaTime;
                zoom = Mathf.Clamp(0 - zoomSoftLimit, zoom, 1 + zoomSoftLimit);
            }
            //if(isGamepad)
            //{ 
                //if(GameController.inputService:RightBumperIsPressed())
                //{ 
                    //var val = lookValue.x;
                    //if(math.abs(val) > 0.5)
                    //{ 
                        //if(val < 0  &&  this.isGamepadRotateBlocked == null)
                        //{ 
                            //this.currentAngle = this.currentAngle - math.rad(90);
                        //}else
                        //{
                            //if this.isGamepadRotateBlocked == null)
                            //{ 
                                //this.currentAngle = this.currentAngle + math.rad(90);
                            //}
                            //this.isGamepadRotateBlocked = true;
                        //}
                    //}
                    //else{
                        //this.isGamepadRotateBlocked = null;
                    //}
                //}
            //}
            //else
            //{
                //if(Input.GetKeyDown(KeyCode.Z))
                //{ 
                    //this.currentAngle = this.currentAngle - math.rad(90);
                //}
                //else
                //{
                    //if Input.GetKeyDown(KeyCode.X))
                    //{ 
                        //this.currentAngle = this.currentAngle + math.rad(90);
                    //}
                //}
             //}
   
            angle = Mathf.Lerp(angle, currentAngle, Time.deltaTime * lerpLimitSpeed);
        }

        public void FixedUpdate()
        {
        }

        public void LateUpdate()
        {
             if (targetEntity != null)
             {   
                //Align zoom
                if (zoom < 0)
                {  
                    zoom = Mathf.Lerp(zoom, 0, Time.deltaTime * lerpLimitSpeed);
                }
                else
                {
                    if (zoom > 1)
                    { 
                        zoom = Mathf.Lerp(zoom, 1, Time.deltaTime * lerpLimitSpeed);
                    }
                    currentZoom = Mathf.Lerp(currentZoom, zoom, lerpZoomSpeed * Time.deltaTime);
     
                    //Calculate current parameters
                    float heightValue = Mathf.Lerp(minHeight, maxHeight, currentZoom);
                    float distanceValue = Mathf.Lerp(minDistance, maxDistance, currentZoom);
                    if (heightValue == null)
                    {
                        this.heightValue = heightValue;
                        this.distanceValue = distanceValue;
                    }
                    //heightValue = math.lerp(this.heightValue, heightValue, GameController.deltaTime * 50)
                    //distanceValue = math.lerp(this.distanceValue, distanceValue, GameController.deltaTime * 50)
                    this.heightValue = heightValue;
                    this.distanceValue = distanceValue;
                    Vector3 offsetVector = new Vector3(0, heightValue, -distanceValue);
                    //offsetVector:RotateAroundY(this.angle);
     
                    //Setup position  &&  rotation
                    Vector3 position = targetEntity.transform.position + offsetVector;
                    SetPosition(position);
     
                    //Setup rotation
                    offsetVector.y = offsetVector.y - targetHeight;
                    Vector3 rotation = /*RotationUtils.LookRotation(offsetVector * -1)*/;
                    self:SetRotation(rotation);
                }
             }
         }
    }
 }