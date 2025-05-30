using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMover : TimeControlled
{
    public override void TimeUpdate()
    {
        Vector2 pos = transform.position;
        pos.x += 1 * Time.deltaTime;
        transform.position = pos;
    }
}
