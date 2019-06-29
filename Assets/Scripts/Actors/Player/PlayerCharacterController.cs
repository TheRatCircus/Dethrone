using UnityEngine;

// Central handling for all player input
public class PlayerCharacterController : Actor
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

    // Status vars
    private bool isJumping;

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

        canCharacterAction = true;
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

        if (canCharacterAction)
        {
            MoveInput();
            JumpInput();
            DodgeInput();
            TonicInput();
            TalentInput();
        }
    }

    // Catch input for horizontal movement
    public void MoveInput()
    {
        playerMovementController.SetMove(Input.GetAxis("Horizontal"));
    }

    // Catch input activating a jump
    public void JumpInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            playerMovementController.SetJumping(Input.GetButtonDown("Jump"));
        }
        else
        {
            playerMovementController.SetJumping(Input.GetButtonUp("Jump"));
        }
    }

    // Catch input activating dodge
    public void DodgeInput()
    {
        (int dodgeX, int dodgeY) dodgeAxes;

        // Check vertical first
        if (Input.GetAxis("Vertical") > 0)
        {
            dodgeAxes.dodgeY = 1;
        }
        else if (Input.GetAxis("Vertical") < 0)
        {
            dodgeAxes.dodgeY = -1;
        }
        else
        {
            dodgeAxes.dodgeY = 0;
        }

        // If not dodging vertically, default is backstep
        if (dodgeAxes.dodgeY == 0)
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                dodgeAxes.dodgeX = 1;
            }
            else
            {
                dodgeAxes.dodgeX = -1;
            }
        }
        // If dodging vertically, allow a non-horizontal dodge
        else
        {
            if (Input.GetAxis("Horizontal") > 0)
            {
                dodgeAxes.dodgeX = 1;
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                dodgeAxes.dodgeX = -1;
            }
            else
            {
                dodgeAxes.dodgeX = 0;
            }
        }
        playerDodge.SetDodgeDirection(dodgeAxes);
        if (Input.GetButtonDown("Dodge"))
        {
            playerDodge.TryDodge();
        }
    }

    // Catch input activating tonic
    public void TonicInput()
    {
        if (Input.GetButtonDown("Tonic"))
        {
            tonic.ConsumeTonic();
        }
    }

    // Send talent input to TalentController
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
