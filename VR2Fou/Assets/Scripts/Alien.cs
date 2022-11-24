using System;
using System.Collections;
using UnityEngine;

public class Alien : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Animator animator;
    private Rigidbody rb;
    public Rigidbody Rigidbody => rb;

    private Action<int> onTouchBorder;
    private Action onTouchDown, onDeath;

    private Vector3 leftBorder, rightBorder, downBorder;
    private bool isLeftDirection;
    private int alienLineIndex;

    [SerializeField] private ParticleSystem deathParticle;
    [SerializeField] private LayerMask aliensMask;

    private bool isDead;
    private float deathDuration = 0;

    [Header("Materials")]
    [SerializeField] private Material vacuumMaterial;
    [SerializeField] private Material balloonMaterial;

    [Header("Particles")]
    [SerializeField] private ParticleSystem confettiParticlesPrefab;
    [SerializeField] private ParticleSystem dustParticlesPrefab;
    [SerializeField] private ParticleSystem lightningParticlesPrefab;

    [Header("Shooting")]
    [SerializeField] private Bullet alienBulletPrefab;

    private void Awake()
    {
        deathParticle.Stop();

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
    }

    public void Initialisation(Action<int> _onTouchBorder, int _alienLineIndex, Action _onTouchDown, Vector3 _leftBorder, Vector3 _rightBorder, Vector3 _downBorder, Action _onDeath)
    {
        onTouchBorder = _onTouchBorder;
        alienLineIndex = _alienLineIndex;
        
        onTouchDown = _onTouchDown;
        leftBorder = _leftBorder;
        rightBorder = _rightBorder;
        downBorder = _downBorder;

        onDeath = _onDeath;
    }

    public void SetDirection(bool _isLeft)
    {
        isLeftDirection = _isLeft;
    }

    public void AddOnDeathAction(Action _onDeath)
    {
        onDeath += _onDeath;
    }

    private void Update()
    {
        if (isDead)
            return;

        if (!isLeftDirection && transform.position.x >= rightBorder.x)
        {
            onTouchBorder(alienLineIndex);
        }
        else if (isLeftDirection && transform.position.x <= leftBorder.x)
        {
            onTouchBorder(alienLineIndex);
        }
    }

    public void Shoot()
    {
        Instantiate(alienBulletPrefab, transform.position, Quaternion.identity).Initiate(Vector3.back, transform.position);
    }
    
    private void OnDeath()
    {
        isDead = true;
        deathParticle.Play();
    }

    private IEnumerator BlackHoleDeath(Vector3 _holePosition, Material _vacuumMaterial)
    {
        Camera.main.GetComponent<CameraShake>().LaunchShake(.3f, .1f);
        
        meshRenderer.material = _vacuumMaterial;
        meshRenderer.material.SetVector("_Black_Hole_Position", _holePosition);

        float t = 0f;

        while (t <= 1f)
        {
            yield return null;

            t += Time.deltaTime;
            float value = Mathf.Lerp(0f, 1f, t);
            meshRenderer.material.SetFloat("_Effect", value);
        }
        
        //Camera.main.GetComponent<CameraShake>().LaunchShake(0.5f, 0.3f);
        Destroy(gameObject);
    }

    private void StartBalloonDissolve()
    {
        StartCoroutine(BalloonDeath(balloonMaterial));
    }
    
    private IEnumerator BalloonDeath(Material _baloonMaterial)
    {
        meshRenderer.material = _baloonMaterial;
        //meshRenderer.material.SetVector("_Black_Hole_Position", _holePosition);

        bool particlePlayed = false;
        ParticleSystem confettiParticles = Instantiate(confettiParticlesPrefab, transform.position, Quaternion.identity);
        
        float t = 0f;

        while (t <= 1f)
        {
            yield return null;

            t += Time.deltaTime * 5f;
            float value = Mathf.Lerp(0f, 1f, t);
            meshRenderer.material.SetFloat("_Dissolve", value);

            if (!particlePlayed && value >= 0.75f)
            {
                Camera.main.GetComponent<CameraShake>().LaunchShake(0.2f, 0.3f);
                confettiParticles.Play();
                particlePlayed = true;
            }
        }

        while (confettiParticles.IsAlive())
        {
            yield return null;
        }

        //Camera.main.GetComponent<CameraShake>().LaunchShake(0.5f, 0.3f);
        Destroy(confettiParticles.gameObject);
        Destroy(gameObject);
    }

    private IEnumerator BeingDust(Material _material)
    {
        meshRenderer.material = _material;
        
        ParticleSystem dustParticles = Instantiate(dustParticlesPrefab, transform.position, Quaternion.identity);
        dustParticles.Play();
        
        float t = 0f;

        while (t <= 1f)
        {
            yield return null;

            t += Time.deltaTime;
            float value = Mathf.Lerp(0f, 1f, t);
            meshRenderer.material.SetFloat("_Dissolve", value);
        }
        
        dustParticles.Stop();
        
        while (dustParticles.IsAlive())
        {
            yield return null;
        }
        
        Destroy(dustParticles.gameObject);
        Destroy(gameObject);
    }

    private IEnumerator Lightning()
    {
        Camera.main.GetComponent<CameraShake>().LaunchShake(0.3f, 0.5f);
        ParticleSystem lightingParts = Instantiate(lightningParticlesPrefab, transform.position, Quaternion.identity);
        lightingParts.Play();
        float t = 0f;

        while (t <= lightingParts.main.duration)
        {
            yield return null;

            t += Time.deltaTime;
            float value = Mathf.Lerp(0f, 1f, t);
            meshRenderer.material.SetFloat("_Effect", value);
        }
        Destroy(lightingParts.gameObject);
        Destroy(gameObject);
    }
    
    public void Hitted(Bullet.EBulletType _bulletType, Vector3 _bulletPosition)
    {
        if (isDead)
            return;

        isDead = true; //TEMP

        GameManager.instance.IncrementCombo();

        transform.parent = null;

        StopAllCoroutines();
        
        switch (_bulletType)
        {
            default:
                Destroy(gameObject);
                break;
            case Bullet.EBulletType.BlackHole:
                StartCoroutine(BlackHoleDeath(_bulletPosition, vacuumMaterial));
                break;
            case Bullet.EBulletType.Laser:
                Destroy(gameObject);
                break;
            case Bullet.EBulletType.Lightning:
                StartCoroutine(Lightning());
                break;

            case Bullet.EBulletType.Balloon:
                animator.SetTrigger("BalloonDeath");
                break;

            case Bullet.EBulletType.Dust:
                StartCoroutine(BeingDust(balloonMaterial));
                break;
        }
    }
}