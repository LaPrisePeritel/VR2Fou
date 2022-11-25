using JSAM;
using UnityEngine;
using UnityEngine.UI;

internal class FXManager : MonoBehaviour
{
    [SerializeField] private bool _isActive;

    [SerializeField] private RectTransform _panel;
    [Space]
    [SerializeField] private Toggle _toggleShootEnemy;
    [SerializeField] private Toggle _toggleShootPlayer;
    [SerializeField] private Toggle _toggleStars;
    [SerializeField] private Toggle _toggleShield;
    [SerializeField] private Toggle _toggleMusic;
    [SerializeField] private Toggle _toggleSound;
    [Space]
    [SerializeField] private GameObject _stars;
    [SerializeField] private GameObject _shield;
    [Space]
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private BoardMovement _boardMovement;
    [SerializeField] private Shooting _shooting;

    private void Awake()
    {
        _panel.gameObject.SetActive(_isActive);

        _toggleShootEnemy.onValueChanged.AddListener(x => _boardMovement.CanShoot = x);
        _toggleShootPlayer.onValueChanged.AddListener(x => _shooting.CanShoot = x);
        _toggleStars.onValueChanged.AddListener(_stars.SetActive);
        _toggleShield.onValueChanged.AddListener(_shield.SetActive);
        _toggleMusic.onValueChanged.AddListener(x => AudioManager.SetMusicChannelMute(!x));
        _toggleSound.onValueChanged.AddListener(x => AudioManager.SetSoundChannelMute(!x));
    }

    private void Start()
    {
        _toggleShootEnemy.isOn = false;
        _toggleShootPlayer.isOn = false;
        _toggleStars.isOn = false;
        _toggleShield.isOn = false;
        _toggleMusic.isOn = false;
        _toggleSound.isOn = false;
    }

    private void Update()
    {
        _isActive ^= Input.GetKeyDown(KeyCode.F1);

        _panel.gameObject.SetActive(_isActive);

        Cursor.visible = _isActive;
        Cursor.lockState = _isActive ? CursorLockMode.None : CursorLockMode.Locked;

        //_cameraController.CanLook = !_isActive;
    }
}