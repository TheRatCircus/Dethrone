// Handler for in-game character menu
using UnityEngine;
using System.Collections;

public class CharacterMenu : MonoBehaviour
{
    public GameObject characterMenu;
    public GameObject pauseMenu;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("CharacterMenu"))
        {
            ToggleCharacterMenu();
        }
    }

    public void ToggleCharacterMenu()
    {
        if (characterMenu.activeSelf)
        {
            characterMenu.SetActive(false);
        }
        else
        {
            characterMenu.SetActive(true);
        }
    }
}
