using System.Collections;
using System.Collections.Generic;
using AuraHull.AuraVRGame;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reload : MonoBehaviour
{
    [SerializeField] private bool reload = false;

    void Update()
    {
        if (reload == true)
        {
            reload = false;
            AuraSceneManager.Instance.SceneReset();
        }
    }
}
