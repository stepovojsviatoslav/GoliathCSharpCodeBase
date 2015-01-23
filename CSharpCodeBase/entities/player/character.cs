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
    public class Character : UnityEntity
    {
        private bool isDeath = false;
        protected float deathTimeout = 0f;
        protected bool isStatic = false;
        protected int characterClass;
        protected Dictionary<string, object> config;

        new public void Awake()
        {
            //config = GetComponent<config>().Get("heroes", name);        
            gameObject.AddComponent<Health>();
            //gameObject.AddComponent<CharacterDamageReceiver>();
            //gameObject.AddComponent<DamageVisualizerComponent>();
            gameObject.AddComponent<Mover>();
            //gameObject.AddComponent<CharacterWeaponContainer>();
            //gameObject.AddComponent<CombatComponent>();
            //gameObject.AddComponent<ResistComponent>();
            //gameObject.AddComponent<DamageProcessorComponent>();
            //gameObject.AddComponent<DamageReceiverComponent>();
            gameObject.AddComponent<PlayerController>();
            //gameObject.AddComponent<PossibleActionsComponent>();
            //gameObject.AddComponent<GrenageVisualizer>();
            gameObject.AddComponent<GamePadMover>();
            if(!GameController.inputService.IsGamepad()) {
              GetComponent<GamePadMover>().enabled = false;
            }
            //gameObject.AddComponent<TimeManagerComponentr>();
            //GetComponent<Mover>().autoLook = false;            

            var tmp = config["characterClass"];
            if (tmp != null)
                characterClass = (int)tmp;
            else
                tmp = 1;
        }                                                      
        
        new public void Update()
        {
            var gamePadMover = GetComponent<GamePadMover>();
            // for left trigger
            if (gamePadMover.enabled)
            {
                if (GameController.inputService.LeftTriggerIsPressed())
                {
                    gamePadMover.enabled = false;
                    gamePadMover.Disable();
                }
                else
                {
                    gamePadMover.enabled = true;
                }
            }
            if(!isDeath)
            { 
                base.Update();
            }
            else
            {
                deathTimeout = deathTimeout - Time.deltaTime;
                if(deathTimeout <= 0)
                { 
                    Respawn();
                }
            }
        }

        new public void FixedUpdate()
        {
            if (!isDeath)
                base.FixedUpdate();        
        }

        public void CanAttack(GameObject targetEntity)
        {
        // check weapon for(target entity
          //return this.weaponContainer:CanAttack(targetEntity);
        }
        
        public void Hit(object damageData)
        {
          //if damageData.summary != 0  ){ 
            SendMessage("Hit", damageData);
          //end
        }
        
        public void Pushed(object damageData){
        //if damageData.summary != 0  ){ 
            SendMessage("Pushed", damageData);
        //end
        }
        
        public void Death()
        {
            if (characterClass > 1)
            { 
                // mech
                GetComponent<Mover>().Stop();
                isDeath = true;
                interactable = false;
                enabled = false;
                GameController.player.GetComponent<PlayerManager>().DeleteCurrentSlot();
            }
            else
            {
                GetComponent<Animator>().Play("Death");
                isDeath = true;
                interactable = false;
                deathTimeout = 5;
                GetComponent<Mover>().Stop();
          }
        }
        
        public void Respawn()
        {
            SendMessage("OnRespawn");
            SetPosition(GameController.player.homePosition) ;
            //GameController.worldController:TeleportTargetTransform(this.transform, GameController.player.homePosition.x, GameController.player.homePosition.y, GameController.player.homePosition.z);
            GetComponent<Animator>().Play("Idle");
            interactable = true;
            isDeath = false;
        }
        
        public void OnSelectCharacter()
        {
            //  GameController.ui.SetupIcon(this.config:Get("cpanel_icon"));
            //  GameController.ui.SetupAmount(this.health:GetPercentAmount());
            //  //GameController.ui.UpdateHealth(this.health:GetPercentAmount())
            Debug.Log("Spell system initialization!");
            //  GameController.spellSystem:SetupSpells(this.config:Get("spell"));
            //  //GameController.ui.slotsController:SetContainerSubTag("weapon", this.config:Get("weaponTag"))  
            interactable = true;
            enabled = true;
        }

        public void OnDeselectCharacter()
        {
            //GameController.spellSystem:SaveStatuses();
            interactable = false;
            enabled = false;
            if (GetComponent<GamePadMover>().enabled)
            { 
                GetComponent<GamePadMover>().DropTarget();
            }  
        }

        /* FiX ME AFTER SPELL will be defined
        public void OnSpellCast(Spell spell)
        {
        //  return this.playerController:OnSpellCast(spell);
        }
         */

        public void OnHealthChanged()
        {
        //  GameController.ui.SetupAmount(this.health:GetPercentAmount());
        //  //GameController.ui.UpdateHealth(this.health:GetPercentAmount())
        }

        public void OnCollisionEnter(GameObject targetEntity)
        {
        }

        public void SetTimer(string name, float value)  {
            //GetComponent<TimeManager>():Add("TimerHandler", value, name, "extended");
        }
        /*
        public void TimerHandler(tResult)    {
        //  if(tResult.timeLeft <= 0  ){ 
        //    tResult.complete = true;
        //    GameController.spellSystem:TimerUpdate(this.name, tResult.name, 0)    ;
        //  }else{
        //    GameController.spellSystem:TimerUpdate(this.name, tResult.name, tResult.timeLeft);
        //  }  
        }
         */        
    }
}