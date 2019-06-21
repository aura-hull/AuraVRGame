using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Speaker : MonoBehaviour
{
    public List<Dialogue> _dialogues;
    public int currentDialogue;
    public bool autoPlay = false;

    public Action OnDialogueStart;
    public Action OnDialogueFinish;
    public Action OnFullCycle;

    private AudioSource _source;
    private bool isSpeaking = false;

    private int sampleDataLength = 256;
    private float[] clipSampleData;

    // Start is called before the first frame update
    void Start()
    {
        _source = GetComponent<AudioSource>();
        _source.loop = false;

        OnDialogueFinish += DialogueFinish;

        clipSampleData = new float[sampleDataLength];
    }

    void Update()
    {
        if (isSpeaking && !_source.isPlaying)
        {
            isSpeaking = false;
            OnDialogueFinish?.Invoke();
        }
    }

    public void Speak()
    {
        Play(currentDialogue++);
        isSpeaking = true;
    }

    private void Play(int index)
    {
        if (index < 0 || index >= _dialogues.Count) return;

        _source.Stop();
        _source.clip = _dialogues[index].Audio;
        _source.Play();

        OnDialogueStart?.Invoke();
    }
    
    private void DialogueFinish()
    {
        if (currentDialogue >= _dialogues.Count)
        {
            OnFullCycle?.Invoke();
            return;
        }

        if (autoPlay)
        {
            Speak();
        }
    }
    
    public float GetCurrentLoudness()
    {
        if (_source.clip == null) return 0.0f;

        _source.clip.GetData(clipSampleData, _source.timeSamples);

        float loudness = 0.0f;
        foreach (var sample in clipSampleData)
        {
            loudness += Mathf.Abs(sample);
        }

        return loudness / sampleDataLength;
    }
}


[Serializable]
public class Dialogue
{
    [SerializeField] private string _name;
    [SerializeField] private AudioClip _audio;

    public string Name
    {
        get { return _name; }
        set { _name = value; }
    }

    public AudioClip Audio
    {
        get {  return _audio; }
        set { _audio = value; }
    }

    public Dialogue(string name, AudioClip audio)
    {
        Name = name;
        Audio = audio;
    }
}
