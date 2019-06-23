using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDHandler : MonoBehaviour
{
    public Text dodgeChargeCounter;
    public Text healthCounter;
    public Text tonicCounter;
    public Text manaCounter;

    public GameObject player;

    private DodgeController dodgeScript;
    private Health playerHealth;
    private Tonic playerTonic;
    private Mana manaScript;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = player.GetComponent<Health>();
        dodgeScript = player.GetComponent<DodgeController>();
        playerTonic = player.GetComponent<Tonic>();
        manaScript = player.GetComponent<Mana>();
    }

    // Update is called once per frame
    private void Update()
    {
        healthCounter.text = $"Health: {playerHealth._health:F0}";
        dodgeChargeCounter.text = "Dodges: " + dodgeScript.DodgeCharges;
        tonicCounter.text = "Tonics: " + playerTonic.TonicCount;
        manaCounter.text = $"Mana: {manaScript._mana:F0}";
    }
}
