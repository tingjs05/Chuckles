using Dictation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MicManager : MonoBehaviour
{
    
    public Image micImg;
    public LaughListener laughListener;

    private bool isEnabled;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleMic()
    {
        isEnabled = !isEnabled;
        if (!isEnabled)
        {
            laughListener.DestroyListener();

            micImg.color = Color.red;

        }
        else
        {
            laughListener.StartListener();

            micImg.color = Color.green;


        }
    }

}
