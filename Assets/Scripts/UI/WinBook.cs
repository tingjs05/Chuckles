using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class WinBook : MonoBehaviour
{

    public TextMeshProUGUI pageContent;
    public TextMeshProUGUI gradeLetter;

    [Range(0f,1f)]public float replaceMe;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SetBook();
    }

    void SetBook()
    {
        if (replaceMe >= 1f)
        {
            gradeLetter.text = "A";
            pageContent.text = "The police were appalled that such a monster existed in those woods. They said they'll keep in touch. May my brother now rest in peace.";
        }
        else if (replaceMe >= 0.75f)
        {
            gradeLetter.text = "B";
            pageContent.text = "The police looked at the photos with a hint of doubt, but it convinced them enough to send some park rangers to find that thing. I hope this is enough.";
        }
        else if (replaceMe >= 0.5f)
        {
            gradeLetter.text = "C";
            pageContent.text = "The police looked at me as if I reported a missing pet. I doubt they'll even consider checking out those woods.";
        }
        else
        {
            gradeLetter.text = "D";
            pageContent.text = "The police looked at me as if I was some sort of lunatic and said that I was wasting my time. They wouldn't even consider taking a look at these photos.";
        }
    }

    public void OnClickRestart()
    {
        SceneManager.LoadScene("GameLevel");
    }

    public void OnClickMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


}
