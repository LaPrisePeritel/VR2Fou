using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
internal sealed class CameraController : MonoBehaviour
{
    [SerializeField] private InputActionProperty camInput;

    private const float Y_ROT_LIMIT = 90;

    [field: Range(.1f, 10)]
    [field: SerializeField]
    public float Sensitivity { get; private set; }

    [SerializeField]
    private Transform _body;

    private Vector2 _rotation;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Vector2 axis = camInput.action.ReadValue<Vector2>();
        _rotation.x += axis.x * Sensitivity;
        _rotation.y += axis.y * Sensitivity;
        _rotation.y = Mathf.Clamp(_rotation.y, -Y_ROT_LIMIT, Y_ROT_LIMIT);
        var xQuat = Quaternion.AngleAxis(_rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(_rotation.y, Vector3.left);
        transform.localRotation = yQuat;
        _body.localRotation = xQuat;
    }
}