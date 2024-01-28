using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{


    private bool isPaused = false;

    [Header("UI Screens")]
    public GameObject pauseJournal;
    public GameObject mainUI;
    public AudioClip openJournal;

    [Header("Inventory Icons")]
    public Image cameraSlot;
    public Image flashlightSlot;


    [Header("Clown Tracking")]
    public float pointerThreshold = 27.5f;
    public RectTransform pointerTransform;
    public GameObject player;
    public Transform clownTransform;
    public GameObject trackingArrow;
    public GameObject trackingHint;
    public float hintTimer = 7.5f;

    private float distToClown;
    private AudioSource audioSource;


    [Header("Objectives")]
    public TextMeshProUGUI objectiveHint;
    private FlashlightController flashlightController;



    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        mainUI.SetActive(true);
        pauseJournal.SetActive(false);

        trackingArrow.SetActive(false);
        trackingHint.SetActive(false); 

        flashlightController = player.GetComponent<FlashlightController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        JournalEntries();
        InventorySlots();
        PauseGame();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ClownTracking();
        }

    }

    void InventorySlots()
    {


        Color camColour = cameraSlot.color;
        Color flashColour = flashlightSlot.color;

        if (flashlightController.IsHoldingFlashlight)
        {
            camColour.a = 0.45f;
            cameraSlot.color = camColour; 
            flashColour.a = 1f;
            flashlightSlot.color = flashColour;
        }
        else
        {
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
            audioSource.PlayOneShot(openJournal);

            isPaused = !isPaused;
            if (isPaused)
            {
                player.SetActive(false);
                mainUI.SetActive(false);
                pauseJournal.SetActive(true);

                Time.timeScale = 0f;
            }
            else
            {
                player.SetActive(true);
                mainUI.SetActive(true);
                pauseJournal.SetActive(false);

                Time.timeScale = 1.0f;
            }
        }
    }

    void JournalEntries()
    {
        if (GameManager.Instance == null) return;

        if (GameManager.Instance.ObjectiveProgress >= 1)
        {
            objectiveHint.text = "If no one believes this, then everyone's a fool.";
        }
        else if (GameManager.Instance.ObjectiveProgress >= 0.75f)
        {
            objectiveHint.text = "I've gotten more than enough. I need to get out";
        }
        else if (GameManager.Instance.ObjectiveProgress >= 0.5f)
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

    public void OnClickExit()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");

    }

    public void OnLeftWoods()
    {
        Time.timeScale = 1.0f;
    }

    public void ClownTracking()
    {
        Vector3 clownPos = clownTransform.position;
        Vector3 startPos = player.transform.position;

        Vector3 dirToClown = (clownPos - startPos).normalized;
        distToClown = (clownPos - startPos).magnitude;

        Debug.Log(distToClown);

        if (distToClown <= pointerThreshold)
        {
            Debug.Log("Clown is near");
            trackingHint.SetActive(true);
            trackingHint.GetComponent<TextMeshProUGUI>().text = "It's near.";

            StartCoroutine(DisableCloseHint());

        }
        else
        {
            Debug.Log("Clown is far");

            trackingArrow.SetActive(true);
            trackingHint.SetActive(true);

            trackingHint.GetComponent<TextMeshProUGUI>().text = "A noise from there.";

            float angle = Mathf.Atan2(dirToClown.z, dirToClown.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360;

            pointerTransform.localEulerAngles = new Vector3(0, 0, angle);

            StartCoroutine(DisableFarHint());

        }

    }

    IEnumerator DisableCloseHint()
    {
        yield return new WaitForSeconds(hintTimer);

        trackingHint.SetActive(false);
    }

    IEnumerator DisableFarHint()
    {
        yield return new WaitForSeconds(hintTimer);

        trackingArrow.SetActive(false);
        trackingHint.SetActive(false);
    }


}
