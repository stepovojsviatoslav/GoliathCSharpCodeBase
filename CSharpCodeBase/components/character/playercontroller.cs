using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using MainGame.core;

namespace MainGame {
    public class PlayerController : MonoBehaviour {
        private int countSlots = 0;
        private int maxCountSlots = 1;
        private int currentSlot = 0;
        private int characterGearSelected = 0;
        private bool characterGearEnabled = false;
        private List<GameObject> slots = new List<GameObject>();
        private CharacterGear characterGear = GameFacade.characterGear;


        /*public void init (self, entity)      ;
              Component.init(self, entity);
              this.entity.playerController = self;
              this.slots = {}      
              this.countSlots = 0;
              this.maxCountSlots = GameController.database:Get("heroes", "Common/maxCountSlots");
              this.currentSlot = 0                  ;
              this.characterGear = luanet.GameFacade.characterGear;
              this.characterGear:SetupFinalCallback(function (idx) self:OnUIFinalSelectionCallback(idx) }){
              this.characterGearEnabled = false;
        }})
       */

        public void Awake() {
            this.maxCountSlots = GetComponent<config>.Get("heroes", "Common/maxCountSlots");
            this.characterGear.SetupFinalCallback(/*callvack*/);
        }

        public void OnUIFinalSelectionCallback(int idx) {
            if (idx + 1 != currentSlot) {
                SelectSlot(idx + 1);
            }
        }

        public GameObject GetSlot(int number) {
            if (number > 0 && number <= this.countSlots) {
                return this.slots[number];
            } else {
                return null;
            }
        }

        public GameObject GetCurrentSlot() {
            if (this.currentSlot > 0) {
                return GetSlot(this.currentSlot);
            } else {
                return null;
            }
        }

        //FIXME
        public bool SelectSlot(int number) {
            if (number != this.currentSlot && number > 0 && number <= this.countSlots) {
                if (this.currentSlot != 0) {
                    var oldSlot = GetCurrentSlot();
                    oldSlot.SetActive(false);
                    SendMessage("OnDeselectCharacter");
                    this.currentSlot = number;
                } else {
                    this.currentSlot = 1;
                }
                var newSlot = GetCurrentSlot();
                newSlot.transform.position = transform.position;
                newSlot.transform.rotation = transform.rotation;
                newSlot.SetActive(true);
                SendMessage("OnSelectCharacter");
                GameController.camera.LoadForEntity(newSlot);
                return true;
            } else {
                return false;
            }
        }
        //FIXME
        public GameObject LoadSlot(string entityName) {
            var bundleName = GameController.database.Get("heroes", entityName + "/bundle");
            var character = BundleUtils.CreateFromBundle(bundleName);
            character.name = entityName;
            var characterEntity = new CharacterEntity(character);
            return characterEntity;
        }

        public bool AddSlot(GameObject entity) {
            if (this.countSlots < this.maxCountSlots) {
                entity.SetActive(false);
                this.slots.Add(entity);
                this.countSlots = this.countSlots + 1;
                return true;
            }
            return false;
        }
        //FIXME
        public bool DeleteSlot(int number) {
            if (number > 1 && number <= this.countSlots) {
                var deletedSlot = this.currentSlot;
                SelectSlot(1);
                this.slots.RemoveAt(deletedSlot);
                //deletedSlot:Destroy ();
                this.countSlots = this.countSlots - 1;
                return true;
            }
            return false;
        }

        public void LoadAndAdd(string entityName) {
            var characterEntity = LoadSlot(entityName);
            AddSlot(characterEntity);
        }

        public bool DeleteCurrentSlot() {
            var deletedSlot = this.currentSlot;
            SelectSlot(1);
            this.slots.RemoveAt(deletedSlot);
            //deletedSlot: Destroy();
            this.countSlots = this.countSlots - 1;
            return true;
        }

        public void Update() {
            var slot = GetCurrentSlot();
            if (slot != null) {
                transform.position = slot.transform.position;
                transform.rotation = slot.transform.rotation;
            }
            if (!this.characterGearEnabled && GameController.inputService.LeftBumperIsPressed()) {
                this.characterGearEnabled = true;
                for (int i = 0; i < 4; i++) {
                    var islot = this.slots[i];
                    if (islot != null) {
                        this.characterGear.SetupSlot(i - 1, islot.config.Get("cgear_icon_left"),
                          islot.config.Get("cgear_icon_top"),
                          islot.config.Get("cgear_icon_bottom"));
                    } else {
                        this.characterGear.SetupSlot(i - 1, "", "", "");
                    }
                }
                this.characterGearSelected = this.currentSlot - 1;
                this.characterGear.Show(this.currentSlot - 1);
                GameController.inputService.PushFrame("character_gear");
            }
            if (this.characterGearEnabled && !GameController.inputService.LeftBumperIsPressed("character_gear")) {
                this.characterGearEnabled = false;
                this.characterGear.Hide();
                GameController.inputService.PopFrame();
            }
            if (this.characterGearEnabled) {
                if (GameController.inputService.IsGamepad()) {
                    var lookVector = GameController.inputService.GetLookValue("character_gear");
                    var selected = this.characterGearSelected;
                    //selected = this.currentSlot - 1
                    if (lookVector.magnitude > 0.4f) {
                        var angle = Mathf.Atan2(lookVector.z, lookVector.x) * 180 / Mathf.PI;
                        while (angle < 0) {
                            angle = angle + 360;
                        }
                        if (angle > 315 || angle <= 45) {
                            selected = 1;
                        } else if (angle > 225 && angle <= 315) {
                            selected = 0;
                        } else if (angle > 135 && angle <= 225) {
                            selected = 3;
                        } else {
                            selected = 2;
                        }
                    }
                    if (this.slots[selected + 1] == null) {
                        selected = this.characterGearSelected;
                        //selected = this.currentSlot - 1
                    }
                    if (selected != this.characterGearSelected) {
                        // update
                        this.characterGear.UpdateSelected(selected);
                        this.characterGearSelected = selected;
                    }
                }
            }
        }
    }
}