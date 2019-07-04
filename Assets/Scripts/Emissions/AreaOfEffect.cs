using UnityEngine;

// Any area of effect cast via a Talent, damaging or otherwise
namespace Dethrone.Emissions
{
    public class AreaOfEffect : MonoBehaviour
    {
        // Requisite components
        private Animator animator;

        // Boolean properties
        private bool dealsKnockback;

        // Numeric properties
        protected float damage;

        // Status vars
        private bool isActive;
        private bool isTelegraphing;
        private bool isAffecting;
        public bool IsActive { get => isActive; set => isActive = value; }
        public bool IsTelegraphing { get => isTelegraphing; set => isTelegraphing = value; }
        public bool IsAffecting { get => isAffecting; set => isAffecting = value; }

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
        public virtual void Initialize(float damage, bool ownedByPlayer)
        {
            this.damage = damage;
            if (ownedByPlayer)
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

        public void EnableAOE(bool isTrigger)
        {
            transform.GetComponent<Collider2D>().isTrigger = isTrigger;
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            OnHitActor(collision.gameObject);
        }

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
