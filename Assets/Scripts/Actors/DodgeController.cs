using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeController : MonoBehaviour
{
    public GameObject dodgeCrosshair;
    private CrosshairCollide collideHandler;
    private TargettingController targettingController;

    private int dodgeCharges;
    private int dodgeChargesMax;
    public int DodgeCharges { get => dodgeCharges; }
    public int DodgeChargesMax { get => dodgeChargesMax; set => dodgeChargesMax = value; }

    private bool isRecovering;

    private void Start()
    {
        collideHandler = dodgeCrosshair.GetComponent<CrosshairCollide>();
        targettingController = GetComponent<TargettingController>();

        dodgeChargesMax = 3;
        dodgeCharges = dodgeChargesMax;
    }

    private void Update()
    {
        MoveCrosshair();
    }

    public void Dodge()
    {
        if (!collideHandler.GetIsColliding() && dodgeCharges > 0)
        {
            GetComponent<Rigidbody2D>().position = dodgeCrosshair.transform.position;
            dodgeCharges--;
            if (!isRecovering)
            {
                StartCoroutine("RecoverCharges");
            }
        }
    }

    IEnumerator RecoverCharges()
    {
        isRecovering = true;
        while (dodgeCharges < dodgeChargesMax)
        {
            yield return new WaitForSeconds(1.0f);
            dodgeCharges++;
        }
        isRecovering = false;
    }

    private void MoveCrosshair()
    {
        dodgeCrosshair.transform.position = targettingController.AimRay.GetPoint(3.0f);
    }
}