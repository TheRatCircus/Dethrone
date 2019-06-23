using UnityEngine;

public class CrosshairCollide : MonoBehaviour
{
    private bool isColliding = false;
    
    public bool GetIsColliding()
    {
        return isColliding;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        isColliding = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        isColliding = false;
    }

}
