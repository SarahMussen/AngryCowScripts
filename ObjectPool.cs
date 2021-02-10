using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ObjectPool : NetworkBehaviour
{
    //public GameObject objectPrefab;
    public CustomNetworkManager networkManager;

    public int poolSize;
    private List<GameObject> objectPool = new List<GameObject>();

    MoveInDirection moveInDirectionController;
    MoveInRandomDirection moveInRandomDirectionController;

    void Start()
    {
        //fill the pool with objects
        for (int x = 0; x < poolSize; x++)
        {
            CreateNewObject();
        }
    }

    //instantiate an object, set it to not active and add it to the pool
    private GameObject CreateNewObject()
    {
        GameObject obj = Instantiate(networkManager.spawnPrefabs[2]);

        moveInDirectionController = obj.GetComponent<MoveInDirection>();
        moveInDirectionController.enabled = false;

        moveInRandomDirectionController = obj.GetComponent<MoveInRandomDirection>();
        moveInRandomDirectionController.enabled = true;

        obj.SetActive(false);
        objectPool.Add(obj);

        return obj;
    }

    //find an object that's not active
    //make a new one if there are none
    //set it to active
    public GameObject GetObject()
    {
        GameObject obj = objectPool.Find(x => x.activeInHierarchy == false);

        if (obj == null)
        {
            obj = CreateNewObject();
        }
        obj.SetActive(true);

        return obj;
    }
}
