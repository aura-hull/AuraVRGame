using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinDialogueManager : MonoBehaviour
{
    [SerializeField]
    Animator _penguinAnim;
    [SerializeField]
    GameObject _penguinModel;
    [SerializeField]
    GameObject _auraModel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GoToAuraCenter(string id)
    {
        _penguinAnim.SetBool(id,true);
    }

}
