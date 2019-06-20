using System;
using System.Collections;
using System.Collections.Generic;
using AuraHull.AuraVRGame;
using Photon.Pun;
using UnityEngine;

public class TutorialModel : MonoBehaviour
{
    [SerializeField] private GameObject penguinPrefab;
    [SerializeField] private Transform spawnPoint;

    public List<TutorialCondition> specialConditions;

    private Speaker _penguinSpeaker;
    private PenguinAnimationControl _penguinAnimator;
    private bool _isInitialized = false;

    void Awake()
    {
        AuraGameManager.Instance.tutorialModel = this;
        specialConditions = new List<TutorialCondition>();
    }

    public void Initialize()
    {
        if (_isInitialized) return;

        NetworkController.OnTutorialStarted += SetReferences;
        NetworkController.OnTutorialSpeakNext += PlayNextSpeaker;

        if (PhotonNetwork.IsMasterClient)
        {
            if (penguinPrefab == null)
            {
                Finish();
                return;
            }

            if (spawnPoint == null)
            {
                spawnPoint = transform;
            }

            GameObject penguin = PhotonNetwork.InstantiateSceneObject(penguinPrefab.name, spawnPoint.position, spawnPoint.rotation);
            NetworkController.Instance.NotifyTutorialStarted(penguin.GetPhotonView().ViewID);
        }

        _isInitialized = true;
    }

    private void Finish()
    {
        AuraGameManager.Instance.StartGameplay();

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(_penguinSpeaker.gameObject);
        }
    }

    public void SetupNextTutorialCondition()
    {
        for (int i = 0; i < specialConditions.Count; i++)
        {
            if (specialConditions[i].tutorialIndex == _penguinSpeaker.currentDialogue)
            {
                if (specialConditions[i].wasTriggeredEarly)
                {
                    NetworkController.Instance.NotifyTutorialSpeakNext();
                    return;
                }

                specialConditions[i].SetLive();
                return;
            }
        }

        // Default conditions
        NetworkController.Instance.NotifyTutorialSpeakNext();
    }

    public void PlayNextSpeaker()
    {
        _penguinSpeaker.Speak();
    }

    private void SetReferences(int penguinViewId)
    {
        GameObject penguin = PhotonView.Find(penguinViewId).gameObject;

        _penguinSpeaker = penguin.GetComponent<Speaker>();
        _penguinAnimator = penguin.GetComponent<PenguinAnimationControl>();

        if (_penguinSpeaker == null)
        {
            Finish();
            return;
        }

        _penguinSpeaker.OnFullCycle += Finish;

        if (PhotonNetwork.IsMasterClient)
        {
            _penguinSpeaker.OnDialogueFinish += SetupNextTutorialCondition;
            NetworkController.Instance.NotifyTutorialSpeakNext();
        }
    }
}
