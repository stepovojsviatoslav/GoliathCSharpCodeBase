using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace MainGame
{
    public class daytime
    {
        public enum ePhase
        {
            PHASE_MORNING = 0,
            PHASE_DAY_START = 1,
            PHASE_DAY_PROGRESS = 2,
            PHASE_DAY_END = 3,
            PHASE_EVENING = 5,
            PHASE_NIGHT = 6,
        };

        public ePhase currentPhase;

        public int currentDay = 0;
        private Dictionary<ePhase, float> phaseTimes;
        private Dictionary<ePhase, Color> phaseColors;
        private Dictionary<ePhase, Color> phaseAmbientColors;

        private float currentPhaseTime;
        private float currentPhaseTimeRunning;

        public daytime()
        {
            string[] configPhases = new string[] {"morning", "daystart", "day", "dayend", "evening", "night"};
            currentDay = 0;

            phaseTimes = new Dictionary<ePhase, float>();
            phaseColors = new Dictionary<ePhase, Color>();
            phaseAmbientColors = new Dictionary<ePhase, Color>();

            for (int i = 0; i < configPhases.Length; i++)
            {
                phaseTimes[(ePhase) i] = GameController.database.Get("daytime", configPhases[i] + "/time");
                phaseColors[(ePhase) i] = GameController.database.Get("daytime", configPhases[i] + "/color");
                phaseAmbientColors[(ePhase) i] = GameController.database.Get("daytime", configPhases[i] + "/acolor");
            }

            currentPhase = ePhase.PHASE_MORNING;
            currentPhaseTime = phaseTimes[currentPhase];
            currentPhaseTimeRunning = 0;
            OnLoadPhase(currentPhase);
        }

        public void Update()
        {
            currentPhaseTimeRunning += GameController.deltaTime;
            currentPhaseTime = currentPhaseTime - GameController.deltaTime;
            if (currentPhaseTime <= 0)
            {
                SwitchPhase();
            }
        }

        public void SwitchPhase()
        {
            int currentPhaseInt = (int) currentPhase;
            ePhase previousPhase = currentPhase;
            currentPhaseInt++;
            if (currentPhaseInt > (int) ePhase.PHASE_NIGHT)
            {
                currentPhaseInt = (int)ePhase.PHASE_MORNING;
                currentDay++;
            }
            currentPhase = (ePhase) currentPhaseInt;
            currentPhaseTime = phaseTimes[currentPhase];
            currentPhaseTimeRunning = 0;

            OnUnloadPhase(previousPhase);
            OnLoadPhase(currentPhase);
        }

        public void OnUnloadPhase(ePhase phase)
        {

        }

        public void OnLoadPhase(ePhase phase)
        {
            // FIXME
            // DAY PHASE CHANGED EVENT
            /*
                public void OnLoadPhase(phase){
                    print("Current phase " .. phase);
                    GameController.eventSystem:Event("DAY_PHASE_CHANGED",;
                    {
                    current = phase,;
                    color=this.phaseColors[phase],;
                    ambientColor=this.phaseAmbientColors[phase],;
                    })
             */
        }
    }
}