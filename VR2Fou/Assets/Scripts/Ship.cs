using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ship : MonoBehaviour
{
    [SerializeField] private InputActionProperty spaceshipInput;

    private Rigidbody _rigidbody;
    private Vector3 _movement;
    private Vector3 startPosition;

    //field is used for properties
    [field: Range(1, 10)]
    [field: SerializeField]
    public float Speed { get; private set; }

    [SerializeField] private float movementClampX;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        startPosition = transform.position;
    }

    private void Update()
    {
        _movement = transform.right * Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        Vector3 nPosition = _rigidbody.position + _movement * Speed * Time.fixedDeltaTime;
        nPosition.x = Mathf.Clamp(nPosition.x, startPosition.x - movementClampX, startPosition.x + movementClampX);

        _rigidbody.MovePosition(nPosition);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        if (startPosition != Vector3.zero)
        {
            Gizmos.DrawLine(startPosition + Vector3.left * movementClampX,
                startPosition + Vector3.right * movementClampX);

            Gizmos.DrawWireSphere(startPosition + Vector3.left * movementClampX, 0.5f);
            Gizmos.DrawWireSphere(startPosition + Vector3.right * movementClampX, 0.5f);
        }
        else
        {
            Gizmos.DrawLine(transform.position + Vector3.left * movementClampX,
                transform.position + Vector3.right * movementClampX);

            Gizmos.DrawWireSphere(transform.position + Vector3.left * movementClampX, 0.5f);
            Gizmos.DrawWireSphere(transform.position + Vector3.right * movementClampX, 0.5f);
        }
    }
}