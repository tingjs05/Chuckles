using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnClickPlay()
    {
        Debug.Log("Play clicked");
        SceneManager.LoadScene("Introduction");
    }

    public void OnClickQuit()
    {
        Debug.Log("chuckles will be sad");
        Application.Quit();
    }
}
