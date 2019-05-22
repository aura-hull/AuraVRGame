using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AuraHull.AuraVRGame
{
    public abstract class BaseGameState
    {
        public virtual void InitState()
        {
            Debug.Log("State Changed to: " + this);
        }

        public virtual void ExecuteState() { }
        public virtual void FinishState() { }
    }
}