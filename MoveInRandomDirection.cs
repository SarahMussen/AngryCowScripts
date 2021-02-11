using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MoveInRandomDirection : NetworkBehaviour
{

    public float speed = 10f;


    private float targetRangeMinX = -5;
    private float targetRangeMaxX = 5;
    private float targetRangeMinY = 5;
    private float targetRangeMaxY = 14;
    private float targetPosZ = -10;

    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = new Vector3(Random.Range(targetRangeMinX, targetRangeMaxX), Random.Range(targetRangeMinY, targetRangeMaxY), targetPosZ);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Vector3.forward*Time.deltaTime*speed);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }
}
