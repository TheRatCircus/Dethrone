using System.Collections;
using System.Collections.Generic;
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
        private float damage;

        // Status vars
        private bool isActive;
        private bool isTelegraphing;
        private bool isAffecting;
        public bool IsActive { get => isActive; set => isActive = value; }
        public bool IsTelegraphing { get => isTelegraphing; set => isTelegraphing = value; }
        public bool IsAffecting { get => isAffecting; set => isAffecting = value; }

        void Awake()
        {
            gameObject.layer = 9;
        }

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

        public virtual void Initialize(float damage)
        {
            this.damage = damage;
        }

        public virtual void SetStatus(bool isActive, bool isTelegraphing, bool isAffecting)
        {
            this.isActive = isActive;
            this.isTelegraphing = isTelegraphing;
            this.isAffecting = isAffecting;
        }

        public void SetAOEIsTrigger(bool isTrigger)
        {
            transform.GetComponent<Collider2D>().isTrigger = isTrigger;
        }

        private void OnTriggerEnter2D(Collider2D collision)
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

        // Destroy this area of effect
        public void DestroyAOE()
        {
            Destroy(transform.gameObject);
        }
    }

}
