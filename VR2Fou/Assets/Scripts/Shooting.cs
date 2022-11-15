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
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Bullet b = Instantiate<Bullet>(prefabBullet);
            b.Initiate(cam.transform.position, cam.up * speedBullet);
        } 
    }
}
