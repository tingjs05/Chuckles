using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Enemy;

public class LevelOneGameManager : GameManager
{
    [SerializeField] private DayCycle dayCycle;

    private float score1, score2, score3 = -1f;
    private bool completedObjective1, completedObjective2, completedObjective3 = false;

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
        UpdateObjectiveProgress();
        SceneManager.LoadScene("Win");
    }

    public override void LoseGame()
    {
        UpdateScore();
        UpdateObjectiveProgress();
        SceneManager.LoadScene("Lose");
    }

    // score
    public override void UpdateScore()
    {
        Score = Mathf.Round((score1 + score2 + score3) / 3);
    }

    // objective
    public override void UpdateObjectiveProgress()
    {
        ObjectiveProgress = 0.25f;
        if (completedObjective1) ObjectiveProgress += 0.25f;
        if (completedObjective2) ObjectiveProgress += 0.25f;
        if (completedObjective3) ObjectiveProgress += 0.25f;
    }

    // objective score update listeners
    void UpdateObjectiveScore1(float pictureQuality)
    {
        completedObjective1 = true;
        if (pictureQuality > score1) score1 = pictureQuality;
        UpdateScore();
        UpdateObjectiveProgress();
    }

    void UpdateObjectiveScore2(float pictureQuality)
    {
        completedObjective2 = true;
        if (pictureQuality > score2) score2 = pictureQuality;
        UpdateScore();
        UpdateObjectiveProgress();
    }

    void UpdateObjectiveScore3(float pictureQuality)
    {
        completedObjective3 = true;
        if (pictureQuality > score3) score3 = pictureQuality;
        UpdateScore();
        UpdateObjectiveProgress();
    }
}
