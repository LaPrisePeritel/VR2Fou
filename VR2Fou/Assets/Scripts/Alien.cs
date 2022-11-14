using System;
using UnityEngine;

public class Alien : MonoBehaviour
{
    private Action onTouchBorder;

    private Vector3 leftBorder, rightBorder;

    public void Initialisation(Action _onTouchBorder, Vector3 _leftBorder, Vector3 _rightBorder)
    {
        onTouchBorder = _onTouchBorder;
        leftBorder = _leftBorder;
        rightBorder = _rightBorder;
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

    private void OnCollisionEnter(Collision c)
    {
        Debug.Log(c.gameObject.name);
        if (c.collider.CompareTag("Bullet"))
        {
            Destroy(c.gameObject);
            Destroy(this);
        }
    }
}