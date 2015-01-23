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
        private GameObject target;
        private float speed;
        private bool _isStop = false;
        private int moveType = 0;
        private bool autoLook = true;
        private float speedCurve;
        private float targetDistance;
        private bool _useLoa;
        private bool _useLoaDefault;
        private float _epsilon;
        private float _latestMoveSpeed;
        private int _latestMoveType;
        private Vector3 newDirection = Vector3.zero;
        private float raycastSphereRadius;
        private float raycastForwardDistance;
        private float raycastSphericalDistance;
        private float offsetAngle;

        //FIXME
        public void Awake() {
            this.raycastSphereRadius = GetComponent<config>().Get("raycastSphereRadius");
            this.raycastForwardDistance = GetComponent<config>().("raycastForwardDistance");
            this.raycastSphericalDistance = GetComponent<config>().("raycastSphericalDistance");
            this.offsetAngle = GetComponent<config>().("offsetAngle");
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
            RigidbodyUtils.Move(gameObject.GetComponent<Rigidbody>(), this.input * this.speed * GameController.gameTime);
        }

        //FIXME
        public void CheckStop() {
            var inputLength = this.input.magnitude * this.speed;
            if (!this._isStop && inputLength == 0) {
                this.newDirection = Vector3.zero;
                this._isStop = true;
                RigidbodyUtils.Move(GetComponent<Rigidbody>(),Vector3.zero);
                RigidbodyUtils.ResetVelocity(GetComponent<Rigidbody>());
            } else if (this._isStop && inputLength != 0) {
                this._isStop = false;
            }
        }


        public void SetSpeed(float speed) {
            this.speed = speed;
        }

        public void SetInputToVec(Vector3 vec3, GameObject stayTarget = null, int moveType = 0, bool ignoreLoa = true) {
            if (ignoreLoa) {
                this._useLoa = false;
            } else {
                this._useLoa = this._useLoaDefault;
            }
            if (stayTarget!=null) { this.target = null; }
            var dv = vec3 - gameObject.transform.position;
            this.input.x = dv.x;
            this.input.z = dv.z;
            if (this.input.magnitude > 1) {
                this.input.Normalize();
            }
            CheckStop();
            this.moveType = moveType;
        }

        public void SetInputFromVec(Vector3 vec3, GameObject stayTarget = null, int moveType = 0, bool ignoreLoa = true) {
            if (ignoreLoa) {
                this._useLoa = false;
            } else {
                this._useLoa = this._useLoaDefault;
            }
            if (stayTarget != null) { this.target = null; }
            var dv = vec3 - gameObject.transform.position;
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
            RigidbodyUtils.LookAt(gameObject.GetComponent<Rigidbody>(), vec3);
        }

        public void SetGoal(GameObject target, int moveType = 0, float distance = 0) {
            this.target = target;
            this.targetDistance = distance;
            this.moveType = moveType;
        }
        public bool IsHaveGoal() {
            return this.target != null;
        }
        public bool IsReachDestination(Vector3 vec3) {
            var localPos = gameObject.transform.position;
            var v1 = new Vector2(localPos.x, localPos.z);
            var v2 = new Vector2(vec3.x, vec3.z);
            return Vector2.Distance(v1, v2) < this._epsilon;
        }

        //FIXME
        public void Update(){
          if(this.target != null  ){ 
            // check if(we reach target
            var pos = gameObject.transform.position;
            //print(this.entity:GetEffectiveDistance(this.entity, this.target))
            if(pos != null && (this.GetComponent<UnityEntity>().GetEffectiveDistance(this.target) < this.targetDistance  ||  !this.target.GetComponent<UnityEntity>().IsInteractable)){
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
            mecanim.SetFloat("move_speed", moveSpeed);
            this._latestMoveSpeed = moveSpeed;
          }
          if(this._latestMoveType != this.moveType  ){ 
            mecanim.SetFloat("move_type", this.moveType);
            this._latestMoveType = this.moveType;
          }
        }

        //FIXME
        public void FixedUpdate() {
            if (!this._isStop) {
                if (this._useLoa == true) {
                    var result = RigidbodyUtils.AvoidMove(gameObject, 
                                                             this.input, 
                                                             this.newDirection, 
                                                             this.raycastSphereRadius , 
                                                             this.raycastForwardDistance, 
                                                             this.raycastSphericalDistance ,
                                                             this.offsetAngle);
                   this.input = new Vector3(result.x1, 0, result.z1);
                   this.newDirection = new Vector3(result.x2, 0, result.z2);
                   }
                }
                if (this.speedCurve != null) {
                    this.input = this.input * mecanim.GetFloat("speed_curve");
                }
                if(this.newDirection != Vector3.zero ){ 
                  this.input = this.newDirection;
                }
            }
            if(this.input.magnitude > 0) {
                if (this.autoLook) {
                    RigidbodyUtils.Move(GetComponent<Rigidbody>(), this.input * this.speed, this._useLoa);
                } else {
                    if(GameController.inputService.IsGamepad() == false  ){ 
                      RigidbodyUtils.MoveLookAtMouse(GetComponent<Rigidbody>(), this.input * this.speed, this.entity.height / 3);
                     }else{
                      if(gamepadRightStickController  &&   gamepadRightStickController.GetTarget()  ){ 
                        RigidbodyUtils.MoveNotRotate(GetComponent<Rigidbody>(), this.input * this.speed);
                        LookAt((this.entity.gamepadRightStickController.GetTarget().transform.position));
                      }else{
                        RigidbodyUtils.Move(GetComponent<Rigidbody>(), this.input * this.speed);
                      }
                     }
                }
            }
        }
    }
}