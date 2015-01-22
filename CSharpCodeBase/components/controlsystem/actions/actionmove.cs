using UnityEngine;
namespace MainGame
{
    public class actionmove : action
    {
        private GameObject gameObject;
        private GameObject target;

        private bool isKeyboardControl = true;
        private Vector3 currentInput;

        public actionmove(GameObject gameObject, GameObject target = null)
        {
            base.init(1, true, "move");
            this.gameObject = gameObject;
            this.target = target;
            isKeyboardControl = true;
        }

        public void SwitchToKeyboard()
        {
            isKeyboardControl = true;
        }
        public void SwitchToMouse()
        {
            isKeyboardControl = false;
        }

        public override void OnStartRunning()
        {
            if (target != null)
            {
                SwitchToMouse();
                if (target != null)
                {
                    gameObject.GetComponent<mover>().SetGoal(target);
                }
                else
                {
                    if (targetVec != Vector3.zero)
                    {
                        gameObject.GetComponent<mover>().SetGoal(targetVec);
                    }
                }
            }
            GetComponent<mover>().ResetSpeed();
        }

        public override bool Update()
        {
            bool actionResult = true;
            if (GameController.inputService.LeftStickYIsPressed() || GameController.inputService.LeftStickXIsPressed())
            {
                currentInput = GameController.inputService.LeftStickValues();
                currentInput.RotateAroundY(GameController.camera.angle); //FIXME: rotate around y not implemented in vector
                gameObject.GetComponent<mover>().SetInput(currentInput);
                actionResult = false;
            }
            if (isKeyboardControl)
            {
                if (input.GetMouseButtonDown(1))
                {
                    gameObject.GetComponent<mover>().SetGoal(InputUtils.RaycastMouseOnTerrain());
                    SwitchToMouse();
                    actionResult = false;
                }
                if (GameController.inputService.GetMouseButton(1) && currentInput.magnitude == 0)
                {
                    SwitchToMouse();
                    actionResult = false;
                }
            }
            if (!isKeyboardControl)
            {
                if (GameController.inputService.GetMouseButton(1))
                {
                    GameObject targetEntity = InputUtils.RaycastTargetEntity();
                    if (targetEntity != null)
                    {
                        gameObject.GetComponent<mover>().SetGoal(targetEntity);
                    }
                    else
                    {
                        gameObject.GetComponent<mover>().SetGoal(InputUtils.RaycastMouseOnTerrain());
                    }
                    actionResult = false;
                }
                if (!gameObject.GetComponent<mover>().IsHaveGoal() || GameController.inputService.LeftStickYIsPressed() || GameController.inputService.LeftStickXIsPressed())
                {
                    SwitchToKeyboard();
                }
                if (gameObject.GetComponent<mover>().IsHaveGoal())
                {
                    actionResult = false;
                }
            }
            return actionResult;
        }

        public override void OnStopRunning()
        {
            gameObject.GetComponent<mover>().Stop();
        }
    }
}