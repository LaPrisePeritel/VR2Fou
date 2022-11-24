using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    [SerializeField] private InputActionProperty switchBulletKey;
    [SerializeField] private InputActionProperty ShootKey;

    [SerializeField] private Bullet defaultBullet;
    [SerializeField] private List<Bullet> specialBullets;

    private Bullet currentBullet;
    [SerializeField] private Transform bulletStart;

    private int currentBulletIndex = 1;

    [Header("Animation")]
    [SerializeField] private float intervalShootValue = 0.2f;

    private float intervalShoot;
    [SerializeField] private Quaternion from;
    [SerializeField] private Quaternion to;
    [SerializeField] private AnimationCurve recoilCurve;

    [Header("ShootSpecial")]
    [SerializeField]
    private Light colorLight;

    [SerializeField] private float intensityMultiplier;
    private Bullet nextSpecialBullet;
    private bool shootSpecial = false;

    private void Awake()
    {
        from = transform.localRotation;
        to = from;
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
        intervalShoot -= Time.deltaTime;

        if (switchBulletKey.action.WasPressedThisFrame())
            SwitchBullet();

        if (intervalShoot <= 0)
            if (ShootKey.action.WasPressedThisFrame())
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