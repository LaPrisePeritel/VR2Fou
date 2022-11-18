using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Buttons playButton;
    [SerializeField] private Buttons quitButton;
    [SerializeField] private Buttons optionsButton;

    private void Start()
    {
        playButton.Event.AddListener(() => SceneManager.LoadScene("GameScene"));
        quitButton.Event.AddListener(Application.Quit);
        //optionsButton.Event.AddListener();
    }
}