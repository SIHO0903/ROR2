using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public void BtnContinue(GameObject gameObject)
    {
        Debug.Log("BtnContinue");
        CameraMovement.CursorOnOff(false);
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
    public void BtnOption()
    {
        Debug.Log("BtnOption");
        KeySoundSetManager.instance.gameObject.SetActive(true);
    }
    public void BtnToMainMenu()
    {
        Debug.Log("BtnToMainMenu");
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }
    public void BtnGameExit()
    {
        Time.timeScale = 1f;
        Debug.Log("BtnGameExit");
        Application.Quit();
    }
}
