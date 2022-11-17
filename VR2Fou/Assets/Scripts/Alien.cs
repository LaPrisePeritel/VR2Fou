using System;
using UnityEngine;

public class Alien : MonoBehaviour
{
    private Action onTouchBorder, onDeath;

    private Vector3 leftBorder, rightBorder;
    
    private Animator animator;

    [SerializeField]
    private ParticleSystem deathParticle;

    private bool isDead;
    private float deathDuration = 0;

    private void Awake()
    {
        deathParticle.Stop();
    }

    public void Initialisation(Action _onTouchBorder, Vector3 _leftBorder, Vector3 _rightBorder, Action _onDeath)
    {
        onTouchBorder = _onTouchBorder;
        leftBorder = _leftBorder;
        rightBorder = _rightBorder;

        onDeath = _onDeath;
        animator = GetComponent<Animator>();
    }

    public void AddOnDeathAction(Action _onDeath)
    {
        onDeath += _onDeath;
    }

    private void Update()
    {
        if (transform.position.x + transform.localScale.x / 2f >= rightBorder.x)
        {
            onTouchBorder();
        }
        else if (transform.position.x - transform.localScale.x / 2f <= leftBorder.x)
        {
            onTouchBorder();
        }
        if (isDead)
        {
            deathDuration += Time.deltaTime;
            if(deathDuration >= deathParticle.main.duration)
            {
                Destroy(gameObject);
            }
        }
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
    private void OnCollisionEnter(Collision c)
    {
        Debug.Log(c.gameObject.name);
        if (c.collider.CompareTag("Bullet"))
        {
            Destroy(c.gameObject);
            animator.SetTrigger("Death");
        }
    }
}