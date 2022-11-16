using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private const float LIFETIME = 5.0f;
    private const float CAMERA_SHAKE_DURATION = 1.0f;
    private const float CAMERA_SHAKE_MAGNITUDE = 3.0f;

    public Vector3 direction;

    private CameraShake cameraShake;

    private void Start()
    {
        Destroy(gameObject, LIFETIME);
    }

    public void Initiate(Camera camera, Vector3 dir, Vector3 initPos)
    {
        cameraShake = camera.GetComponent<CameraShake>();
        transform.position = initPos;
        direction = dir;
    }

    private void Update()
    {
        transform.Translate(direction);
    }

    private void OnCollisionEnter(Collision c)
    {
        Debug.Log(c.gameObject.name);
        if (c.collider.CompareTag("Alien"))
        {
            cameraShake.LaunchShake(CAMERA_SHAKE_DURATION, CAMERA_SHAKE_MAGNITUDE);
            Destroy(c.gameObject);
            Destroy(gameObject);
        }
    }
}