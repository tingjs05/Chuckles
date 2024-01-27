using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [Header("Controls")]
    public KeyCode ActivateItem = KeyCode.Mouse0;
    public KeyCode SwitchItem = KeyCode.F;

    public static PlayerControls Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }
}
