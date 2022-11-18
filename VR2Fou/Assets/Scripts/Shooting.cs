using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private Camera mainCamera;

    [SerializeField]
    private List<Bullet> prefabsBullet;

    // Start is called before the first frame update
    private void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Bullet b = Instantiate(prefabsBullet[Random.Range(0, prefabsBullet.Count)]);
            b.Initiate(mainCamera, transform.up, transform.position);
        }
    }
}