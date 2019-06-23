using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPCController : MonoBehaviour
{
    public GameObject target;
    private TargettingController targettingController;
    private LandMovementController movementController;

    Vector2 targetPosition;
    public Vector2 TargetPosition { get => targetPosition; set => targetPosition = value; }

    private void Start()
    {
        targettingController = GetComponent<TargettingController>();
        if (target == null)
        {
            target = GameObject.Find("PlayerCharacter");
        }
    }

    void Update()
    {
        targetPosition = target.transform.position;
        targettingController.CatchPointer(targetPosition);


    }
}
