using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    private bool isPaused = false;

    [Header("UI")]
    public GameObject pauseJournal;
    public GameObject mainUI;


    // Start is called before the first frame update
    void Start()
    {
        mainUI.SetActive(true);
        pauseJournal.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        PauseGame();
    }

    void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                mainUI.SetActive(false);
                pauseJournal.SetActive(true);

                Time.timeScale = 0f;
            }
            else
            {
                mainUI.SetActive(true);
                pauseJournal.SetActive(false);

                Time.timeScale = 1.0f;
            }
        }
    }
   
    public void OnClickResume()
    {
        if (isPaused)
        {
            isPaused = false;
            mainUI.SetActive(true);
            pauseJournal.SetActive(false);

            Time.timeScale = 1.0f;
        }
    }

}
