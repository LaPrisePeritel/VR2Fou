using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 direction;

    private void Start()
    {
        Destroy(gameObject, 5.0f);
    }
    public void Initiate(Vector3 initPos, Vector3 dir)
    {
        transform.position = initPos;
        direction = dir;
    }
    void Update()
    {
        transform.Translate(direction);
    }
}
