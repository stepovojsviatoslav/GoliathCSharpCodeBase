using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using MainGame.core;

namespace MainGame {
    public class EnemySupportComponent : MonoBehaviour {

        //FIXME
        public void OnFriendAttacked(object[] data) {
            //this.entity:WakeUp();
            /*if(!this.entity.relationship:HasInstance("enemy", data.target)  ){ 
              if(math.chance(this.entity.config:Get("supportScreamChance"))  ){ 
                this.entity._screamTrigger = true;
                this.entity._screamTarget = data.target;
              }
            }
            this.entity.relationship:AddInstance("enemy", data.target);*/
        }
    }
}