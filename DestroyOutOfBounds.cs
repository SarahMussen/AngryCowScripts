using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DestroyOutOfBounds : NetworkBehaviour
{

    private float bound = -6.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    
    // Update is called once per frame
    void Update()
    {
        //if the poop is behind the farmer, destroy the poop
        if (transform.position.z <= bound)
        {
            GameManager.Instance.addMissedPoop();
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }
}
