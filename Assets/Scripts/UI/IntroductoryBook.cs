using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroductoryBook : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnClickStart()
    {
        SceneManager.LoadScene("GameLevel");
    }
}
