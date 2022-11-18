using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum EBulletType
    {
        Bullet,
        BlackHole
    }
    
    private const float LIFETIME = 5.0f;
    private const float CAMERA_SHAKE_DURATION = .5f;
    private const float CAMERA_SHAKE_MAGNITUDE = 1.2f;

    private Vector3 direction;

    private CameraShake cameraShake;

    public EBulletType bulletType;

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

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Alien"))
            return;

        other.transform.parent.GetComponent<Alien>().Hitted(bulletType, transform.position);
        
        Destroy(gameObject);
    }
}
