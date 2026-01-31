using UnityEngine;
using DG.Tweening;
using Input;

public class HookAbility : MonoBehaviour
{
    private static readonly int Hook = Animator.StringToHash("Hook");
    
    [SerializeField] private HookProjectile _projectilePrefab;
    [SerializeField] private Transform _projectileSpawnPoint;
    [SerializeField] private float _upDistance;
    [SerializeField] private float _upTime;

    private bool _lock;

    private HookProjectile _currentProjectile;
    private PlayerInputController _playerInputController;
    private ThirdPersonController _thirdPersonController;
    private Animator _animator;
    private Transform _originalParent;

    private void Awake()
    {
        _playerInputController = GetComponent<PlayerInputController>();
        _thirdPersonController = GetComponent<ThirdPersonController>();
        _animator = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        if (_playerInputController != null)
        {
            _playerInputController.UseAbility += Execute;
        }
    }

    private void OnDisable()
    {
        if (_playerInputController != null)
        {
            _playerInputController.UseAbility -= Execute;
        }
    }

    private void Execute()
    {
        if (_currentProjectile == null)
        {
            FireHook();
        }
        else if (!_lock)
        {
            ReleaseHook();
        }
    }

    private void FireHook()
    {
        if (_currentProjectile != null)
        {
            return;
        }
        
        _animator.SetTrigger("FireHook");
        _animator.SetBool(Hook, true);
        
        _lock = true;
        _currentProjectile = Instantiate(_projectilePrefab);
        _currentProjectile.transform.position = _projectileSpawnPoint.position;
        _currentProjectile.OnContact += OnContact;
        _currentProjectile.OnDestroyed += OnProjectileDestroyed;
        
        _thirdPersonController.EnableAllMovement(false);
        
        _currentProjectile.MoveUp();
    }

    private void OnProjectileDestroyed()
    {
        _lock = false;
        
        _thirdPersonController.EnableAllMovement(true);
    }

    private void ReleaseHook()
    {
        if (_currentProjectile == null)
        {
            return;
        }
        
        transform.SetParent(_originalParent, true);
        _animator.SetBool(Hook, false);
        
        var currentProjectile = _currentProjectile;
        currentProjectile.OnContact -= OnContact;
        
        Destroy(currentProjectile.gameObject);
        
        _currentProjectile = null;
    }

    private void OnContact(Transform newParent)
    {
        _originalParent = transform.parent;
        transform.SetParent(newParent, true);
        
        var originalPosition = transform.position;

        var end = _currentProjectile.EndPoint - Vector3.up * _upDistance;
        var finalDistance = Mathf.Abs(end.y - _currentProjectile.Origin.y);
        
        DOVirtual.Float(0f, finalDistance, _upTime, distance =>
        {
            transform.position = originalPosition + Vector3.up * distance;
            _currentProjectile.UpdateOrigin(distance);
        }).SetEase(Ease.InSine).OnComplete(() =>
        {
            _lock = false;
        });
    }
}