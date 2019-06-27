using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatStopper : MonoBehaviour
{
    [SerializeField] private BoatDriver driver;
    [SerializeField] private string[] stopperColliderNames;

    private void OnCollisionEnter(Collision collision)
    {
        foreach (string n in stopperColliderNames)
        {
            if (collision.gameObject.name == n)
            {
                driver.ForceStopThrottle();
                return;
            }
        }
    }
}
