using System.Collections.Generic;

namespace MainGame
{
    public class actionmachine
    {
        private List<action> buffer;
        private int bufferLimit = 2;
        private action previousAction = null;
        private action emptyAction = null;

        public void init(action emptyAction = null)
        {
            buffer = new List<action>();
            previousAction = null;
            this.emptyAction = emptyAction;
        }

        public action GetCurrentAction()
        {
            if (buffer.Count > 0)
            {
                return buffer[buffer.Count - 1];
            }
            else
            {
                return emptyAction;
            }
        }

        public void Flush()
        {
            action currentAction = GetCurrentAction();
            foreach (action v in buffer)
            {
                v.OnRemove();
            }
            buffer.Clear();
        }

        public void FixedUpdate()
        {
            action currentAction = GetCurrentAction();
            if (currentAction != null && currentAction.IsStarted())
            {
                currentAction.FixedUpdate();
            }
        }

        public void Update()
        {
            action currentAction = GetCurrentAction();
            if (currentAction != previousAction)
            {
                if (previousAction != null && previousAction.IsStarted())
                {
                    previousAction.OnSuspend();
                }
                if (currentAction != null)
                {
                    if (currentAction.IsStarted())
                    {
                        currentAction.OnResume();
                    }
                    else
                    {
                        currentAction.OnStart();
                    }
                }
                previousAction = currentAction;
            }
            else if (currentAction != null && currentAction == previousAction && !currentAction.IsStarted())
            {
                currentAction.OnStart();
            }

            if (currentAction != null)
            {
                if (currentAction.Update())
                {
                    buffer.RemoveAt(buffer.Count - 1);
                    currentAction.OnComplete();
                    previousAction = null;
                }
            }
        }

        public bool IsActionExists(string name)
        {
            foreach (action v in buffer)
            {
                if (v.name == name)
                {
                    return true;
                }
            }
            return false;
        }

        public int InsertAction(action act)
        {
            int idx = 0;
            foreach (action v in buffer)
            {
                if (v.GetPriority() <= act.GetPriority())
                {
                    idx++;
                }
                else
                {
                    break;
                }
            }
            buffer.Insert(idx, act);
            return idx;
        }

        public void CutLimit()
        {
            while (buffer.Count > bufferLimit)
            {
                buffer[0].OnRemove();
                buffer.RemoveAt(0);
            }
        }

        public void RemoveActionsWithSamePriority()
        {
            int previousPriority = buffer[buffer.Count - 1].GetPriority();
            for (int i = buffer.Count - 2; i >= 0; i--)
            {
                int currentPriority = buffer[i].GetPriority();
                if (currentPriority == previousPriority)
                {
                    buffer[i].OnRemove();
                    buffer.RemoveAt(i);
                }
                else
                {
                    previousPriority = currentPriority;
                }
            }
        }

        public void RemoveUncontinuousActions()
        {
            bool needIterate = true;
            while (needIterate)
            {
                bool isRemoved = false;
                for (int i = 0; i < buffer.Count; i++)
                {
                    action v = buffer[i];
                    if (i < buffer.Count - 2)
                    {
                        if (!v.IsContinuous() && v.IsStarted())
                        {
                            v.OnRemove();
                            buffer.RemoveAt(i);
                            isRemoved = true;
                            break;
                        }
                    }
                }
                needIterate = isRemoved;
            }
        }

        public void PushAction(action act)
        {
            action currentAction = GetCurrentAction();
            InsertAction(act);
            CutLimit();
            RemoveActionsWithSamePriority();
            RemoveUncontinuousActions();
            act.OnPushed();
        }

        public bool IsEmpty()
        {
            return buffer.Count == 0;
        }

        public action GetTop()
        {
            if (buffer.Count > 0)
            {
                return buffer[buffer.Count - 1];
            }
            else
            {
                return null;
            }
        }
    }
}
