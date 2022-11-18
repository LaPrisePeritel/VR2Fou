using System;
using System.Collections;
using UnityEngine;

public class Alien : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private Animator animator;

    private Action onTouchBorder, onDeath;

    private Vector3 leftBorder, rightBorder;

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

    public void Initialisation(Action _onTouchBorder, Vector3 _leftBorder, Vector3 _rightBorder, Action _onDeath)
    {
        onTouchBorder = _onTouchBorder;
        leftBorder = _leftBorder;
        rightBorder = _rightBorder;

        onDeath = _onDeath;
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
        deathParticle.Play();
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