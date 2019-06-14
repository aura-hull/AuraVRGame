using System.Collections;
using System.Collections.Generic;
using AuraHull.AuraVRGame;
using Photon.Pun;
using UnityEngine;

public class BuildingSite : MonoBehaviour
{
    [SerializeField] private BuildPartMatcher[] partMatchers;
    [SerializeField] GameObject _objectToBecome;
    [SerializeField] Material _holoMaterial;
    [SerializeField] Material _filledMaterial;

    private int _partsBuiltCount = 0;

    void Awake()
    {
        NetworkController.OnTurbinePartBuilt += ConstructPartNetworked;
        
        for (int i = 0; i < partMatchers.Length; i++)
        {
            if (partMatchers[i] != null)
            {
                Renderer partRend = partMatchers[i].GetComponent<Renderer>();
                if (partRend != null)
                {
                    partRend.material = _holoMaterial;
                }
            }
        }
    }

    public void BuildPart(BuildPartMatcher matcher)
    {
        if (!ConstructPart(matcher.Index)) return;

        NetworkController.Instance.NotifyTurbinePartBuilt(matcher.Index);
    }

    private void ConstructPartNetworked(int actorNumber, int matcherIndex)
    {
        if (actorNumber == PhotonNetwork.LocalPlayer.ActorNumber) return;
        ConstructPart(matcherIndex);
    }

    private bool ConstructPart(int matcherIndex)
    {
        BuildPartMatcher target = null;
        foreach (BuildPartMatcher m in partMatchers)
        {
            if (m.Index == matcherIndex)
            {
                target = m;
                break;
            }
        }

        if (target == null) return false;

        Renderer partRenderer = target.GetComponent<Renderer>();
        if (partRenderer == null) return false;

        target.enabled = false;
        partRenderer.material = _filledMaterial;
        
        if (++_partsBuiltCount >= partMatchers.Length)
        {
            NetworkController.Instance.NotifyTurbineBuilt(gameObject.GetPhotonView().ViewID, _objectToBecome.name);
        }

        return true;
    }
}
