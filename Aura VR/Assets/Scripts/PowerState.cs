using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerState : MonoBehaviour
{
    [SerializeField]
    bool _powerState = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetPowerState(bool powered)
    {
        _powerState = powered;
    }
    public bool ReturnPowerState()
    {
        return _powerState;
    }
}
