// Any area of effect cast via a Talent, damaging or otherwise
using UnityEngine;

namespace Dethrone.Emissions
{
    public class AreaOfEffect : MonoBehaviour
    {
        // Requisite components
        private Animator animator;

        private bool dealsKnockback;
        protected float damage;

        // Status vars
        protected bool isActive;
        protected bool isTelegraphing;
        protected bool isAffecting;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            animator.SetBool("isActive", isActive);
            animator.SetBool("isAffecting", isAffecting);
            animator.SetBool("isTelegraphing", isTelegraphing);
        }

        // Call on instantiation to pass Talent behaviour to this AOE
        public virtual void Initialize(float damage, GameObject owner)
        {
            this.damage = damage;
            if (owner.tag == "Player")
            {
                gameObject.layer = 9;
            }
            else
            {
                gameObject.layer = 15;
            }
        }

        public virtual void SetStatus(bool isActive, bool isTelegraphing, bool isAffecting)
        {
            this.isActive = isActive;
            this.isTelegraphing = isTelegraphing;
            this.isAffecting = isAffecting;
        }

        // Turn this AOE into a trigger
        public void EnableAOE(bool isTrigger)
        {
            transform.GetComponent<Collider2D>().isTrigger = isTrigger;
        }

        // Called when an attached trigger collider enters another collider
        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            OnHitActor(collision.gameObject);
        }

        // AOE behaviour on hitting an actor
        protected virtual void OnHitActor(GameObject gameObject)
        {
            Actor actor = gameObject.GetComponent<Actor>();
            if (actor != null)
            {
                actor.OnHit();
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

        // Destroy this area of effect
        public void DestroyAOE()
        {
            Destroy(transform.gameObject);
        }
    }

}
