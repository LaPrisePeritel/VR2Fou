using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField]
    private List<Bullet> prefabsBullet;


    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Bullet b = Instantiate(prefabsBullet[Random.Range(0, prefabsBullet.Count)]);
            b.Initiate(transform.up, transform.position);
        }
    }
}