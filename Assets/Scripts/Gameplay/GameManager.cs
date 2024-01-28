using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameManager : MonoBehaviour
{
    public float Score { get; protected set; } = 0f;
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this);
    }

    public abstract void WinGame();
    public abstract void LoseGame();
    public abstract void UpdateScore();
}
