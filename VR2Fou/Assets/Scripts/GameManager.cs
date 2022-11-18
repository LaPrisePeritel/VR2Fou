using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Score")]
    [SerializeField]
    private TMP_Text ScoreText;

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

    #region Score

    public void IncrementScore()
    {
        score++;
        if (ScoreText != null)
            ScoreText.text = score.ToString();
    }

    #endregion Score
}