// Projectile emitted by Impel
using UnityEngine;
using System.Collections;

namespace Dethrone.Emissions
{
    public class Impel : Projectile
    {
        protected float noFallTime;

        // Awake is called when the script instance is being loaded
        protected override void Awake()
        {
            base.Awake();
            noFallTime = .7f;
            lifetime = 2;
        }

        // Start is called before the first frame update
        private void Start()
        {
            rb2d = GetComponent<Rigidbody2D>();
            rb2d.gravityScale = 0;
            rb2d.AddForce(direction * speed);
            StartCoroutine(NoFallTime());
        }

        // Update is called once per frame
        private void Update()
        {
            transform.Rotate(0, 0, 16, Space.Self);
        }

        // Called when an attached trigger collider enters another collider
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);
            Destroy(Instantiate(onDeathEffect, (Vector2)transform.position, new Quaternion()), .5f);
            DestroyProjectile();
        }

        IEnumerator NoFallTime()
        {
            yield return new WaitForSeconds(noFallTime);
            rb2d.gravityScale = 2;
        }
    }
}

