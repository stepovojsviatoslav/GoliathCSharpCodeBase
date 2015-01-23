using System.Collections.Generic;
using InControl;
using UnityEngine;

namespace MainGame 
{
    public class InputService {
        private string defaultLayer = "Default Layer";
        private string mouseAndKeyboard = "Keyboard/Mouse";

        private Stack<string> inputLayersStack;
        private InputDevice inputDevice;
    /*
        public void init ()
        {
            inputLayersStack = new Stack<string>();
            inputLayersStack.Push(defaultLayer);
            inputDevice = InputManager.ActiveDevice;
        }
     */

        public bool IsGamepad()
        {
            return inputDevice.Name != mouseAndKeyboard;
        }

        public bool CheckLayer(string layer)
        {
            if (string.IsNullOrEmpty(layer))
            {
                layer = defaultLayer;
            }
            return layer == inputLayersStack.Peek();
        }

        public void PushFrame(string name)
        {
            inputLayersStack:Push(name);
        }


        public void PopFrame()
        {
            if (inputLayersStack.Peek() != defaultLayer)
            {
                inputLayersStack.Pop();
            }
        }

        public bool GetKey(KeyCode keycode, string layer=null)
        {
            if (CheckLayer(layer))
            {
                return Input.GetKey(keycode);
            }
            else
            {
                return false;
            }
        }


        public bool GetKeyDown(KeyCode keycode, string layer=null)
        {
            if(CheckLayer(layer))
            { 
                return Input.GetKeyDown(keycode);
            }
            else
            {
                return false;
            }
        }

        
        public bool GetKeyDownUnlocked(KeyCode keycode, string layer=null)
        {
            if (CheckLayer(layer))
            {
                return Input.GetKeyDown(keycode);
            }
            else
            {
                return false;
            }
        }

        public bool GetMouseButtonDown(int button, string layer=null)
        {
            if (CheckLayer(layer))
            {
                return Input.GetMouseButtonDown(button)
            }
            else
            {
                return false;
            }
        }

        public bool GetMouseButton(int button, string layer=null)
        {
            if (CheckLayer(layer))
            {
                return Input.GetMouseButtonDown(button);
            }
            else
            {
                return false;
            }
        }

        public bool BottomButtonWasPressed(string layer=null)
        {
            if (CheckLayer(layer))
            {
                return inputDevice.Action1.WasPressed;
            }
            else
            {
                return false;
            }
        }

        public bool LeftButtonWasPressed(string layer=null)
        {
            if (CheckLayer(layer))
            {
                return inputDevice.Action3.WasPressed;
            }
            else
            {
                return false;
            }
        }

        public bool TopButtonWasPressed(string layer=null)
        {
            if (CheckLayer(layer))
            {
                return inputDevice.Action4.WasPressed;
            }
            else
            {
                return false;
            }
        }

        public bool RightButtonWasPressed(string layer=null)
        {
            if (CheckLayer(layer))
            {
                return inputDevice.Action2.WasPressed;
            }
            else
            {
                return false;
            }
        }

        public Vector3 LeftStickValues(string layer=null)
        {
            Vector3 value = new Vector3();
            if (CheckLayer(layer))
            {
                value.x = inputDevice.LeftStickX.Value;
                value.z = inputDevice.LeftStickY.Value;
            }
            return value;
        }

        public bool LeftStickYIsPressed(string layer=null)
        {
            if (CheckLayer(layer))
            {
                return inputDevice.LeftStickY.IsPressed;
            }
            else
            {
                return false;
            }
        }

        public bool LeftStickXIsPressed(string layer=null)
        {
            if (CheckLayer(layer))
            {
                return inputDevice.LeftStickX.IsPressed;
            }
            else
            {
                return false;
            }
        }

        public bool RightTriggerWasPressed(string layer=null)
        {
            if (CheckLayer(layer))
            {
                return inputDevice.RightTrigger.WasPressed;
            }
            else
            {
                return false;
            }
        }

