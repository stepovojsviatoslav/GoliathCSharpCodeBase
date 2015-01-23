using UnityEngine;
namespace MainGame
{
    public class PlayerController : MonoBehaviour
    {
        private actionmachine actionMachine;
        private float spaceCommandTimer;
        private float raycastScreenTimer;
        private GameObject currentLabelEntity;
        private UIMouseUnderLabel mouseLabel;
        private InputService inputService;

        public void init()
        {
            actionMachine = new actionmachine();
            spaceCommandTimer = 0;
            raycastScreenTimer = 0;
            currentLabelEntity = null;
            mouseLabel = GameFacade.uiMouseUnderLabel;
            mouseLabel.Hide();
            //FIXME Setup player controller to entity
            inputService = GameController.inputService;
        }

        public bool IsMoveKeyPressed()
        {
            return inputService.LeftStickYIsPressed() || inputService.LeftStickXIsPressed();
        }

        public void FixedUpdate()
        {
            actionMachine.FixedUpdate();
        }

        public GameObject GetInteractEntityUnderMouse()
        {
            GameObject target = InputUtils.RaycastTargetEntity();
            possibleactions possibleActions = target.GetComponent<possibleactions>();
            unityentity entity = target.GetComponent<unityentity>();
            if (target != null && possibleActions != null && entity.interactable)
            {
                if (possibleActions.GetAction(0) == "take" || possibleActions.GetAction(0) == "interact")
                {
                    return target;
                }
            }
            return null;
        }

        public GameObject GetNearInteractEntity(float radius)
        {
            GameObject[] entities = raycastutils.GetEntitiesInRadius(transform.position, radius);
            float min = radius * 10;
            GameObject minEntity = null;

            foreach (GameObject entity in entities)
            {
                if (entity != null && entity.GetComponent<possibleactions>() != null && entity.GetComponent<unityentity>().interactable)
                {
                    possibleactions possibleActions = entity.GetComponent<possibleactions>();
                    if (possibleActions.GetAction(0) == "take" || possibleActions.GetAction(0) == "interact")
                    {
                        float dst = entity.GetComponent<unityentity>().GetSimpleDistance(gameObject);
                        if (dst < min)
                        {
                            min = dst;
                            minEntity = entity;
                        }
                    }
                }
            }

            return minEntity;
        }

        public void Update()
        {
            SearchingThings();
            Interact();
            SpecialAction();
            Move();
            Attack();
            actionMachine.Update();
        }

        public void SpecialAction()
        {
            if ((inputService.RightTriggerWasPressed() && !actionMachine.IsActionExists("block"))
                || (inputService.RightTriggerWasPressed() && actionMachine.IsEmpty()))
            {
                actionMachine.PushAction(new actionblock(gameObject));
            }
        }

        public void SearchingThings()
        {
            raycastScreenTimer = raycastScreenTimer - GameController.deltaTime;
            if (raycastScreenTimer < 0 && !actionMachine.IsActionExists("take"))
            {
                raycastScreenTimer = 0.2f;
                GameObject previousLabelEntity = currentLabelEntity;
                currentLabelEntity = GetInteractEntityUnderMouse();
                if (currentLabelEntity == null)
                {
                    currentLabelEntity = GetNearInteractEntity(3);
                }
                if (currentLabelEntity != null)
                {
                    Vector3 pos = currentLabelEntity.transform.position;
                    mouseLabel.Show(pos.x, pos.y + currentLabelEntity.GetComponent<unityentity>().height, pos.z, "'E' to take");
                }
                else
                {
                    if (previousLabelEntity == null)
                    {
                        mouseLabel.Hide();
                    }
                }

            }
        }

        public void Interact()
        {
            if (inputService.BottomButtonWasPressed() && currentLabelEntity != null)
            {
                mouseLabel.Hide();
                actionMachine.PushAction(new actiontake(gameObject, currentLabelEntity));
            }
        }

        public void Move()
        {
            spaceCommandTimer = spaceCommandTimer - GameController.deltaTime;
            if (!actionMachine.IsActionExists("block") && (IsMoveKeyPressed() || inputService.GetMouseButton(1)))
            {
                if (!actionMachine.IsActionExists("move"))
                {
                    if (inputService.GetMouseButtonDown(1))
                    {
                        actionMachine.PushAction(new actionmove(gameObject, InputUtils.RaycastMouseOnTerrain()));
                    }
                    else
                    {
                        actionMachine.PushAction(new actionmove(GameObject));
                    }
                }
            }
        }

        public void Attack()
        {
            bool needAttack = inputService.LeftButtonWasPressed();
            if (!inputService.IsGamepad())
            {
                if (!InputUtils.IsPointerOverUI())
                {
                    needAttack = false;
                }
            }

            needAttack = needAttack && GameController.ui.mainInterface.IsAttackPossible();

            if (needAttack)
            {
                if (gameObject.GetComponent<grenademodevisualizer>().active)
                {
                    grenademodevisualizer gc = gameObject.GetComponent<grenademodevisualizer>();
                    grenade g = grenade(gc.name, gameObject, gc.GetSpeed());
                }

                Vector3 vec3;
                GamePadMover gmover = gameObject.GetComponent<GamePadMover>();
                if (gmover != null && gmover.GetTarget() != null)
                {
                    vec3 = gmover.GetTarget().transform.position;
                    gameObject.GetComponent<mover>().LookAt(gmover.GetTarget().transform.position);
                }
                else
                {
                    vec3 = transform.position;
                }
                actionMachine.PushAction(new actionattackmouse(gameObject, vec3));
            }
        }

        /*
        public void Hit(damageData)  {
          if(damageData.summary != 0  &&  this.entity.characterClass <= damageData.source.characterClass  ){ 
            this.entity.mecanim:ForceSetState("Empty", 1);
            this.actionMachine:PushAction(this.luaPool:Fetch(ActionHit, this.entity));
          }
        }
        public void Pushed(damageData){
          if(this.entity.characterClass <= damageData.source.characterClass  ){ 
            this.entity.mecanim:ForceSetState("Empty", 1);
            this.actionMachine:PushAction(ActionPush(this.entity, damageData.source:GetPosition()));
          }
        }*/

        public void OnDeselectCharacter()
        {
            actionMachine.Flush();
        }

        public void OnSelectCharacter()
        {

        }

        public void OnRespawn()
        {
            actionMachine.Flush();
        }

        public void OnEvent(object data)
        {
            action state = actionMachine.GetTop();
            if (state != null)
            {
                state.OnEvent(data);
            }
        }

        public bool OnSpellCast(action spellAction)
        {
            spellAction.gameObject = gameObject;
            actionMachine.PushAction(spellAction);
            return true;
        }
    }
}
