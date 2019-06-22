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

    private int _clientsReady = 0;
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

        TutorialManager.Instance.tutorialModel = this;

        if (PhotonNetwork.IsMasterClient)
        {
            _speaker.OnDialogueFinish += TutorialManager.Instance.CheckNextTutorialCondition;
        }

        _speaker.OnDialogueFinish += NetworkController.Instance.NotifyTutorialClientReady;
        _speaker.OnFullCycle += TutorialManager.Instance.EndTutorial;

        NetworkController.OnTutorialClientReady += OnClientReady;
        NetworkController.OnPlayNextTutorial += SpeakWhenReady;
    }

    public void Initialize()
    {
        ResetTutorial();

        NetworkController.Instance.NotifyTutorialClientReady(_speaker.currentDialogue);
        TutorialManager.Instance.CheckNextTutorialCondition(_speaker.currentDialogue);
    }

    private void SpeakWhenReady()
    {
        StartCoroutine(Coroutine_SpeakWhenReady());
    }

    private IEnumerator Coroutine_SpeakWhenReady()
    {
        while (_clientsReady < RequiredClients)
            yield return null;

        _speaker.Speak();
    }

    private void OnClientReady(int dialogueIndex)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _clientsReady++;
        }
    }

    public void ResetTutorial()
    {
        _animator.speaking = false;
        _speaker.currentDialogue = 0;
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
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (stream.IsWriting)
                stream.SendNext(_clientsReady);
        }
        else
        {
            if (stream.IsReading)
                _clientsReady = (int)stream.ReceiveNext();
        }
    }
}
