using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{


    private bool isPaused = false;

    [Header("UI Screens")]
    public GameObject pauseJournal;
    public GameObject mainUI;

    [Header("Inventory Icons")]
    public Image cameraSlot;
    public Image flashlightSlot;


    [Header("Clown Tracking")]
    public RectTransform pointerTransform;
    public Transform player;
    public Transform clownTransform;
    public float distToClown;

    [Header("Objectives")]
    public TextMeshProUGUI objectiveHint;
    public float completionPercentage;
    
    private FlashlightController flashlightController;



    // Start is called before the first frame update
    void Start()
    {
        mainUI.SetActive(true);
        pauseJournal.SetActive(false);
        flashlightController = player.GetComponent<FlashlightController>();


    }

    // Update is called once per frame
    void Update()
    {
        JournalEntries();
        InventorySlots();
        ClownTracking();
        PauseGame();
    }

    void InventorySlots()
    {


        Color camColour = cameraSlot.color;
        Color flashColour = flashlightSlot.color;

        if (flashlightController.IsHoldingFlashlight)
        {
            Debug.Log("Equipped flashlight");
            camColour.a = 0.45f;
            cameraSlot.color = camColour; 
            flashColour.a = 1f;
            flashlightSlot.color = flashColour;
        }
        else
        {
            Debug.Log("Equipped camera");
            camColour.a = 1f;
            cameraSlot.color = camColour;
            flashColour.a = 0.45f;
            flashlightSlot.color = flashColour;
        }
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

    void JournalEntries()
    {
        if (completionPercentage >= 1)
        {
            objectiveHint.text = "If no one believes this, then everyone's a fool.";
        }
        else if (completionPercentage >= 0.75f)
        {
            objectiveHint.text = "I've gotten more than enough. I need to get out";
        }
        else if (completionPercentage >= 0.5f)
        {
            objectiveHint.text = "It would be great if I've gotten more; But this can do.";
        }
        else
        {
            objectiveHint.text = "I need to prove this thing exists. Time to get some evidence.";
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

    void ClownTracking()
    {
        Vector3 clownPos = clownTransform.position;
        Vector3 startPos = player.position;

        Vector3 dirToClown = (clownPos - startPos).normalized;
        float angle = Mathf.Atan2(dirToClown.z, dirToClown.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;

        pointerTransform.localEulerAngles = new Vector3(0, 0, angle);

    }

}
