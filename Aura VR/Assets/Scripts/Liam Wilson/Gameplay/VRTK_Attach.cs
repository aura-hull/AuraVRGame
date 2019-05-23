using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using VRTK;

namespace AuraHull.AuraVRGame
{
    public class VRTK_Attach : MonoBehaviour
    {
        [SerializeField]
        private bool lockRotation = false;

        private Transform localPosition = null;

        void Awake()
        {
            VRTK_SDKManager.instance.AddBehaviourToToggleOnLoadedSetupChange(this);
        }

        void Start()
        {
            Transform vrtkActiveRig = VRTK_DeviceFinder.PlayAreaTransform();
            VRTK_SDKSetup vrtkSetup = vrtkActiveRig.GetComponentInParent<VRTK_SDKSetup>();
            localPosition = vrtkSetup.actualHeadset.transform;

            if (localPosition == null)
            {
                DestroyImmediate(this.gameObject);
            }
            else
            {
                Debug.Log("AuraHull.AuraVRGame.VRTK_Avatar : Avatar linked!");
                transform.SetParent(localPosition, false);
            }
        }

        void Update()
        {
            if (lockRotation)
            {
                transform.rotation = Quaternion.identity;
            }
        }

        void OnDestroy()
        {
            VRTK_SDKManager.instance.RemoveBehaviourToToggleOnLoadedSetupChange(this);
        }
    }
}