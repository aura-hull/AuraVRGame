using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using RootMotion.FinalIK;
using UnityEngine;
using VRTK;
using Object = System.Object;

public class IKTargetHandler : MonoBehaviour
{
    private static Vector3 headTargetPosition = new Vector3(0, -0.07827437f, -0.1492147f);
    private static Vector3 headTargetEuler = new Vector3(0, -90, -90);

    private static Vector3 leftTargetPosition = new Vector3(-0.03f, -0.03f, -0.1f);
    private static Vector3 leftTargetEuler = new Vector3(90, 0, -90);

    private static Vector3 rightTargetPosition = new Vector3(0.03f, -0.03f, -0.1f);
    private static Vector3 rightTargetEuler = new Vector3(-90, 0, 90);

    [SerializeField] private VRTK_SDKManager vrtkManager;
    [SerializeField] private VRIK finalIKSetup;
    [SerializeField] private GameObject photonTrackedObjectPrefab;

    public PhotonView headTarget { get; private set; } = null;
    public PhotonView leftTarget { get; private set; } = null;
    public PhotonView rightTarget { get; private set; } = null;

    public Action OnIKTargetsSetup = null;

    void Awake()
    {
        vrtkManager.LoadedSetupChanged += OnLoadedSetupChanged;
    }

    public void OnLoadedSetupChanged(VRTK_SDKManager sender, VRTK_SDKManager.LoadedSetupChangeEventArgs e)
    {
        if (photonTrackedObjectPrefab == null) return;

        VRTK_SDKSetup setup = sender.loadedSetup;
        if (setup == null) return;

        // Instantiate IK targets.
        if (headTarget == null)
        {
            GameObject headTargetObj = PhotonNetwork.Instantiate(photonTrackedObjectPrefab.name, Vector3.zero, Quaternion.identity);
            headTarget = headTargetObj.GetPhotonView();
        }
        
        if (leftTarget == null)
        {
            GameObject leftTargetObj = PhotonNetwork.Instantiate(photonTrackedObjectPrefab.name, Vector3.zero, Quaternion.identity);
            leftTarget = leftTargetObj.GetPhotonView();
        }
        
        if (rightTarget == null)
        {
            GameObject rightTargetObj = PhotonNetwork.Instantiate(photonTrackedObjectPrefab.name, Vector3.zero, Quaternion.identity);
            rightTarget = rightTargetObj.GetPhotonView();
        }

        // Setup local values
        SetLocalValues(setup);

        // Set up IK links.
        if (finalIKSetup == null) return;

        finalIKSetup.solver.spine.headTarget = headTarget.transform;
        finalIKSetup.solver.leftArm.target = leftTarget.transform;
        finalIKSetup.solver.rightArm.target = rightTarget.transform;

        OnIKTargetsSetup?.Invoke();

        //VRTK_SDKManager.UnsubscribeLoadedSetupChanged(OnLoadedSetupChanged);
        //DestroyImmediate(this.gameObject);
    }

    private void SetLocalValues(VRTK_SDKSetup setup)
    {
        headTarget.name = "HeadTarget";
        headTarget.transform.SetParent(setup.actualHeadset.transform);
        headTarget.transform.localPosition = headTargetPosition;
        headTarget.transform.localEulerAngles = headTargetEuler;

        leftTarget.name = "LeftTarget";
        leftTarget.transform.SetParent(setup.actualLeftController.transform);
        leftTarget.transform.localPosition = leftTargetPosition;
        leftTarget.transform.localEulerAngles = leftTargetEuler;

        rightTarget.name = "RightTarget";
        rightTarget.transform.SetParent(setup.actualRightController.transform);
        rightTarget.transform.localPosition = rightTargetPosition;
        rightTarget.transform.localEulerAngles = rightTargetEuler;
    }

    void OnDestroy()
    {
        vrtkManager.LoadedSetupChanged -= OnLoadedSetupChanged;
    }
}
