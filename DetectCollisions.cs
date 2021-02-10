using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mirror;

public class DetectCollisions : NetworkBehaviour
{

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
        Debug.Log("colission detected");
        GameManager.Instance.addCathedPoop();
        //Destroy(other.gameObject);
        gameObject.SetActive(false);
    }

}
