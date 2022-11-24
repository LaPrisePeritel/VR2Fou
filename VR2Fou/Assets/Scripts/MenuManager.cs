using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

internal enum HYPERSPACE_STATE
{
    IDLE,
    ENTER_HYPERSPACE,
    IN_HYPERSPACE,
    EXIT_HYPERSPACE
}

public class MenuManager : MonoBehaviour
{
    private const float HYPERSPACE_DURATION = 10.0f;

    [SerializeField] private Buttons playButton;
    [SerializeField] private Buttons quitButton;

    [SerializeField] private GameObject hyperspace;
    [SerializeField] private AnimationCurve hyperspaceCurve;

    private bool inChangingPhase = true;
    private Camera cam;
    private HYPERSPACE_STATE hyperspaceState = HYPERSPACE_STATE.IDLE;

    private void Start()
    {
        playButton.Event.AddListener(() => StartCoroutine(HyperspaceEffect()));
        quitButton.Event.AddListener(Application.Quit);
        cam = Camera.main;
    }

    private IEnumerator HyperspaceEffect()
    {
        var hyperspaceParticle = Instantiate(hyperspace, new Vector3(0, 0.7f, 20.0f), Quaternion.identity, transform);
        cam.GetComponent<CameraShake>().LaunchShake(HYPERSPACE_DURATION, 0.01f);

        float time = 0.0f;

        Vector3 startPosition = hyperspaceParticle.transform.position;
        while (time < HYPERSPACE_DURATION)
        {
            var interpolateTime = time / HYPERSPACE_DURATION;
            if (interpolateTime >= 0.5f)
                hyperspaceState = HYPERSPACE_STATE.EXIT_HYPERSPACE;
            else if (interpolateTime >= 0.4f)
                hyperspaceState = HYPERSPACE_STATE.IN_HYPERSPACE;
            else
                hyperspaceState = HYPERSPACE_STATE.ENTER_HYPERSPACE;

            hyperspaceParticle.transform.position = Vector3.Lerp(startPosition, new Vector3(startPosition.x, startPosition.y, 0.0f), interpolateTime);
            switch (hyperspaceState)
            {
                case HYPERSPACE_STATE.ENTER_HYPERSPACE:
                    if (inChangingPhase)
                    {
                        inChangingPhase = false;
                        StartCoroutine(EnterHyperspace());
                    }
                    break;

                case HYPERSPACE_STATE.IN_HYPERSPACE:
                    cam.fieldOfView = 120.0f;
                    break;

                case HYPERSPACE_STATE.EXIT_HYPERSPACE:
                    if (inChangingPhase)
                    {
                        inChangingPhase = false;
                        StartCoroutine(ExitHyperspace());
                    }
                    break;
            }

            time += Time.deltaTime;
            yield return null;
        }

        hyperspaceParticle.transform.position = new Vector3(startPosition.x, startPosition.y, 0.0f);
        Destroy(hyperspaceParticle);

        SceneManager.LoadScene("BasicProto");
    }

    private IEnumerator EnterHyperspace()
    {
        float time = 0.0f;
        while (time < 4.0f)
        {
            cam.fieldOfView = Mathf.Lerp(80.0f, 120.0f, time / 0.2f);
            time += Time.deltaTime;
            yield return null;
        }

        cam.fieldOfView = 120.0f;
        inChangingPhase = true;
    }

    private IEnumerator ExitHyperspace()
    {
        float time = 0.0f;
        while (time < 5.0f)
        {
            cam.fieldOfView = Mathf.Lerp(120.0f, 80.0f, time / 5.0f);
            time += Time.deltaTime;
            yield return null;
        }

        cam.fieldOfView = 80.0f;
        inChangingPhase = true;
    }
}