using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dethrone.Emissions
{
    public class Impel : Projectile
    {
        private void Start()
        {
            GetComponent<Rigidbody2D>().AddForce(direction * speed);
        }

        private void Update()
        {
            transform.Rotate(0, 0, 16, Space.Self);
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            DestroyProjectile();
        }
    }
}

