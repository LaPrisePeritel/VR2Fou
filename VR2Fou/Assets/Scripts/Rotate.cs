using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.rotation *= Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z + rotationSpeed));
    }
}
