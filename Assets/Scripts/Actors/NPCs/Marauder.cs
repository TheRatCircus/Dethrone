// Fast melee enemy with a leaping attack
using System.Collections;
using UnityEngine;
using Dethrone.Talents;

// State of this NPC
public enum NPCState
{
    None,
    Peaceful,
    Patrolling,
    WaitingForPlayer,
    Combat
}

// State of this NPC in combat
public enum CombatState
{
    None,
    Idle,
    Moving,
    Attacking
}

namespace Dethrone.NPCs
{
    public class Marauder : Actor
    {
        // Requisite scripts
        protected TargettingController targettingController;
        protected LandMovementController movementController;

        protected Slash slash;

        public GameObject target;
        public float awareDistance;
        public float attackRange;

        protected NPCState npcState;
        protected CombatState combatState;

        protected bool isTelegraphing;
        protected bool isCasting;

        // Awake is called when the script instance is being loaded
        protected void Awake()
        {
            npcState = NPCState.WaitingForPlayer;
            combatState = CombatState.None;
        }

        // Start is called before the first frame update
        new void Start()
        {
            
            movementController = GetComponent<LandMovementController>();
            targettingController = GetComponent<TargettingController>();

            if (target == null)
            {
                target = GameObject.FindGameObjectWithTag("Player");
            }

            slash = ScriptableObject.CreateInstance<Slash>();
            slash.Initialize(targettingController, gameObject);

            StartCoroutine(ScanForTarget());
        }

        // Update is called once per frame
        void Update()
        {
            if (npcState == NPCState.Combat)
            {
                targettingController.CatchPointer(target.transform.position);
                
                if (combatState == CombatState.Moving)
                {
                    if (Vector2.Distance(target.transform.position, transform.position) < attackRange)
                    {
                        StartCoroutine(Attack());
                    }
                    else
                    {
                        movementController.SetMove(target.transform.position.x > transform.position.x ? 1 : -1);
                    }
                }
            }
        }

        // Check if the target is detected
        IEnumerator ScanForTarget()
        {
            while (npcState != NPCState.Combat)
            {
                yield return new WaitForSeconds(.25f);
                LayerMask layerMask = (1 << 10) | (1 << 13);

                float sqrDstToTarget = (target.transform.position - transform.position).sqrMagnitude;
                if (sqrDstToTarget <= Mathf.Pow(awareDistance, 2))
                {
                    RaycastHit2D hit = Physics2D.Raycast(transform.position, target.transform.position - transform.position, Mathf.Infinity, layerMask);
                    if (hit)
                    {
                        DetectTarget();
                    }
                }
            }
        }

        // Called when this actor is hit by an AoE or projectile
        public override void OnHit(Vector2 hitSrcPos)
        {
            base.OnHit(hitSrcPos);
            DetectTarget();
        }

        void DetectTarget()
        {
            npcState = NPCState.Combat;
            StartCoroutine(Wait());
        }

        // Execute an attack on the target
        IEnumerator Attack()
        {
            combatState = CombatState.Attacking;
            movementController.SetMove(0);
            StartCoroutine(slash.Cast());
            while (slash.IsActive)
            {
                yield return null;
            }
            StartCoroutine(Wait());
        }

        // Wait a delay of one second
        IEnumerator Wait()
        {
            combatState = CombatState.Idle;
            yield return new WaitForSeconds(1);
            combatState = CombatState.Moving;
        }
    }
}
