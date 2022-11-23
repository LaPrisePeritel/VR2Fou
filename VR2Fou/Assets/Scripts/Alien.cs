using System;
using System.Collections;
using UnityEngine;

public class Alien : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Animator animator;

    private Action onTouchBorder, onTouchDown, onDeath;

    private Vector3 leftBorder, rightBorder, downBorder;
    private bool wLeft, wRight, wDown;

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

    private void Awake()
    {
        deathParticle.Stop();

        animator = GetComponent<Animator>();
        meshRenderer = transform.GetChild(0).GetComponent<MeshRenderer>();
    }

    public void Initialisation(Action _onTouchBorder, Action _onTouchDown, Vector3 _leftBorder, Vector3 _rightBorder, Vector3 _downBorder, Action _onDeath)
    {
        onTouchBorder = _onTouchBorder;
        onTouchDown = _onTouchDown;
        leftBorder = _leftBorder;
        rightBorder = _rightBorder;
        downBorder = _downBorder;

        onDeath = _onDeath;
    }

    public void InitializeBorders()
    {
        wLeft = true;
        wRight = true;
        wDown = true;
        
        RaycastHit[] hits = Physics.RaycastAll(transform.position, -transform.right, 20f, aliensMask);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.parent.gameObject != this)
            {
                wLeft = false;
                break;
            }
        }
        
        hits = Physics.RaycastAll(transform.position, transform.right, 20f, aliensMask);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.parent.gameObject != this)
            {
                wRight = false;
                break;
            }
        }
        
        hits = Physics.RaycastAll(transform.position, -transform.up, 20f, aliensMask);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.parent.gameObject != this)
            {
                wDown = false;
                break;
            }
        }
    }

    public void SetCastBorderLeft()
    {
        wLeft = true;
    }

    public void SetCastBorderRight()
    {
        wRight = true;
    }

    public void SetCastBorderDown()
    {
        wDown = true;
    }

    public void AddOnDeathAction(Action _onDeath)
    {
        onDeath += _onDeath;
    }

    private void Update()
    {
        if (isDead)
            return;

        if (transform.position.x + transform.localScale.x / 2f >= rightBorder.x)
        {
            onTouchBorder();
        }
        else if (transform.position.x - transform.localScale.x / 2f <= leftBorder.x)
        {
            onTouchBorder();
        }
        /*if (isDead)
        {
            deathDuration += Time.deltaTime;
            if(deathDuration >= deathParticle.main.duration)
            {
                Destroy(gameObject);
            }
        }*/
    }
    private void OnDeath()
    {
        isDead = true;
        deathParticle.Play();
    }

    private IEnumerator BlackHoleDeath(Vector3 _holePosition, Material _vacuumMaterial)
    {
        Camera.main.GetComponent<CameraShake>().LaunchShake(.5f, 10f);
        
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