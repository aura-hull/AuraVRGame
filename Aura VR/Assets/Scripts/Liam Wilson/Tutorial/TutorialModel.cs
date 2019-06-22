using System;
using System.Collections;
using System.Collections.Generic;
using AuraHull.AuraVRGame;
using Photon.Pun;
using UnityEngine;

public class TutorialModel : MonoBehaviour, IPunObservable
{
    [SerializeField] private bool localTest = false;

    public Speaker speaker { get; private set; }
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
        speaker = GetComponent<Speaker>();
        _animator = GetComponent<PenguinAnimationControl>();

        TutorialManager.Instance.tutorialModel = this;

        speaker.OnDialogueFinish += NetworkController.Instance.NotifyTutorialClientReady;
        speaker.OnFullCycle += TutorialManager.Instance.EndTutorial;

        if (PhotonNetwork.IsMasterClient)
        {
            speaker.OnDialogueFinish += TutorialManager.Instance.CheckNextTutorialCondition;
        }

        NetworkController.OnTutorialClientReady += OnClientReady;
        NetworkController.OnPlayNextTutorial += SpeakWhenReady;
    }

    public void Initialize()
    {
        //ResetTutorial();

        TutorialManager.Instance.CheckNextTutorialCondition();
        NetworkController.Instance.NotifyTutorialClientReady();
    }

    private void SpeakWhenReady(int nextSpeakIndex)
    {
        StartCoroutine(Coroutine_SpeakWhenReady(nextSpeakIndex));
    }

    private IEnumerator Coroutine_SpeakWhenReady(int nextSpeakIndex)
    {
        while (_clientsReady < RequiredClients)
            yield return null;

        speaker.Speak(nextSpeakIndex);
    }

    private void OnClientReady()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _clientsReady++;
        }
    }

    public void ResetTutorial()
    {
        _animator.speaking = false;
        speaker.currentDialogue = 0;
    }

    private float checkVolumeTicks = 0.0f;
    void Update()
    {
        checkVolumeTicks += Time.deltaTime;
        if (checkVolumeTicks >= checkVolumeStep)
        {
            _animator.speaking = (speaker.GetCurrentLoudness() >= 0.005f);
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
