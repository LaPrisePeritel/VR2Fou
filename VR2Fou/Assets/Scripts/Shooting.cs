using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField]
    Bullet prefabBullet;

    [SerializeField]
    private List<Bullet> prefabsBullet;

    private bool shootSpecial = false;

    private void Awake()
    {
        GameManager.instance.EvCombo.AddListener(ShootSpecial);
    }
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (shootSpecial)
            {
                Bullet b = Instantiate(prefabsBullet[Random.Range(0, prefabsBullet.Count)]);
                shootSpecial = false;
            }
            else
            {
                Bullet b = Instantiate(prefabBullet);
                b.Initiate(transform.up, transform.position);
            }
        }
    }

    private void ShootSpecial()
    {
        //Add effects with special shoot
        shootSpecial = true;
    }
}