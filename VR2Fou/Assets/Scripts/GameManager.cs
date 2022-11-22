using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Combo")]
    public int GaugeRequired = 1;
    public int CurrentGauge;
     [HideInInspector]
    public float ComboGauge;
    public float score { get; private set; }

    [Header("Shoot")]
    public UnityEvent EvCombo;

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

    public void IncrementCombo()
    {
        CurrentGauge++;
        if(CurrentGauge >= GaugeRequired)
        {
            EvCombo.Invoke();
            CurrentGauge = 0;
        }
        ComboGauge = CurrentGauge / GaugeRequired;
    }

    #endregion Score
}