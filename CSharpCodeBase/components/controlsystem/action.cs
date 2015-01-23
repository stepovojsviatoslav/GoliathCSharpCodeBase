namespace MainGame
{
    public class action
    {
        protected int priority;
        protected bool continuous;
        protected string name;

        private bool isStarted = false;

        public void init(int priority, bool continuous, string name)
        {
            this.priority = priority;
            this.continuous = continuous;
            this.name = name;
            this.isStarted = false;
        }

        public bool IsStarted()
        {
            return isStarted;
        }

        public void OnStart()
        {
            isStarted = true;
            OnStartRunning();
        }

        public void OnSuspend()
        {
            OnStopRunning();
        }

        public void OnResume()
        {
            OnStartRunning();
        }

        public void OnComplete()
        {
            OnStopRunning();
        }

        public void OnRemove()
        {
            OnStopRunning();
        }

        virtual public void FixedUpdate()
        {
        }

        virtual public bool Update()
        {
        }

        virtual public void OnStopRunning()
        {
        }

        virtual public void OnStartRunning()
        {
        }

        virtual public void OnPushed()
        {
        }

        virtual public bool IsContinuous()
        {
            return continuous;
        }

        virtual public int GetPriority()
        {
            return priority;
        }

        virtual public void OnEvent(object data)
        {
        }
    }
}
 