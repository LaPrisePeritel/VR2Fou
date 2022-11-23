using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField]
    Bullet prefabBullet;

    [SerializeField]
    private List<Bullet> prefabsBullet;

    [SerializeField]
    private float intervalShootValue = 0.2f;
    private float intervalShoot;

    private bool shootSpecial = false;

    [SerializeField]
    private Transform bulletStart;

    [SerializeField]
    AnimationCurve recoilCurve;

    private Quaternion from;
    private Quaternion to;

    [Header("ShootSpecial")]
    [SerializeField]
    private Light colorLight;
    [SerializeField] float intensityMultiplier;
    private Bullet nextSpecialBullet;

    private void OnEnable()
    {
        GameManager.instance.EvCombo.AddListener(ShootSpecial);
        from = transform.localRotation;
        to = from;
        nextSpecialBullet = prefabsBullet[Random.Range(0, prefabsBullet.Count)];
        colorLight.color = nextSpecialBullet.gunLightColor;
    }
    // Update is called once per frame
    private void Update()
    {
        intervalShoot -= Time.deltaTime;
        if(intervalShoot <= 0)
        {
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                Shoot();
            }
        }
        if(intervalShoot > 0)
        {
            AnimateGun(Time.deltaTime);
        }
    }

    private void Shoot()
    {
        if (shootSpecial)
        {
            Bullet b = Instantiate(nextSpecialBullet);
            nextSpecialBullet = prefabsBullet[Random.Range(0, prefabsBullet.Count)];
            colorLight.color = nextSpecialBullet.gunLightColor;
            b.Initiate(transform.forward, bulletStart.position);
            shootSpecial = false;
        }
        else
        {
            Bullet b = Instantiate(prefabBullet);
            b.Initiate(transform.forward, bulletStart.position);
            colorLight.intensity = intensityMultiplier * GameManager.instance.CurrentGauge;

        }
        from = transform.rotation;
        to *= Quaternion.Euler(new Vector3(0,0,90));
        intervalShoot = intervalShootValue;
    }

    private void AnimateGun(float deltaTime)
    {
        transform.rotation = Quaternion.Lerp(from, to, deltaTime / intervalShoot);
    }

    private void ShootSpecial()
    {
        //TODO : Add effects with special shoot
        shootSpecial = true;
    }

}