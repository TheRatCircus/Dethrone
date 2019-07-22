// Base class for all cast Area of Effects
using UnityEngine;

namespace Dethrone.Emissions
{
    public class AreaOfEffect : MonoBehaviour
    {
        // Requisite objects
        private Animator animator;

        bool alternate;

        // Talent behaviour
        private bool dealsKnockback;
        protected float damage;

        // Status vars
        protected int aoeStatus;
        public int _aoeStatus { get => aoeStatus; set => aoeStatus = value; }

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            animator.SetBool("alternate", alternate);
        }

        // Update is called once per frame
        void Update()
        {
            animator.SetInteger("aoeStatus", aoeStatus);
        }

        // Call on instantiation to pass Talent behaviour to this AOE
        public virtual void Initialize(float damage, GameObject owner, bool alternate)
        {
            this.damage = damage;
            this.alternate = alternate;
            gameObject.layer = (owner.tag == "Player" ? 9 : 15);
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
    }

}
