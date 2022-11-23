using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private const KeyCode SWITCH_BULLET_KEY = KeyCode.B;
    private const KeyCode SHOOT_KEY = KeyCode.Z;

    [SerializeField] private Bullet defaultBullet;
    [SerializeField] private List<Bullet> specialBullets;

    private Bullet currentBullet;
    [SerializeField]private Transform bulletStart;

    private int currentBulletIndex = 1;

    [Header("Animation")]
    [SerializeField]private float intervalShootValue = 0.2f;
    private float intervalShoot;
    private Quaternion from;
    private Quaternion to;
    [SerializeField]AnimationCurve recoilCurve;

    [Header("ShootSpecial")]
    [SerializeField]
    private Light colorLight;
    [SerializeField] float intensityMultiplier;
    private Bullet nextSpecialBullet;
    private bool shootSpecial = false;

    private void Awake() 
    {
        nextSpecialBullet = specialBullets[Random.Range(0, specialBullets.Count)];
        currentBullet = defaultBullet;
    }

    private void OnEnable()
    {
        GameManager.instance.EvCombo.AddListener(ShootSpecial);
    }

    private void OnDisable() => GameManager.instance.EvCombo.RemoveListener(ShootSpecial);

    private void Update()
    {
        if (Input.GetKeyDown(SWITCH_BULLET_KEY))
            SwitchBullet();
        intervalShoot -= Time.deltaTime;
        if (intervalShoot <= 0)
            if (Input.GetKeyDown(SHOOT_KEY))
                Shoot();
        if (intervalShoot > 0)
            AnimateGun(Time.deltaTime);
    }

    private void Shoot()
    {
        if (shootSpecial)
        {
            currentBullet = nextSpecialBullet;
            nextSpecialBullet = specialBullets[Random.Range(0, specialBullets.Count)];
            colorLight.color = nextSpecialBullet.gunLightColor;
            shootSpecial = false;
        }

        ShootCurrentBullet();
        colorLight.intensity = intensityMultiplier * GameManager.instance.CurrentGauge;
        from = transform.rotation;
        to *= Quaternion.Euler(new Vector3(0, 0, 90));
        intervalShoot = intervalShootValue;
    }
    private void AnimateGun(float deltaTime)
    {
        transform.rotation = Quaternion.Lerp(from, to, deltaTime / intervalShoot);
    }

    private void SwitchBullet() => currentBullet = specialBullets[currentBulletIndex++ % specialBullets.Count];

    private void ShootCurrentBullet()
    {
        Instantiate(currentBullet).Initiate(transform.up, transform.position);
        currentBullet = defaultBullet;
    }

    private void ShootSpecial()
    {
        //TODO: add effects with special shoot
        shootSpecial = true;
    }
}