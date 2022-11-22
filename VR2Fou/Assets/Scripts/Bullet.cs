using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum EBulletType
    {
        Laser,
        BlackHole,
        Lightning
        Balloon,
        Dust
    }

    protected Vector3 direction;

    public EBulletType bulletType;

    [SerializeField][Range(0, 2.0f)]private float bulletSpeed;

    public virtual void Initiate(Vector3 dir, Vector3 initPos)
    {
        transform.position = initPos;
        direction = dir * bulletSpeed;
    }

    protected virtual void Update()
    {
        transform.Translate(direction);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Alien"))
            return;

        other.transform.parent.GetComponent<Alien>().Hitted(bulletType, transform.position);

        Destroy(gameObject);
    }
}