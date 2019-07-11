// Projectile emitted by Impel
using UnityEngine;

namespace Dethrone.Emissions
{
    public class Impel : Projectile
    {
        // Awake is called when the script instance is being loaded
        protected override void Awake()
        {
            base.Awake();
            lifetime = 2;
        }

        // Start is called before the first frame update
        private void Start()
        {
            GetComponent<Rigidbody2D>().AddForce(direction * speed);
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
    }
}

