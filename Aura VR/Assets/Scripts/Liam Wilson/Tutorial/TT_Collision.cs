using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TT_Collision : TutorialTrigger
{
    [SerializeField] private string searchForName = "";

    void OnEnable()
    {
        GetComponent<Collider>().enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!PhotonNetwork.IsMasterClient) return;

        bool nameMatch = other.name.Contains(searchForName);
        if (nameMatch == false && other.attachedRigidbody != null)
        {
            nameMatch = other.attachedRigidbody.name.Contains(searchForName);
        }

        if (nameMatch)
        {
            Trigger();
        }
    }

    void OnDisable()
    {
        GetComponent<Collider>().enabled = false;
    }
}
