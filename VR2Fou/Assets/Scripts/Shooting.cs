using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 shootingDirection;

    [SerializeField]
    private Bullet prefabBullet;

    [SerializeField]
    [Range(0.1f, 2.0f)]
    private float speedBullet;

    // Start is called before the first frame update
    private void Start()
    {
        shootingDirection = transform.forward;
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Bullet b = Instantiate(prefabBullet);
            b.Initiate(mainCamera, transform.up * speedBullet, transform.position);
        }
    }
}