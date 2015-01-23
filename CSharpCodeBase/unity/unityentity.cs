using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using MainGame.core;


namespace MainGame {
    public class UnityEntity : MonoBehaviour {
        protected bool visible;
        protected bool enabled;
        protected bool interactable;
        protected bool isStatic;
        protected float visibilityTime;
        protected float radius;
        protected float height;
        protected Vector3 capsuleOffset;

        public bool IsInteractable {
            get { return interactable; }
            set { interactable = value; }
        }

        public void Awake() {
            InitRadiusAndHeight();

            this.isStatic = true;
            this.enabled = false;
            this.visible = false;
            this.interactable = true;
            this.visibilityTime = 0;
        }

        public void FixedUpdate()
        {
        }

        //FIXME PLEASE
        public void InitRadiusAndHeight() {
            //var result = {}
            //RigidbodyUtils.GetCapsuleColliderData(gameObject, result);
            //this.radius = result.radius  ||  0.5;
            //this.height = result.height  ||  0.5;
            //this.capsuleOffset = Vector3(result.ox  ||  0, result.oy  ||  0, result.oz  ||  0);
            this.radius = 0.5f;
            this.height = 0.5f;
            this.capsuleOffset = Vector3.zero;
        }

        public void OnChangeVisibility(bool state) {
            this.visible = state;
            this.enabled = state;
            SendMessage("OnChangeVisibility", state);
            this.visibilityTime = 0;
        }

        public void Update() {
            if (this.visible) {
                this.visibilityTime = this.visibilityTime + Time.deltaTime;
            }
        }

        //FIXME
        public void OnUnload() {
            SendMessage("RemoveFromTimeService");
            //GameController.entityFactory:Destroy(self);
            //GameController:RemoveEntity(self)
            //GameController.pooler:Release(this.gameObject)
        }

        public void OnEvent(object[] data) {
            SendMessage("OnEvent", data);
        }

        //FIXME
        public void Destroy() {
            //GameController.entityFactory:Destroy(self);
        }

        public string GetType() {
            return "UnityEntity";
        }

        //FIXME
        public void LookAt(Vector3 vector) {
            //var rot = RotationUtils.LookRotation(Vector3(vec3.x, 0, vec3.z));
            //self:SetRotation(rot);
        }

        public void SetPosition(Vector3 position) {
            transform.position = position;
        }

        public Vector3 GetPosition() {
            return transform.position;
        }

        public Vector2 GetPosition2D() {
            var pos = this.GetPosition();
            return new Vector2(pos.x, pos.z);
        }

        public void SetRotation(Vector3 rotation) {
            transform.rotation = Quaternion.Euler(rotation);
        }

        public Vector3 GetRotation() {
            return transform.rotation.eulerAngles;
        }


        public float GetEffectiveDistanceToVec(Vector3 vec3) {
            var localPos = GetPosition2D();
            var targetPos = new Vector2(vec3.x, vec3.z);
            var length = (localPos - targetPos).magnitude;
            var result = length - this.radius;
            Debug.Log(result);
            if (result < 0) {
                result = 0;
            }
            return result;
        }

        public float GetSimpleDistance(UnityEntity targetEntity) {
            var localPos = GetPosition2D();
            var targetPos = targetEntity.GetPosition2D();
            var result = (localPos - targetPos).magnitude;
            return result;
        }

        public float GetSimpleDistanceToVec(Vector3 vec3) {
            var localPos = GetPosition2D();
            var targetPos = new Vector2(vec3.x, vec3.z);
            var result = (localPos - targetPos).magnitude;
            return result;
        }

        public float GetEffectiveDistance(UnityEntity targetEntity) {
            var localPos = transform.TransformPoint(this.capsuleOffset);
            var targetPos = transform.TransformPoint(targetEntity.capsuleOffset);
            localPos.y = 0;
            targetPos.y = 0;
            var result = (localPos - targetPos).magnitude - this.radius - targetEntity.radius;
            if (result < 0) { result = 0; }
            return result;
        }

        public void SetScale(Vector3 scale) {
            transform.localScale = scale;
        }

        //FIXME
        public void ChangeRadiusAndHeight(GameObject gameObject) {
            //this.radius = RigidbodyUtils.TryGetRadius(gameObject)  ||  0.5;
            //this.height = RigidbodyUtils.TryGetHeight(gameObject)  ||  0.5;
        }
    }
}