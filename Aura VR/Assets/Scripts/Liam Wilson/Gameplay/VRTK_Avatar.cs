using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using VRTK;

namespace AuraHull.AuraVRGame
{
    public class VRTK_Avatar : MonoBehaviour
    {
        [SerializeField]
        private bool lockXRotation = false;

        private Transform avatarNeck = null;

        void Awake()
        {
            VRTK_SDKManager.instance.AddBehaviourToToggleOnLoadedSetupChange(this);
        }

        void OnEnable()
        {
            Transform vrtkActiveRig = VRTK_DeviceFinder.PlayAreaTransform();
            VRTK_SDKSetup vrtkSetup = vrtkActiveRig.GetComponentInParent<VRTK_SDKSetup>();
            
            switch (vrtkSetup.systemSDKInfo.description.prettyName)
            {
                case "Oculus Rift (Standalone:Oculus)":
                    try
                    {
                        OvrAvatar oculusAvatar = vrtkSetup.GetComponentInChildren<OvrAvatar>();
                        avatarNeck = oculusAvatar.Base.transform;
                    }
                    catch (MissingComponentException)
                    {
                        Debug.LogError("AuraHull.AuraVRGame.VRTK_Avatar : Unable to find Oculus body!");
                    }
                    break;

                case "SteamVR (Standalone:SteamVR)":
                    Debug.LogError("AuraHull.AuraVRGame.VRTK_Avatar : Not implemented (SteamVR)");
                    break;

                case "Simulator (Standalone)":
                    avatarNeck = vrtkSetup.actualHeadset.transform.parent.transform;
                    break;
            }

            if (avatarNeck == null)
            {
                DestroyImmediate(this.gameObject);
            }
            else
            {
                transform.SetParent(avatarNeck, false);
                Debug.Log("AuraHull.AuraVRGame.VRTK_Avatar : Avatar linked! " + avatarNeck.name);
            }
        }

        void Update()
        {
            transform.eulerAngles = new Vector3(
                lockXRotation ? 0.0f : transform.parent.eulerAngles.y,
                transform.parent.eulerAngles.y,
                transform.parent.eulerAngles.z
            );
        }

        void OnDestroy()
        {
            VRTK_SDKManager.instance.RemoveBehaviourToToggleOnLoadedSetupChange(this);
        }
    }
}