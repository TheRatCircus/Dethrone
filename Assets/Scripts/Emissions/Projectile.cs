using UnityEngine;

namespace Dethrone.Emissions
{
    public class Projectile : MonoBehaviour
    {
        protected float lifetime;
        protected float uptime;

        protected float speed;
        protected Vector2 direction;

        protected float damage;

        bool isSpinning;

        protected virtual void Awake()
        {
            gameObject.layer = 9;
        }

        // Call on instantiation to pass Talent behaviour to this projectile
        public virtual void Initialize(float damage)
        {
            this.damage = damage;
        }

        public virtual void Initialize(float damage, Vector2 direction, float speed)
        {
            this.damage = damage;
            this.direction = direction;
            this.speed = speed;
        }

        public void SetProjectileIsTrigger(bool isTrigger)
        {
            transform.GetComponent<Collider2D>().isTrigger = isTrigger;
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            transform.GetComponent<Collider2D>().isTrigger = false;
            if (collision.tag == "Enemy")
            {
                DamageActor(collision.GetComponent<Health>());
            }
        }

        private void DamageActor(Health health)
        {
            health.ModifyHealth(-damage);
        }

        // Destroy this projectile
        public void DestroyProjectile()
        {
            Destroy(transform.gameObject);
        }
    }
}
