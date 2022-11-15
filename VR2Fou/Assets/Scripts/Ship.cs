using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    private const string AXIS_H = "Horizontal";

    //field is used for properties
    [field: Range(1, 10)]
    [field: SerializeField]
    public float Speed { get; private set; }
    private Rigidbody _rigidbody;
    private Vector3 _movement;
    // Start is called before the first frame update
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _movement = transform.right * Input.GetAxis(AXIS_H);
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = _movement * Speed;
    }
}
