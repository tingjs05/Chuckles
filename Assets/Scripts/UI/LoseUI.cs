using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseUI : MonoBehaviour
{
    [Header("UI Objects")]
    public GameObject caughtUI;
    public GameObject loseUI;
    public GameObject clown;
    public GameObject laughingUIPrefab;
    public GameObject letsPlayText;
    public Transform laughParent;
    public GameObject ambientNoise;

    [Header("SFX")]
    private AudioSource audioSource;
    public AudioClip laughSound;


    [Header("Instantiation parameters")]
    public int rowCount = 16;
    public int columnCount = 10;
    public float spacingX = 110f;
    public float spacingY = 110f;

    private const float displayDelay = 0.0001f;

    // Start is called before the first frame update

    void Start()
    {
        Time.timeScale = 1.0f;

        // destroy gamemanager
        if (GameManager.Instance != null) Destroy(GameManager.Instance.gameObject);

        caughtUI.SetActive(true);
        loseUI.SetActive(false);

        audioSource = GetComponent<AudioSource>();

        clown.SetActive(false);
        letsPlayText.SetActive(false);


        StartCoroutine(InstantiateUIPrefabs());
    }

    IEnumerator InstantiateUIPrefabs()
    {
        
        ambientNoise.SetActive(false);
        clown.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        Vector3 startingPos = Vector3.zero;

        //play one shot sound LOL
        audioSource.PlayOneShot(laughSound);

        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < columnCount; col++)
            {
                Vector3 position = startingPos + new Vector3(col * spacingX, row * spacingY, 0f);


                GameObject instantiatedUI = Instantiate(laughingUIPrefab, position, Quaternion.identity);

                instantiatedUI.transform.SetParent(laughParent);

                yield return new WaitForSeconds(displayDelay);
            }
        }

        yield return new WaitForSeconds(0.5f);


        letsPlayText.SetActive(true);

        yield return new WaitForSeconds(3f);

        caughtUI.SetActive(false);

        yield return new WaitForSeconds(1.5f);

        loseUI.SetActive(true);

    }

    public void OnClickRestart()
    {
        Debug.Log("Restarting Level");
        SceneManager.LoadScene("GameLevel");
    }

    public void OnClickMainMenu()
    {
        Debug.Log("Returning to Main Menu");
        SceneManager.LoadScene("MainMenu");
    }

}
