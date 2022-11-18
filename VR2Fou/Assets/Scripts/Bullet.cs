using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum EBulletType
    {
        Laser,
        BlackHole
    }

    private Vector3 direction;

    public EBulletType bulletType;

    [SerializeField][Range(0, 2.0f)]private float bulletSpeed;

    public void Initiate(Vector3 dir, Vector3 initPos)
    {
        transform.position = initPos;
        direction = dir * bulletSpeed;
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