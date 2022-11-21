using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private const float HYPERSPACE_DURATION = 20.0f;

    [SerializeField] private Buttons playButton;
    [SerializeField] private Buttons quitButton;
    [SerializeField] private Buttons optionsButton;

    [SerializeField] private GameObject hyperspace;
    [SerializeField] private AnimationCurve hyperspaceCurve;
    private Camera cam;

    private void Start()
    {
        playButton.Event.AddListener(() => StartCoroutine(HyperspaceEffect()));
        quitButton.Event.AddListener(Application.Quit);
        //optionsButton.Event.AddListener();
        cam = Camera.main;
    }

    private IEnumerator HyperspaceEffect()
    {
        var hyperspaceParticle = Instantiate(hyperspace, new Vector3(0, 0.7f, 20.0f), Quaternion.identity, transform);

        float time = 0;
        Vector3 startPosition = hyperspaceParticle.transform.position;
        while (time < HYPERSPACE_DURATION)
        {
            hyperspaceParticle.transform.position = Vector3.Lerp(startPosition, new Vector3(startPosition.x, startPosition.y, 0.0f), time / HYPERSPACE_DURATION);
            cam.fieldOfView = Mathf.Lerp(60.0f, 90.0f, hyperspaceCurve.Evaluate(time / HYPERSPACE_DURATION));
            time += Time.deltaTime;
            yield return null;
        }
        hyperspaceParticle.transform.position = new Vector3(startPosition.x, startPosition.y, 0.0f);

        //SceneManager.LoadScene("BasicProto");
    }
}