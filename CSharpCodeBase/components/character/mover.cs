using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using MainGame.core;
using MainGame;


namespace MainGame {
    public class Mover : MonoBehaviour {
        private Vector3 input;
        private UnityEntity target;
        private float speed;
        private bool _isStop = false;
        private int moveType = 0;
        private bool autoLook = true;
        private float speedCurve;
        private float targetDistance;
        private bool _useLoa;
        private bool _useLoaDefault;
        private UnityEntity entity;
        private float _epsilon;
        private float _latestMoveSpeed;
        private int _latestMoveType;

        //FIXME
        public void Awake() {
            this.entity = GetComponent<UnityEntity>();
        }


        /*public void init (self, entity);
              Component.init(self, entity);
              this.entity.mover = self;
              this.input = Vector3();
              this.speed = this.entity.config:Get("moverSpeed");
              this.target = null;
              this._isStop = false;
              this._epsilon = this.entity.config:Get("moverEpsilon");
              this.moveType = 0;
              this.newDirection = Vector3(0,0,0);
              this._useLoaDefault = this.entity.config:Get("moverLoa");
              this._useLoa = this._useLoaDefault;
              this.raycastSphereRadius = this.entity.config:Get("raycastSphereRadius");
              this.raycastForwardDistance = this.entity.config:Get("raycastForwardDistance");
              this.raycastSphericalDistance = this.entity.config:Get("raycastSphericalDistance");
              this.offsetAngle = this.entity.config:Get("offsetAngle");
              this.speedCurve = null;
              this.autoLook = true;
       
              this.count = 0;
              this.countmax = 3;
        }})
        */

        //FIXME
        public void ResetSpeed() {
            this.speed = 5;//this.entity.config:Get("moverSpeed");
        }

        public void OnChangeVisibility(bool state) {
            Stop();
        }

        public void SetCurve(float curve) {
            this.speedCurve = curve;
        }
        public void SetInput(Vector3 vec3, int moveType = 0, bool ignoreLoa = true) {
            if (ignoreLoa) {
                this._useLoa = false;
            } else {
                this._useLoa = this._useLoaDefault;
            }
            this.target = null;
            this.input.x = vec3.x;
            this.input.z = vec3.z;

            if (this.input.magnitude > 1) {
                this.input.Normalize();
            }
            CheckStop();
            this.moveType = moveType;
        }
        public void Stop() {
            this.SetInput(Vector3.zero);
            //FIXME MOVE
            //RigidbodyUtils.Move(this.entity.rigidbody, this.input * this.speed * GameController.gameTime);
        }

        //FIXME
        public void CheckStop() {
            var inputLength = this.input.magnitude * this.speed;
            if (!this._isStop && inputLength == 0) {
                //this.newDirection = Vector3();
                this._isStop = true;
                //RigidbodyUtils.Move(this.entity.rigidbody, Vector3());
                //RigidbodyUtils.ResetVelocity(this.entity.rigidbody)
            } else if (this._isStop && inputLength != 0) {
                this._isStop = false;
            }
        }


        public void SetSpeed(float speed) {
            this.speed = speed;
        }

        public void SetInputToVec(Vector3 vec3, UnityEntity stayTarget = null, int moveType = 0, bool ignoreLoa = true) {
            if (ignoreLoa) {
                this._useLoa = false;
            } else {
                this._useLoa = this._useLoaDefault;
            }
            if (!stayTarget) { this.target = null; }
            var dv = vec3 - this.entity.GetPosition();
            this.input.x = dv.x;
            this.input.z = dv.z;
            if (this.input.magnitude > 1) {
                this.input.Normalize();
            }
            CheckStop();
            this.moveType = moveType;
        }

