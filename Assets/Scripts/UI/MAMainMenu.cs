using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MAMainMenu : MonoBehaviour
{
    public Canvas menuCanvas;
    public bool isMainMenu;


    public AudioSource clickSound;


    void Update()
    {
        bool current = menuCanvas.gameObject.activeSelf;
        if (Input.GetKeyDown(KeyCode.Escape) && !isMainMenu)
        {
            menuCanvas.gameObject.SetActive(!current);
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
        Cursor.lockState = CursorLockMode.None;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void playClickSound() {
        this.clickSound.PlayOneShot(this.clickSound.clip);
    }
}
