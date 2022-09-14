using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] Rigidbody player;
    [SerializeField] Transform centerPoint;
    [SerializeField] float speed = 4;

    public Vector3 movement;
    public bool dashPressed;

    private int delay = 0;

    
    void Start()
    {
        player = this.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        dashPressed = Input.GetKeyDown("space");
        transform.LookAt(centerPoint);
        transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y, transform.eulerAngles.z);
    }


    void FixedUpdate()
    {
        movePlayer(movement);
    }

    void movePlayer(Vector3 direction)
    {
        if (delay == 0)
        {
            
            if (dashPressed)
            {
                player.MovePosition(transform.position + (10f * direction * speed * Time.deltaTime));
                delay = 3;
            }
            else
            {
                player.MovePosition(transform.position + (direction * speed * Time.deltaTime));
            }
            
        }
        else
        {
            delay -= 1;
        }
    }
}
