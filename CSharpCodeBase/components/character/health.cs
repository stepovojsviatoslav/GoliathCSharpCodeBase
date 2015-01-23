using System;

using System.Collections.Generic;

using System.Linq;

using System.Text;

using UnityEngine;

using System.Collections;

using MainGame.core;


namespace MainGame {
    public class Health : MonoBehaviour {
        public enum HEALTH_STATE { STATE_LOW, STATE_NORMAL, STATE_HIGH } ;
        private HEALTH_STATE currentHealthState = HEALTH_STATE.STATE_HIGH;
        private float maxAmount;
        private float amount;

        public void Awake() {
            this.maxAmount = GetComponent<config>().Get("healthMaxAmount") != null ? GetComponent<config>().Get("healthMaxAmount") : 100;
            this.amount = this.maxAmount;
            HealthChanged();
        }

        public float Decrease(float value) {
            this.amount = this.amount - value;
            if (this.amount < 0) {
                this.amount = 0;
            }
            HealthChanged();
            return this.amount;
        }

        public float Increase(float value) {
            this.amount = this.amount + value;
            if (this.amount > this.maxAmount) {
                this.amount = this.maxAmount;
            }
            HealthChanged();
            return this.amount;
        }

        public void OnRespawn() {
            Reset();
        }

        public void Reset() {
            this.amount = this.maxAmount;
            HealthChanged();
        }

        public void SetNewHealth(float health) {
            this.maxAmount = health;
            this.amount = this.maxAmount;
            HealthChanged();
        }

        public HEALTH_STATE GetState() {
            var value = this.amount / this.maxAmount;
            if (value >= 0.7) {
                return HEALTH_STATE.STATE_HIGH;
            } else if (value >= 0.4) {
                return HEALTH_STATE.STATE_NORMAL;
            } else {
                return HEALTH_STATE.STATE_LOW;
            }
        }

        public float GetPercentAmount() {
            return this.amount / this.maxAmount;
        }

        public void Load(Storage storage) {
            var amount = storage.GetFloat("healthcomponent_amount", -1);
            if (amount == -1) {
                this.amount = this.maxAmount;
            } else {
                this.amount = amount;
            }
            HealthChanged();
        }

        public void Save(Storage storage) {
            storage.SetFloat("healthcomponent_amount", this.amount);
        }

        public void HealthChanged() {
            //if(this.entity.OnHealthChanged  ){ 
            //   this.entity:OnHealthChanged(this.amount);
            //}
            var state = GetState();
            if (!this.currentHealthState.Equals(state)) {
                this.currentHealthState = state;
                SendMessage("EntityHealthChanged", state);
            }
        }

        //var HealthComponent = Class({__includes=Component, init=function(self, entity)
        //      Component.init(self, entity);
        //      this.STATE_HIGH = "high";
        //      this.STATE_NORMAL = "normal";
        //      this.STATE_LOW = "low";
        //      this.entity.health = self;
        //      this.currentHealthState = this.STATE_HIGH;
        //      this.maxAmount = this.entity.config:Get("healthMaxAmount")  ||  100;
        //      this.amount = this.maxAmount;
        //      self:HealthChanged();
        //}})




        //Component
    }
}