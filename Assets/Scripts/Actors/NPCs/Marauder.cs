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
//public enum CombatState
//{
//    None,
//    Idle,
//    Moving,
//    Attacking
//}

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
        //protected CombatState combatState;

        protected bool isTelegraphing;
        protected bool isCasting;

        // How much horizontal difference between this and target is tolerated
        // to make this actor stand still?
        protected float positionalDiffTolerance = .4f;
        protected float shortDelay = .1f;
        protected float longDelay = 1;

        // Track the position of this actor's "eyes" so this actor can look
        // over low walls to see target
        protected Vector2 eyePosition;
        // The distance to the target squared. Used instead of Vector2.Distance
        // to save performance
        protected float sqrDstToTarget;

        // Awake is called when the script instance is being loaded
        protected void Awake()
        {
            npcState = NPCState.WaitingForPlayer;
            //combatState = CombatState.None;

            eyePosition = new Vector2();
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            movementController = GetComponent<LandMovementController>();
            targettingController = GetComponent<TargettingController>();

            if (target == null)
            {
                target = GameObject.FindGameObjectWithTag("Player");
            }

            slash = ScriptableObject.CreateInstance<Slash>();
            slash.Initialize(targettingController, gameObject);
            slash.TelegraphTime = .75f;

            StartCoroutine(ScanForTarget());
        }

        //Update is called once per frame
        void Update()
        {
            eyePosition = transform.position;
            eyePosition.y += .5f;

            if (npcState == NPCState.Combat)
            {
                targettingController.CatchPointer(target.transform.position);
            }

            sqrDstToTarget = (target.transform.position - transform.position).sqrMagnitude;
        }

        //Called when this actor is hit by an AoE or projectile
        public override void OnHit(Vector2 hitSrcPos)
        {
            base.OnHit(hitSrcPos);
            DetectTarget();
        }

        // Use square distancing to check if target is within a given distance
        private bool TargetWithinDistance(float distance) => sqrDstToTarget < Mathf.Pow(distance, 2);

        // Check if the target is detectable
        private IEnumerator ScanForTarget()
        {
            while (npcState != NPCState.Combat)
            {
                yield return new WaitForSeconds(.25f);

                LayerMask layerMask = LayerMask.GetMask("Terrain", "Objects", "Player");
                ContactFilter2D contactFilter = new ContactFilter2D();
                contactFilter.SetLayerMask(layerMask);

                if (TargetWithinDistance(awareDistance))
                {
                    RaycastHit2D[] results = new RaycastHit2D[16];
                    int noResults = Physics2D.Raycast(eyePosition, target.transform.position - transform.position, contactFilter, results, awareDistance);
                    for (int i = 0; i < noResults; i++)
                    {
                        if (results[i].collider.gameObject.tag != "Player")
                        {
                            break;
                        }
                        else if (results[i].collider.gameObject.tag == "Player")
                        {
                            DetectTarget();
                            break;
                        }
                        else
                        {
                            // Do nothing
                        }
                    }
                }
            }
        }

        // Detect the target, alerting this actor
        private void DetectTarget()
        {
            npcState = NPCState.Combat;
            StartCoroutine(Wait());
        }

        // Wait a delay
        private IEnumerator Wait()
        {
            yield return new WaitForSeconds(longDelay);
            StartCoroutine(ChooseAction());
        }

        // Decide the actor's next action
        private IEnumerator ChooseAction()
        {
            yield return new WaitForSeconds(shortDelay);

            if (TargetWithinDistance(attackRange))
            {
                StartCoroutine(Attack());
            }
            else
            {
                Move();
            }
        }

        // Move this actor towards the target
        void Move()
        {
            if (target.transform.position.x >= (transform.position.x - positionalDiffTolerance) &&
                target.transform.position.x <= (transform.position.x + positionalDiffTolerance))
            {
                movementController.SetMove(0);
                //If the target is within the same x but higher, try jumping
                if (!TargetWithinDistance(attackRange)
                    && target.transform.position.y > transform.position.y)
                {
                    movementController.StartCoroutine(movementController.ControlledJump());
                }
            }
            else if (target.transform.position.x > transform.position.x)
            {
                movementController.SetMove(1);
            }
            else if (target.transform.position.x < transform.position.x)
            {
                movementController.SetMove(-1);
            }
            
            StartCoroutine(ChooseAction());
        }

        // Execute an attack on the target
        IEnumerator Attack()
        {
            movementController.SetMove(0);
            StartCoroutine(slash.Cast());
            while (slash.IsActive)
            {
                yield return null;
            }
            StartCoroutine(Wait());
        }
    }
}
