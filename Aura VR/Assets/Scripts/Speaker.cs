using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Speaker : MonoBehaviour
{
    public List<Dialogue> _dialogues;
    public int _currentDialogue;

    public Action OnDialogueStart;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource source = GetComponent<AudioSource>();
        source.loop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Speak();
        }
    }

    public void Speak()
    {
        if (_currentDialogue >= _dialogues.Count)
            _currentDialogue = 0;

        Play(_currentDialogue);
        _currentDialogue += 1;
    }

    public void SpeakByName(string name)
    {
        for (int i = 0; i < _dialogues.Count; i += 1)
        {
            if (_dialogues[i].Name == name)
            {
                Play(i);
            }
        }
    }

    private void Play(int index)
    {
        AudioSource source = GetComponent<AudioSource>();
        source.clip = _dialogues[index].Audio;
        source.Play();
        OnDialogueStart?.Invoke();
    }
}


[Serializable]
public class Dialogue
{
    [SerializeField]
    private string _name;
    [SerializeField]
    private AudioClip _audio;

    public string Name
    {
        get
        {
            return _name;
        }
        set
        {
            _name = value;
        }
    }
    public AudioClip Audio
    {
        get
        {
            return _audio;
        }
        set
        {
            _audio = value;
        }
    }

    public Dialogue(string name, AudioClip audio)
    {
        Name = name;
        Audio = audio;
    }
}
