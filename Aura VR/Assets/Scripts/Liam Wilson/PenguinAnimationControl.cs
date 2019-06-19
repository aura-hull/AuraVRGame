using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinAnimationControl : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string speakTriggerName = "Speak";
    [SerializeField] private string[] randomTriggerNames;
    [SerializeField, Range(3.0f, 60.0f)] private float randomMinFrequency;
    [SerializeField, Range(3.0f, 60.0f)] private float randomMaxFrequency;

    public bool speak = false;

    private float timeSinceLastRandom = 0.0f;
    private float timeUntilNextRandom;
    private int lastRandom = -1;

    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }

        timeUntilNextRandom = Random.Range(randomMinFrequency, randomMaxFrequency);
    }

    void Update()
    {
        if (speak)
        {
            speak = false;
            animator.SetTrigger("Speak");
        }

        if (randomTriggerNames != null)
        {
            timeSinceLastRandom += Time.deltaTime;

            if (timeSinceLastRandom >= timeUntilNextRandom)
            {
                timeSinceLastRandom = 0.0f;
                timeUntilNextRandom = Random.Range(randomMinFrequency, randomMaxFrequency);

                int newRandom = Random.Range(0, randomTriggerNames.Length);
                if (newRandom == lastRandom)
                {
                    newRandom = (newRandom + 1) % randomTriggerNames.Length;
                }
                lastRandom = newRandom;

                animator.SetTrigger(randomTriggerNames[newRandom]);
            }
        }
    }
}
