using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialConditionTrigger : TutorialCondition
{
    [SerializeField] private string searchForName = "";

    void OnEnable()
    {
        GetComponent<Collider>().enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains(searchForName) || 
            (other.attachedRigidbody != null && 
             other.attachedRigidbody.name.Contains(searchForName)))
        {
            if (canTriggerEarly)
            {
                wasTriggeredEarly = true;
            }

            if (live)
            {
                OnConditionMet?.Invoke();
                live = false;
            }
        }
    }

    void OnDisable()
    {
        GetComponent<Collider>().enabled = false;
    }
}
