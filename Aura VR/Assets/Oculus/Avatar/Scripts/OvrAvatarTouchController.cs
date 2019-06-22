using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OvrAvatarTouchController : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);

        TutorialManager.Instance.OnTutorialStart += () => { gameObject.SetActive(true); };
        TutorialManager.Instance.OnTutorialEnd += () => { gameObject.SetActive(false); };
    }
}
