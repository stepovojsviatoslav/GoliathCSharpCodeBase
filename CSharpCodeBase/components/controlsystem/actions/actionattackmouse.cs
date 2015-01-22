using UnityEngine;
namespace MainGame
{
    public class actionattackmouse : action
    {
        private int STATE_MOVING = 0;
        private int STATE_REQUEST_ATTACK = 0;
        private int STATE_ATTACK = 2;

        private int state;

        private GameObject gameObject;
        private GameObject target;
        public actionattackmouse(GameObject gameObject, GameObject target = null)
        {
            base.init(2, false, "fastattack");
            this.gameObject = gameObject;
            this.target = target;
            state = STATE_REQUEST_ATTACK;
        }

        public override void OnPushed()
        {
            priority = 3;
        }

        public override void OnStartRunning()
        {
            state = STATE_REQUEST_ATTACK;
            gameObject.GetComponent<combat>().AttackVector(target);
            gameObject.GetComponent<mover>().SetSpeed(gameObject.GetComponent<config>().Get("moverSpeed") / 4);
        }

        public override void OnStopRunning()
        {
            gameObject.GetComponent<mover>().ResetSpeed();
            base.OnStopRunning();
        }

        public override bool Update()
        {
            ProcessMoving();
            return state == STATE_ATTACK
        }

        public void ProcessMoving()
        {
            if (GameController.inputService.LeftStickYIsPressed() || GameController.inputService.LeftStickXIsPressed())
            {
                Vector3 currentInput = GameController.inputService.LeftStickValues();
                currentInput.RotateAroundY(GameController.camera.angle); // FIXME, vector3 doesn't have this method
                gameObject.GetComponent<mover>().SetInput(currentInput);
            }
        }

        public void OnEvent(object data)
        {
            string stringdata = (string)data;
            if (strindata == "punch")
            {
                state = STATE_ATTACK;
                // FIXME: Attack code here
            }
        }
        /*
         public void ActionFastAttack:OnEvent(data){
           if(data == "punch"  ){   
             this.state = STATE_ATTACK;
             // Get damage objects
             var miss = true;
             if(this.entity.weaponContainer.currentWeapon.remote > 0  ){ 
               // shoot
               var sourceForward = Transform.GetForwardVector(this.entity.transform);
               this.entity.weaponContainer:Attack(this.entity:GetPosition() + this.target);
             }else{
               var nearEntities = RaycastUtils.GetEntitiesInRadius(this.entity:GetPosition(), 5);
               var sourceForward = Transform.GetForwardVector(this.entity.transform);
               for(k, v in pairs(nearEntities) ){
                 if(v != this.entity  &&  v.interactable  ){ 
                   if(HitTester.CheckHitEntity(this.entity, sourceForward, v, 45, 1.5)  ){ 
                     this.entity.damageProcessor:SendDamage(v, this.entity.weaponContainer.currentWeapon:GetDamage());
                     if(GameController.inputService:IsGamepad()  &&  this.entity.gamepadRightStickController:GetTarget() == null  &&  v.gameObject.tag != "Player"  &&  v.gameObject.tag == "Enemy"  ){ 
                       this.entity.gamepadRightStickController:SetTarget(v);
                     }
                     miss = false;
                   }
                 }
               }
             }
             if(miss  ){ 
               this.entity.combat:ResetCombo();
             }
           }
         }
         */
        public override bool IsContinuous()
        {
            return false;
        }
    }
}
