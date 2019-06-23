//
using UnityEngine;

public class Tonic : MonoBehaviour
{
    private int tonicCount;
    public int TonicCount { get => tonicCount; set => tonicCount = value; }

    private Health health;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        tonicCount = 3;
    }

    //
    public void ConsumeTonic()
    {
        if (tonicCount > 0)
        {
            tonicCount--;
            health.modHealth(25);
        }
    }
}
