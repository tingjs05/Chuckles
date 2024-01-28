using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Enemy;

public class LevelOneGameManager : GameManager
{
    [SerializeField] private DayCycle dayCycle;

    private float score1, score2, score3 = -1f;

    void Start()
    {
        // subscribe to win event
        dayCycle.DayEnded += WinGame;

        // subscribe to lose event
        EnemyStateMachine.KilledPlayer += LoseGame;

        // subscribe to objective events
        EnemyActionPlaygroundState.CapturedWhileInPlayground += UpdateObjectiveScore1;
        EnemyActionCampsiteState.CapturedWhileInCampsite += UpdateObjectiveScore2;
        EnemyChaseState.CapturedWhileChasing += UpdateObjectiveScore3;
    }

    public override void WinGame()
    {
        UpdateScore();
        SceneManager.LoadScene("Win");
    }

    public override void LoseGame()
    {
        UpdateScore();
        SceneManager.LoadScene("Lose");
    }

    // score
    public override void UpdateScore()
    {
        Score = Mathf.Round((score1 + score2 + score3) / 3);
    }

    // objective score update listeners
    void UpdateObjectiveScore1(float pictureQuality)
    {
        if (pictureQuality > score1) score1 = pictureQuality;
        UpdateScore();
    }

    void UpdateObjectiveScore2(float pictureQuality)
    {
        if (pictureQuality > score2) score2 = pictureQuality;
        UpdateScore();
    }

    void UpdateObjectiveScore3(float pictureQuality)
    {
        if (pictureQuality > score3) score3 = pictureQuality;
        UpdateScore();
    }
}
