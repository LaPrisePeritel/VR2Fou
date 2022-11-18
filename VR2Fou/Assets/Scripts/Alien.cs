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

    [SerializeField]
    private ParticleSystem deathParticle;

    private bool isDead;
    private float deathDuration = 0;

    [Header("Materials")]
    [SerializeField] private Material vacuumMaterial;

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
        
        RaycastHit[] hits = Physics.RaycastAll(transform.position, -transform.right * 20f);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.parent.gameObject != this)
            {
                wLeft = false;
                break;
            }
        }
        
        hits = Physics.RaycastAll(transform.position, transform.right * 20f);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.parent.gameObject != this)
            {
                wRight = false;
                break;
            }
        }
        hits = Physics.RaycastAll(transform.position, -transform.up * 20f);
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

    private void OnDestroy()
    {
        GameManager.instance.IncrementScore();
    }

    private void OnDeath()
    {
        isDead = true;

        transform.GetChild(0).GetComponent<Collider>().enabled = false;
        transform.SetParent(null);
        
        if (wLeft)
        {
            RaycastHit[] hits = Physics.RaycastAll(transform.position, -transform.right * 5f);
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.parent.gameObject != this)
                {
                    Alien alien = hit.transform.parent.gameObject.GetComponent<Alien>();

                    if (alien != null)
                    {
                        alien.SetCastBorderLeft();
                        break;
                    }
                }
            }
        }
        
        if (wLeft)
        {
            RaycastHit[] hits = Physics.RaycastAll(transform.position, -transform.right * 20f);
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.parent.gameObject != this)
                {
                    Alien alien = hit.transform.parent.gameObject.GetComponent<Alien>();

                    if (alien != null)
                    {
                        alien.SetCastBorderLeft();
                        break;
                    }
                }
            }
        }
        
        if (wRight)
        {
            RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.right * 20f);
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.parent.gameObject != this)
                {
                    Alien alien = hit.transform.parent.gameObject.GetComponent<Alien>();

                    if (alien != null)
                    {
                        alien.SetCastBorderRight();
                        break;
                    }
                }
            }
        }
        
        if (wDown)
        {
            RaycastHit[] hits = Physics.RaycastAll(transform.position, -transform.up * 20f);
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.parent.gameObject != this)
                {
                    Alien alien = hit.transform.parent.gameObject.GetComponent<Alien>();

                    if (alien != null)
                    {
                        alien.SetCastBorderDown();
                        break;
                    }
                }
            }
        }
        
        //deathParticle.Play();
    }

    private IEnumerator BlackHoleDeath(Vector3 _holePosition, Material _vacuumMaterial)
    {
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

        Camera.main.GetComponent<CameraShake>().LaunchShake();
        Destroy(gameObject);
    }

    public void Hitted(Bullet.EBulletType _bulletType, Vector3 _bulletPosition)
    {
        if (isDead)
            return;

        isDead = true; //TEMP

        StopAllCoroutines();

        switch (_bulletType)
        {
            case Bullet.EBulletType.BlackHole:
                StartCoroutine(BlackHoleDeath(_bulletPosition, vacuumMaterial));
                break;
            case Bullet.EBulletType.Laser:
                Destroy(gameObject);
                break;
        }
    }

    /*private void OnCollisionEnter(Collision c)
    {
        if (c.collider.CompareTag("Bullet"))
        {
            Destroy(c.gameObject);
            animator.SetTrigger("Death");
        }
    }*/
}