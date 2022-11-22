using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Score")]
    public int GaugeRequired;
    public int CurrentGauge;
     [HideInInspector]
    public float ComboGauge;
    public float score { get; private set; }


    [Header("Timer")]
    public float timer;

    [Header("Rounds")]
    public int currentRound;

    private void Awake()
    {
        if (instance != null && instance != this) { }
        Destroy(gameObject);

        instance = this;
    }

    public void End(bool _victory)
    {
        Debug.Log($"Victory: {_victory}");
    }

    #region Score

    public void IncrementScore()
    {
        CurrentGauge++;
        if(CurrentGauge >= GaugeRequired)
        {
            CurrentGauge = 0;
        }
        ComboGauge = CurrentGauge / GaugeRequired;
    }

    #endregion Score
}