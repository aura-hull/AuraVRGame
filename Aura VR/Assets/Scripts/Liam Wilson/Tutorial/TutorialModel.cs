using System;
using System.Collections;
using System.Collections.Generic;
using AuraHull.AuraVRGame;
using Photon.Pun;
using UnityEngine;

public class TutorialModel : MonoBehaviour, IPunObservable
{
    [SerializeField] private bool localTest = false;

    private Speaker _speaker;
    private PenguinAnimationControl _animator;

    private int clientsReady = 0;
    private int RequiredClients
    {
        get
        {
            if (localTest) return 1;
            else return NetworkController.MAX_PLAYERS;
        }
    }

    private float checkVolumeStep = 0.1f;

    void Start()
    {
        _speaker = GetComponent<Speaker>();
        _animator = GetComponent<PenguinAnimationControl>();

        Initialize();
    }

    public void Initialize()
    {
        TutorialManager.Instance.tutorialModel = this;

        _speaker.OnDialogueFinish += CheckNextTutorialCondition;

        NetworkController.OnTutorialClientProgress += TutorialClientProgress;
        NetworkController.OnTutorialClientProgressAll += TutorialClientProgressAll;

        _speaker.OnFullCycle += TutorialManager.Instance.EndTutorial;

        CheckNextTutorialCondition();
    }

    private float checkVolumeTicks = 0.0f;
    void Update()
    {
        checkVolumeTicks += Time.deltaTime;
        if (checkVolumeTicks >= checkVolumeStep)
        {
            _animator.speaking = (_speaker.GetCurrentLoudness() >= 0.005f);
            checkVolumeTicks = 0.0f;
        }

        if (!localTest) return;

        if (clientsReady >= RequiredClients)
        {
            _speaker.Speak();
            clientsReady = 0;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (PhotonNetwork.IsMasterClient && stream.IsWriting)
        {
            stream.SendNext(clientsReady);
        }
        else if (!PhotonNetwork.IsMasterClient)
        {
            clientsReady = (int)stream.ReceiveNext();
        }

        if (clientsReady >= RequiredClients)
        {
            _speaker.Speak();
            clientsReady = 0;
        }
    }

    public void TutorialClientProgress()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        clientsReady++;
    }

    public void TutorialClientProgressAll()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        clientsReady = int.MaxValue;
    }

    public void Finish()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void CheckNextTutorialCondition()
    {
        foreach (TutorialCondition condition in TutorialManager.Instance.specialConditions)
        {
            if (condition.tutorialIndex == _speaker.currentDialogue)
            {
                if (condition.wasTriggeredEarly)
                {
                    NetworkController.Instance.NotifyClientProgressAll();
                    return;
                }

                condition.SetLive();
                return;
            }
        }

        // Default conditions
        NetworkController.Instance.NotifyClientProgress();
    }
}
