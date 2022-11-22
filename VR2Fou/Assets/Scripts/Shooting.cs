using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private const KeyCode SWITCH_BULLET_KEY = KeyCode.B;
    private const KeyCode SHOOT_KEY = KeyCode.Z;

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
        if (Input.GetKeyDown(SWITCH_BULLET_KEY))
            SwitchBullet();

        if (Input.GetKeyDown(SHOOT_KEY))
        {
            if (isRandom)
            {
                isRandom = false;
                currentBullet = bullets[Random.Range(0, bullets.Count)];
                return;
            }

            ShootCurrentBullet();
        }
    }

    private void SwitchBullet() => currentBullet = bullets[currentBulletIndex++ % bullets.Count];

    private void ShootCurrentBullet() => Instantiate(currentBullet).Initiate(transform.up, transform.position);

    private void ShootSpecial()
    {
        //TODO: add effects with special shoot
    }
}