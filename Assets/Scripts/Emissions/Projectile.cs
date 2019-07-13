// Base class for all projectiles
using UnityEngine;

namespace Dethrone.Emissions
{
    public class Projectile : MonoBehaviour
    {
        public GameObject onDeathEffect;
        public Rigidbody2D rb2d;

        protected float lifetime;
        public float Lifetime { get => lifetime; }

        protected float speed;
        protected Vector2 direction;

        protected float damage;

        bool isSpinning;

        // Awake is called when the script instance is being loaded
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

        // Turn this projectile into a trigger
        public void EnableProjectile(bool isTrigger)
        {
            transform.GetComponent<Collider2D>().isTrigger = isTrigger;
        }

        // Called when an attached trigger collider enters another collider
        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            OnHitActor(collision.gameObject);
        }

        // Projectile behaviour upon hitting an actor
        protected virtual void OnHitActor(GameObject gameObject)
        {
            Actor actor = gameObject.GetComponent<Actor>();
            if (actor != null)
            {
                actor.OnHit(transform.position);
            }

            Health health = gameObject.GetComponent<Health>();
            if (health != null)
            {
                DamageActor(health);
            }
        }

        // Deal damage to an actor, usually on hit
        protected void DamageActor(Health health)
        {
            health.ChangeHealth(-damage);
        }

        // Destroy this projectile
        public void DestroyProjectile()
        {
            Destroy(transform.gameObject);
        }
    }
}
