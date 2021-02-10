using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MoveInDirection : NetworkBehaviour
{

    public float speed = 10f;

    private Vector3 shootDirection;
    private Vector3 targetPosition;

    public void SetShootDirection(Vector3 direction)
    {
        shootDirection = direction;
    }

    void Start()
    {
        targetPosition = new Vector3(shootDirection.x, shootDirection.y, -6);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }
}
