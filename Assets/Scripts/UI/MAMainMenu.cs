using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MAMainMenu : MonoBehaviour
{
    public GameObject menuCanvas;
    public bool isMainMenu;


    public AudioSource clickSound;


    void Update()
    {
        bool current = menuCanvas.activeSelf;
        if (Input.GetKeyDown(KeyCode.Escape) && !isMainMenu)
        {
            menuCanvas.SetActive(!current);
            if (current)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    public void PlayGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("quit.");
        Application.Quit();
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void playClickSound() {
        this.clickSound.PlayOneShot(this.clickSound.clip);
    }
}
