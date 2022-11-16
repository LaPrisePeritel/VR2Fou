using System;
using UnityEngine;

public class Alien : MonoBehaviour
{
    private Action onTouchBorder, onDeath;

    private Vector3 leftBorder, rightBorder;
    
    private Animator animator;

    [SerializeField]
    private ParticleSystem deathParticle;


    public void Initialisation(Action _onTouchBorder, Vector3 _leftBorder, Vector3 _rightBorder, Action _onDeath)
    {
        onTouchBorder = _onTouchBorder;
        leftBorder = _leftBorder;
        rightBorder = _rightBorder;

        onDeath = _onDeath;
        animator = GetComponent<Animator>();
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
    }

    private void OnDeath()
    {
        animator.SetTrigger("Death");
    }
    private void OnCollisionEnter(Collision c)
    {
        Debug.Log(c.gameObject.name);
        if (c.collider.CompareTag("Bullet"))
        {
            Destroy(c.gameObject);
            OnDeath();
        }
    }
}