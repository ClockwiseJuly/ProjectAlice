using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : TimeControlled
{
    float moveSpeed = 5f;
    float jumpVelocity = 40f;


    // Start is called before the first frame update
    void Start()
    {

    }

    public override void TimeUpdate()
    {
        base.TimeUpdate();

        Vector2 pos = transform.position;

        pos.y += velocity.y * Time.deltaTime;
        velocity.y += TimeController.gravity * Time.deltaTime;

        if (pos.y < 1)
        {
            pos.y = 1;
            velocity.y = 0;
        }

        // if (Input.GetKeyDown(KeyCode.W))
        if (Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = jumpVelocity;
        }

        if (Input.GetKey(KeyCode.A))
        {
            pos.x -= moveSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            pos.x += moveSpeed * Time.deltaTime;
        }

        transform.position = pos;
    }
}
