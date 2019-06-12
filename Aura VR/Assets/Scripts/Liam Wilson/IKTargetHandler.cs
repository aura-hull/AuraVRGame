using System;
using System.Collections;
using System.Collections.Generic;
using AuraHull.AuraVRGame;
using Photon.Pun;
using RootMotion.FinalIK;
using UnityEngine;
using VRTK;
using Object = System.Object;

[RequireComponent(typeof(PhotonView))]
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

    private PhotonView _photonView = null;

    void Awake()
    {
        vrtkManager.LoadedSetupChanged += OnLoadedSetupChanged;
    }

    void Start()
    {
        _photonView = GetComponent<PhotonView>();
        NetworkController.OnIKHandlesSet += OnIKHandlesSet;
    }

    // Only happens locally because of the event subscription.
    public void OnLoadedSetupChanged(VRTK_SDKManager sender, VRTK_SDKManager.LoadedSetupChangeEventArgs e)
    {
        if (photonTrackedObjectPrefab == null) return;

        int positionIndex = (int)PhotonNetwork.LocalPlayer.CustomProperties["position"];

        VRTK_SDKSetup setup = sender.loadedSetup;
        if (setup == null) return;

        // Instantiate IK targets.
        if (headTarget == null)
        {
            GameObject headTargetObj = PhotonNetwork.Instantiate(photonTrackedObjectPrefab.name, Vector3.zero, Quaternion.identity);
            headTarget = headTargetObj.GetPhotonView();
        }

        headTarget.transform.SetParent(setup.actualHeadset.transform);
        headTarget.transform.localPosition = headTargetPosition;
        headTarget.transform.localEulerAngles = headTargetEuler;

        if (leftTarget == null)
        {
            GameObject leftTargetObj = PhotonNetwork.Instantiate(photonTrackedObjectPrefab.name, Vector3.zero, Quaternion.identity);
            leftTarget = leftTargetObj.GetPhotonView();
        }

        leftTarget.transform.SetParent(setup.actualLeftController.transform);
        leftTarget.transform.localPosition = leftTargetPosition;
        leftTarget.transform.localEulerAngles = leftTargetEuler;

        if (rightTarget == null)
        {
            GameObject rightTargetObj = PhotonNetwork.Instantiate(photonTrackedObjectPrefab.name, Vector3.zero, Quaternion.identity);
            rightTarget = rightTargetObj.GetPhotonView();
        }

        rightTarget.transform.SetParent(setup.actualRightController.transform);
        rightTarget.transform.localPosition = rightTargetPosition;
        rightTarget.transform.localEulerAngles = rightTargetEuler;

        // Set local names.
        SetLocalNames(positionIndex);

        // Set IK links.
        if (finalIKSetup == null) return;
        finalIKSetup.solver.spine.headTarget = headTarget.transform;
        finalIKSetup.solver.leftArm.target = leftTarget.transform;
        finalIKSetup.solver.rightArm.target = rightTarget.transform;

        NetworkController.Instance.NotifyIKHandlesSet(positionIndex, headTarget.ViewID, leftTarget.ViewID, rightTarget.ViewID);
    }

    public void OnIKHandlesSet(int positionIndex, int headPunId, int leftPunId, int rightPunId)
    {
        if (_photonView.Controller.IsLocal) return;
        
        headTarget = PhotonView.Find(headPunId);
        leftTarget = PhotonView.Find(leftPunId);
        rightTarget = PhotonView.Find(rightPunId);

        // Set local names.
        SetLocalNames(positionIndex);

        // Set IK links.
        if (finalIKSetup == null) return;
        finalIKSetup.solver.spine.headTarget = headTarget.transform;
        finalIKSetup.solver.leftArm.target = leftTarget.transform;
        finalIKSetup.solver.rightArm.target = rightTarget.transform;
    }

    private void SetLocalNames(int positionIndex)
    {
        headTarget.name = $"HeadTarget ({positionIndex})";
        leftTarget.name = $"LeftTarget ({positionIndex})";
        rightTarget.name = $"RightTarget ({positionIndex})";
    }

    void OnDestroy()
    {
        vrtkManager.LoadedSetupChanged -= OnLoadedSetupChanged;
    }
}
