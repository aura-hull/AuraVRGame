using System;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;
using VRTK;

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

    public GameObject headTarget { get; private set; } = null;
    public GameObject leftTarget { get; private set; } = null;
    public GameObject rightTarget { get; private set; } = null;

    public Action OnIKTargetsSetup = null;

    void Awake()
    {
        VRTK_SDKManager.SubscribeLoadedSetupChanged(OnLoadedSetupChanged);
    }

    public void OnLoadedSetupChanged(VRTK_SDKManager sender, VRTK_SDKManager.LoadedSetupChangeEventArgs e)
    {
        // Instantiate IK targets.
        VRTK_SDKSetup setup = sender.loadedSetup;
        if (setup == null) return;

        if (headTarget == null)
        {
            headTarget = new GameObject("HeadTarget");
            headTarget.transform.SetParent(setup.actualHeadset.transform);
            headTarget.transform.localPosition = headTargetPosition;
            headTarget.transform.localEulerAngles = headTargetEuler;
        }

        if (leftTarget == null)
        {
            leftTarget = new GameObject("LeftTarget");
            leftTarget.transform.SetParent(setup.actualLeftController.transform);
            leftTarget.transform.localPosition = leftTargetPosition;
            leftTarget.transform.localEulerAngles = leftTargetEuler;
        }

        if (rightTarget == null)
        {
            rightTarget = new GameObject("RightTarget");
            rightTarget.transform.SetParent(setup.actualRightController.transform);
            rightTarget.transform.localPosition = rightTargetPosition;
            rightTarget.transform.localEulerAngles = rightTargetEuler;
        }

        // Set up IK links.
        if (finalIKSetup == null) return;

        finalIKSetup.solver.spine.headTarget = headTarget.transform;
        finalIKSetup.solver.leftArm.target = leftTarget.transform;
        finalIKSetup.solver.rightArm.target = rightTarget.transform;

        OnIKTargetsSetup?.Invoke();

        //VRTK_SDKManager.UnsubscribeLoadedSetupChanged(OnLoadedSetupChanged);
        //DestroyImmediate(this.gameObject);
    }

    void OnDestroy()
    {
        VRTK_SDKManager.UnsubscribeLoadedSetupChanged(OnLoadedSetupChanged);
    }
}
