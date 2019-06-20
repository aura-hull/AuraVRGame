using System;
using System.Collections;
using System.Collections.Generic;
using AuraHull.AuraVRGame;
using Photon.Pun;
using UnityEngine;

public class TutorialModel : MonoBehaviour, IPunObservable
{
    private Speaker _speaker;
    private PenguinAnimationControl _animator;

    private int clientsReady = 0;

    void Start()
    {
        _speaker = GetComponent<Speaker>();
        _animator = GetComponent<PenguinAnimationControl>();

        Initialize();
    }

    public void Initialize()
    {
        _speaker.OnDialogueFinish += CheckNextTutorialCondition;
        NetworkController.OnTutorialClientProgress += TutorialClientProgress;

        CheckNextTutorialCondition();
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

        if (clientsReady >= 2)
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

    private void Finish()
    {
        AuraGameManager.Instance.StartGameplay();

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
                    NetworkController.Instance.NotifyClientProgress();
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