        public void SetInputFromVec(Vector3 vec3, UnityEntity stayTarget = null, int moveType = 0, bool ignoreLoa = true) {
            if (ignoreLoa) {
                this._useLoa = false;
            } else {
                this._useLoa = this._useLoaDefault;
            }
            if (!stayTarget) { this.target = null; }
            var dv = vec3 - this.entity.GetPosition();
            this.input.x = -dv.x;
            this.input.z = -dv.z;
            if (this.input.magnitude > 1) {
                this.input.Normalize();
            }
            CheckStop();
            this.moveType = moveType;
        }

        //FIXME
        public void LookAt(Vector3 vec3) {
            //RigidbodyUtils.LookAt(this.entity.rigidbody, vec3);
        }

        public void SetGoal(UnityEntity target, int moveType = 0, float distance = 0) {
            this.target = target;
            this.targetDistance = distance;
            this.moveType = moveType;
        }
        public bool IsHaveGoal() {
            return this.target != null;
        }
        public bool IsReachDestination(Vector3 vec3) {
            var localPos = this.entity.GetPosition();
            var v1 = new Vector2(localPos.x, localPos.z);
            var v2 = new Vector2(vec3.x, vec3.z);
            return Vector2.Distance(v1, v2) < this._epsilon;
        }

        //FIXME
        public void Update(){
          if(this.target != null  ){ 
            // check if(we reach target
            var pos = this.target.GetPosition();
            //print(this.entity:GetEffectiveDistance(this.entity, this.target))
            if(pos != null && (this.entity.GetEffectiveDistance(this.target) < this.targetDistance  ||  !this.target.IsInteractable){
              this.target = null;
              Stop();
            }else if( pos != null && IsReachDestination(pos)){ 
              this.target = null;
              Stop();
            }else{
              SetInputToVec(pos);
            }
          }
          var moveSpeed = (this.input * this.speed).magnitude;
          if(this._latestMoveSpeed != moveSpeed  ){ 
            //this.entity.mecanim:SetFloat("move_speed", moveSpeed);
            this._latestMoveSpeed = moveSpeed;
          }
          if(this._latestMoveType != this.moveType  ){ 
           // this.entity.mecanim:SetFloat("move_type", this.moveType);
            this._latestMoveType = this.moveType;
          }
        }

        //FIXME
        public void FixedUpdate() {
            if (!this._isStop) {
                if (this._useLoa == true) {
                    /* var result = RigidbodyUtils.AvoidMove(this.entity.gameObject, ;
                                                             this.input, ;
                                                             this.newDirection, ;
                                                             this.raycastSphereRadius , ;
                                                             this.raycastForwardDistance, ;
                                                             this.raycastSphericalDistance ,;
                                                             this.offsetAngle);
                   this.input = Vector3(result.x1, 0, result.z1);
                   this.newDirection = Vector3(result.x2, 0, result.z2);
                   }*/
                }
                if (this.speedCurve != null) {
                    //this.input = this.input * this.entity.mecanim:GetFloat("speed_curve");
                }
                //if(this.newDirection  &&  this.newDirection.x != 0  &&  this.newDirection.y !=0  &&  this.newDirection.z !=0  ){ 
                //  this.input = this.newDirection;
                //}
            }
            if (this.input.magnitude > 0) {
                if (this.autoLook) {
                    //RigidbodyUtils.Move(this.entity.rigidbody, this.input * this.speed, this._useLoa  &&  5  ||  10);
                } else {
                    //if(GameController.inputService:IsGamepad() == false  ){ 
                    //  RigidbodyUtils.MoveLookAtMouse(this.entity.rigidbody, this.input * this.speed, this.entity.height / 3);
                    // }else{
                    //  if(this.entity.gamepadRightStickController  &&   this.entity.gamepadRightStickController:GetTarget()  ){ 
                    //    RigidbodyUtils.MoveNotRotate(this.entity.rigidbody, this.input * this.speed);
                    //    self:LookAt((this.entity.gamepadRightStickController:GetTarget():GetPosition()));
                    //  }else{
                    //    RigidbodyUtils.Move(this.entity.rigidbody, this.input * this.speed);
                    //  }
                    // }
                }
            }
        }
    }
}