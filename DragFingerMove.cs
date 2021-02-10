using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DragFingerMove : NetworkBehaviour
{
    [SerializeField]
    CustomNetworkManager networkManager;

    private Vector3 touchPosition;
    private Rigidbody2D rigidbody;
    private Vector3 direction;
    private float moveSpeed = 5f;

    // Start is called before the first frame update
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            // the ScreenToWorldPoint needs a Vector3 with the same z-value as the Camera's position.
            touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 20f));
            if (touchPosition.y >= 1)
            {
                direction = (touchPosition - transform.position);
                rigidbody.velocity = new Vector2(direction.x, direction.y) * moveSpeed;
            }
            else
            {
                rigidbody.velocity = Vector2.zero;
            }
            networkManager.setShootDirection(transform.position);


            if (touch.phase == TouchPhase.Ended)
            {
                rigidbody.velocity = Vector2.zero;
            }
        }
    }
}
