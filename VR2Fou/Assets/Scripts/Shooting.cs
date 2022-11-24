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
    [SerializeField] private Quaternion from;
    [SerializeField] private Quaternion to;
    [SerializeField]AnimationCurve recoilCurve;
    private Vector3 recoilPosition;

    [Header("ShootSpecial")]
    [SerializeField]
    private Light colorLight;
    [SerializeField] float intensityMultiplier;
    private Bullet nextSpecialBullet;
    private bool shootSpecial = false;

    private void Awake()
    {
        from = transform.localRotation;
        to = from;
        nextSpecialBullet = specialBullets[Random.Range(0, specialBullets.Count)];
        currentBullet = defaultBullet;
        recoilPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        GameManager.instance.EvCombo.AddListener(ShootSpecial);
    }

    private void OnDisable() => GameManager.instance.EvCombo.RemoveListener(ShootSpecial);

    private void Update()
    {
        intervalShoot -= Time.deltaTime;
        if (Input.GetKeyDown(SWITCH_BULLET_KEY))
            SwitchBullet();
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
        from = transform.localRotation;
        to *= Quaternion.Euler(new Vector3(0, 0, 360 / GameManager.instance.GaugeRequired));
        intervalShoot = intervalShootValue;
    }
    private void AnimateGun(float deltaTime)
    {
        transform.localRotation = Quaternion.Lerp(from, to, deltaTime / intervalShoot);
        transform.localPosition = Vector3.up * recoilCurve.Evaluate(intervalShoot / intervalShootValue) + recoilPosition;
    }

    private void SwitchBullet() => currentBullet = specialBullets[currentBulletIndex++ % specialBullets.Count];

    private void ShootCurrentBullet()
    {
        Instantiate(currentBullet).Initiate(transform.forward, bulletStart.position);
        currentBullet = defaultBullet;
    }

    private void ShootSpecial()
    {
        //TODO: add effects with special shoot
        shootSpecial = true;
    }
}