using System.Collections.Generic;
using UnityEngine;

namespace MainGame
{
    public class possibleactions : MonoBehaviour
    {
        private List<string> actions;
        private string defaultAction;

        public void Awake()
        {
            actions = new List<string>();
            defaultAction = "attack";
            LoadActions();
        }

        public void LoadActions()
        {
            actions = new List<string>(GetComponent<config>().Get("action") as object[]);
        }

        public string GetAction(int idx)
        {
            if (actions.Count == 0)
            {
                return defaultAction;
            }
            else if (idx < 0)
            {
                return actions[0];
            }
            else if (idx >= actions.Count)
            {
                return actions[actions.Count - 1];
            }
            else
            {
                return actions[idx];
            }
        }

        public void SetActions(string[] actions)
        {
            this.actions = new List<string>(actions);
        }
    }
}
