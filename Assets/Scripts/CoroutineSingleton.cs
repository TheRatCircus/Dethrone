// Global singleton used to start coroutines in non-MonoBehaviour scripts
using UnityEngine;

public class CoroutineSingleton : MonoBehaviour
{
    public static CoroutineSingleton instance;

    private void Awake()
    {
        instance = this;
    }
}
