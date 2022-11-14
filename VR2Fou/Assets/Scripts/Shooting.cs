using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField]
    Transform cam;
    [SerializeField]
    Vector3 shootingDirection;
    [SerializeField]
    Bullet prefabBullet;
    [SerializeField]
    [Range(0.1f, 2.0f)]
    float speedBullet;
    // Start is called before the first frame update
    void Start()
    {
        shootingDirection = cam.forward;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Bullet b = Instantiate<Bullet>(prefabBullet);
            b.direction = cam.forward * speedBullet;
            Destroy(b.gameObject, 5.0f);
        } 
    }
}
