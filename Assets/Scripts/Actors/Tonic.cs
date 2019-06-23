// Controls the player's healing Tonic system
using UnityEngine;
using System.Collections;

public class Tonic : MonoBehaviour
{
    // Requisite scripts
    private Health health;
    private Actor actorController;

    // Numeric fields
    private int tonicCount;
    public int TonicCount { get => tonicCount; set => tonicCount = value; }

    private float delayTime;
    private float healAmount;
    private float healTime;

    private bool tonicActive;

    // Start is called before the first frame update
    void Start()
    {
        health = GetComponent<Health>();
        actorController = GetComponent<Actor>();

        tonicCount = 3;
        delayTime = 1f;

        healAmount = 25f;
        healTime = 3f;
    }

    private void Update()
    {
        if (tonicActive)
        {
            health.ModifyHealth((healAmount / healTime) * Time.deltaTime);
        }
    }

    // Attempt to consume a Tonic, checking if possible
    public void TryConsumeTonic()
    {
        if (tonicCount > 0)
        {
            ConsumeTonic();
        }
    }

    // Consume a tonic
    public void ConsumeTonic()
    {
        tonicCount--;
        StartCoroutine(TonicHeal());
    }

    // Run the tonic heal time
    public IEnumerator TonicHeal()
    {
        actorController.CanCharacterAction = false;
        yield return new WaitForSeconds(delayTime);
        actorController.CanCharacterAction = true;

        tonicActive = true;
        yield return new WaitForSeconds(healTime);
        tonicActive = false;
    }
}
