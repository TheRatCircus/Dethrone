using System;
using UnityEngine;

// Central handling for all player input
public class PlayerInputHandler : MonoBehaviour
{
    public Camera cam;
    private Vector2 pointerPosition;
    public Vector2 PointerPosition { get => pointerPosition; }

    // Scripts receiving input
    private TargettingController targettingController;
    private DodgeController playerDodge;
    private TalentController playerTalentController;
    private LandMovementController playerMovementController;
    private Tonic tonic;

    // Use this for initialization
    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }

        targettingController = GetComponent<TargettingController>();
        playerDodge = GetComponent<DodgeController>();
        playerTalentController = GetComponent<TalentController>();
        playerMovementController = GetComponent<LandMovementController>();
        tonic = GetComponent<Tonic>();
    }

    // Update is called once per frame
    void Update()
    {
        // Handle global pointer with...
        // Mouse
        if (Input.GetJoystickNames().Length == 0)
        {
            pointerPosition = cam.ScreenToWorldPoint((Vector2)Input.mousePosition);
        }
        // Joystick
        else
        {
            Vector2 newPointerPos = pointerPosition += new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            pointerPosition += newPointerPos;
        }
        targettingController.CatchPointer(pointerPosition);
        // TODO: Handle movement
        if (Input.GetButtonDown("Jump"))
        {

        }
        else if (Input.GetButtonUp("Jump"))
        {

        }
        // Dodge handling
        if (Input.GetButtonDown("Dodge"))
        {
            playerDodge.Dodge();
        }
        // Tonic handling
        if (Input.GetButtonDown("Tonic"))
        {
            tonic.ConsumeTonic();
        }
        // Talent input handling
        TalentInput();
    }

    // On cast input, check what axes are also being input
    public void TalentInput()
    {
        bool secondaryInput = false;

        if (Input.GetButtonDown("Secondary"))
        {
            secondaryInput = true;
        }

        if (Input.GetButtonDown("Talent1"))
        {
            playerTalentController.TryCast(0, secondaryInput);
        }
        else if (Input.GetButtonDown("Talent2"))
        {
            playerTalentController.TryCast(1, secondaryInput);
        }
        else if (Input.GetButtonDown("Talent3"))
        {
            playerTalentController.TryCast(2, secondaryInput);
        }
        else if (Input.GetButtonDown("Talent4"))
        {
            playerTalentController.TryCast(3, secondaryInput);
        }
    }
}
