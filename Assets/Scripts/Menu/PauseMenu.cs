using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        if (pauseMenu.activeSelf)
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1.0f;
        }
        else
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