        public bool RightTriggerIsPressed(string layer=null)
        {
            if (CheckLayer(layer))
            {
                return inputDevice.RightTrigger.IsPressed;
            }
            else
            {
                return false;
            }
        }

        public bool LeftTriggerWasPressed(string layer=null)
        {
            if (CheckLayer(layer))
            {
                return inputDevice.LeftTrigger.WasPressed;
            }
            else
            {
                return false;
            }
        }

        public bool LeftTriggerIsPressed(string layer=null)
        {
            if (CheckLayer(layer))
            {
                return inputDevice.LeftTrigger.IsPressed;
            }
            else
            {
                return false;
            }
        }

        public Vector3 GetLookValue(string layer=null)
        {
            Vector3 value = new Vector3();
            if (CheckLayer(layer))
            {
                value.x = inputDevice.RightStickX.Value;
                value.z = inputDevice.RightStickY.Value;
            }
            return value;
        }

        // FIXME: Эти функции дублируются с двумя нижними
        public bool LookXWasPressed(string layer=null)
        {
            if (CheckLayer(layer))
            {
                return inputDevice.RightStickX.WasPressed;
            }
            else
            {
                return false;
            }
        }

        public bool LookYWasPressed(string layer=null)
        {
            if (CheckLayer(layer))
            {
                return inputDevice.RightStickY.WasPressed;
            }
            else
            {
                return false;
            }
        }

        public bool RightStickYWasPressed(string layer=null)
        {
            if (CheckLayer(layer))
            {
                return inputDevice.RightStickY.WasPressed;
            }
            else
            {
                return false;
            }
        }

        public bool RightStickXWasPressed(string layer=null)
        {
            if (CheckLayer(layer))
            {
                return inputDevice.RightStickX.WasPressed;
            }
            else
            {
                return false;
            }
        }

        public bool RightStickButtonWasPressed(string layer=null)
        {
            if (CheckLayer(layer))
            {
                return inputDevice.RightStickButton.WasPressed;
            }
            else
            {
                return false;
            }
        }

        public bool LeftStickButtonWasPressed(string layer=null)
        {
            if (CheckLayer(layer))
            {
                return inputDevice.LeftStickButton.WasPressed;
            }
            else
            {
                return false;
            }
        }

        public bool LeftBumperWasPressed(string layer=null)
        {
            if (CheckLayer(layer))
            {
                return inputDevice.LeftBumper.WasPressed;
            }
            else
            {
                return false;
            }
        }

        public bool LeftBumperIsPressed(string layer=null)
        {
            if (CheckLayer(layer))
            {
                return inputDevice.LeftBumper.IsPressed;
            }
            else
            {
                return false;
            }
        }

        public bool RightBumperWasPressed(string layer=null)
        {
            if (CheckLayer(layer))
            {
                return inputDevice.RightBumper.WasPressed;
            }
            else
            {
                return false;
            }
        }

        public bool RightBumperIsPressed(string layer=null)
        {
            if (CheckLayer(layer))
            {
                return inputDevice.RightBumper.IsPressed;
            }
            else
            {
                return false;
            }
        }

        public bool DPadLeftIsPressed(string layer=null)
        {
            if (CheckLayer(layer))
            {
                return inputDevice.DPadLeft.IsPressed;
            }
            else
            {
                return false;
            }
        }

        public bool DPadRightIsPressed(string layer=null)
        {
            if (CheckLayer(layer))
            {
                return inputDevice.DPadRight.IsPressed;
            }
            else
            {
                return false;
            }
        }

        public bool DPadUpIsPressed(string layer=null)
        {
            if (CheckLayer(layer))
            {
                return inputDevice.DPadUp.IsPressed;
            }
            else
            {
                return false;
            }
        }

        public bool DPadDawnIsPressed(string layer=null)
        {
            if (CheckLayer(layer))
            {
                return inputDevice.DPadDown.IsPressed;
            }
            else
            {
                return false;
            }
        }
    }
}