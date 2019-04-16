using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField]
    PartSpawner partSpawner;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        partSpawner.ItemPickedUp();
        // other.getComponent<parts>().GiveParts()
        gameObject.SetActive(false);
    }    
}
