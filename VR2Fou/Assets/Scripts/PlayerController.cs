using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
internal sealed class PlayerController : MonoBehaviour
{
    private const string AXIS_H = "Horizontal";
    private const string AXIS_V = "Vertical";

    [field: Range(1, 10)]
    [field: SerializeField]
    public float Speed { get; private set; }

    private Rigidbody _rigidbody;

    private Vector3 _movement;

    private void Awake() => _rigidbody = GetComponent<Rigidbody>();

    private void Update() => _movement = transform.forward * Input.GetAxis(AXIS_V) + transform.right * Input.GetAxis(AXIS_H);

    private void FixedUpdate() => _rigidbody.velocity = _movement * Speed;
}