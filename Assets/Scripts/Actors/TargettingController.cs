// Controls actor targetting for Talents
using UnityEngine;

public class TargettingController : MonoBehaviour
{
    private UnityEngine.Object inputSource;
    private Vector2 sourcePointer;

    // Global pointer
    private Vector2 globalPointer;
    public Vector2 GlobalPointer { get => globalPointer; }

    // Point-cast pointer
    private Vector2 pointCastPointer;
    public Vector2 PointCastPointer { get => pointCastPointer; }
    private float pointCastPointerMin;
    private float pointCastPointerMax;

    // Ray oriented to direction of player aim and relevant vars
    private Ray2D aimRay;
    public Ray2D AimRay { get => aimRay; }
    private Vector2 aimDirection;
    public Vector2 AimDirection { get => aimDirection; set => aimDirection = value; }
    private Quaternion aimRotation;
    public Quaternion AimRotation { get => aimRotation; }

    // First area of ground beneath the point-cast pointer
    private Vector2 groundPointer;
    public Vector2 GroundPointer { get => groundPointer; }

    // Use this for initialization
    void Start()
    {
        globalPointer = Vector2.zero;
        pointCastPointer = Vector2.zero;
        pointCastPointerMin = 3.0f;
        pointCastPointerMax = 5.0f;
    }

    // Update is called once per frame
    void Update()
    {
        HandleGroundPointer();
        HandleAimRay();
        HandleAimDirection();
        HandleAimRotation();
        HandlePointCastPointer();
    }

    // Receive outside input of actor target point. Call every frame
    public void CatchPointer(Vector2 pointerPos)
    {
        globalPointer = pointerPos;
    }

    // Handle point-cast pointer
    private void HandlePointCastPointer()
    {
        pointCastPointer = globalPointer;
        float distance = Vector2.Distance(pointCastPointer, transform.localPosition);

        if (distance > pointCastPointerMax)
        {
            Vector2 fromOriginToObject = pointCastPointer - (Vector2)transform.localPosition;
            fromOriginToObject *= pointCastPointerMax / distance;
            pointCastPointer = (Vector2)transform.localPosition + fromOriginToObject;
        }
        else if (distance < pointCastPointerMin)
        {
            Vector2 fromOriginToObject = pointCastPointer - (Vector2)transform.localPosition;
            fromOriginToObject *= pointCastPointerMin / distance;
            pointCastPointer = (Vector2)transform.localPosition + fromOriginToObject;
        }
    }

    // Handle ground pointer
    private void HandleGroundPointer()
    {
        LayerMask terrainMask = LayerMask.GetMask("Terrain");
        RaycastHit2D groundPoint = Physics2D.Raycast(pointCastPointer, Vector2.down, 10f, terrainMask);
        if (groundPoint)
        {
            groundPointer = groundPoint.point;
        }
    }

    // Handle aimRay
    private void HandleAimRay()
    {
        aimRay.origin = transform.position;
        aimRay.direction = globalPointer - (Vector2)transform.position;
    }

    // Handle aimDirection
    private void HandleAimDirection()
    {
        aimDirection = (aimRay.GetPoint(1) - (Vector2)transform.position).normalized;
    }

    // Handle aimRotation
    private void HandleAimRotation()
    {
        float aimAngle = Mathf.Atan2(aimRay.direction.y, aimRay.direction.x) * Mathf.Rad2Deg;
        aimAngle = (aimAngle < 0 ? aimAngle += 360f : aimAngle);
        aimRotation = Quaternion.Euler(0, 0, aimAngle);
    }

    // Get a point in the actor's orbit, given a range
    public Vector2 GetOrbitPoint(float orbitRange)
    {
        return aimRay.GetPoint(orbitRange);
    }

    // Debug Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(globalPointer, 0.2f);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(aimRay.origin, aimRay.direction);
        Gizmos.DrawSphere(aimDirection, 0.2f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(GroundPointer, 0.2f);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(GetOrbitPoint(1.5f), 0.2f);
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(pointCastPointer, 0.1f);
    }
}
