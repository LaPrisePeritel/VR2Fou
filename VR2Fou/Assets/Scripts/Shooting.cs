using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private const KeyCode DEBUG_KEY = KeyCode.B;

    [SerializeField] private Bullet defaultBullet;
    [SerializeField] private List<Bullet> bullets;

    private Bullet currentBullet;

    private int currentBulletIndex = 1;
    private bool isRandom;

    private void Awake() => currentBullet = defaultBullet;

    private void OnEnable() => GameManager.instance.EvCombo.AddListener(ShootSpecial);

    private void OnDisable() => GameManager.instance.EvCombo.RemoveListener(ShootSpecial);

    private void Update()
    {
        if (Input.GetKeyDown(DEBUG_KEY))
            currentBullet = bullets[currentBulletIndex++ % bullets.Count];

        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (isRandom)
            {
                currentBullet = bullets[Random.Range(0, bullets.Count)];
                isRandom = false;
                return;
            }

            ShootCurrentBullet();
        }
    }

    private void ShootCurrentBullet() => Instantiate(currentBullet).Initiate(transform.up, transform.position);

    private void ShootSpecial()
    {
        //TODO: Add effects with special shoot
    }
}