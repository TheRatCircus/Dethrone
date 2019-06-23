using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCameraController : MonoBehaviour
{
    public GameObject target;
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            Vector3 targetPos = cam.transform.position;
            targetPos.x = target.transform.position.x;
            targetPos.y = target.transform.position.y;
            targetPos.z = -10f;
            cam.transform.position = targetPos;
        }
    }
}
