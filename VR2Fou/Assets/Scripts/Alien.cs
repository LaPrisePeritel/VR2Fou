using System;
using UnityEngine;

public class Alien : MonoBehaviour
{
    private Action onTouchBorder, onDeath;

    private Vector3 leftBorder, rightBorder;

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
        if (transform.position.x + transform.localScale.x / 2f >= rightBorder.x)
        {
            onTouchBorder();
        }
        else if (transform.position.x - transform.localScale.x / 2f <= leftBorder.x)
        {
            onTouchBorder();
        }
    }
}