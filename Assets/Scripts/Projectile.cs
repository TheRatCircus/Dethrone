using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : PhysicsObject
{
    public float lifetime;
    private float uptime;
    public float projSpeed;

    // Start is called before the first frame update
    void Start()
    {
        isGravitating = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (uptime < lifetime)
        {
            transform.Translate(Time.deltaTime * projSpeed, 0, 0, Space.Self);
            uptime = uptime + (Time.deltaTime);
        } else
        {
            Destroy(transform.gameObject);
        }
    }
}
