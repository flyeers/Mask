using System;
using DG.Tweening;
using UnityEngine;

public class HookProjectile : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _upDistance;
    [SerializeField] private float _upTime;
    [SerializeField] private LayerMask _ceilingLayer;

    public event Action<Transform> OnContact;
    public event Action OnDestroyed;
    
    private Vector3 _origin;
    private Vector3 _end;
    private Tween _tween;
    private bool _hit = false;
    
    public Vector3 Origin => _origin;
    public Vector3 EndPoint => _end;

    public void MoveUp()
    {
        _origin = transform.position;
        _hit = false;
        
        _tween = DOVirtual.Float(0f, _upDistance, _upTime, distance =>
        {
            _end = _origin + Vector3.up * distance;

            _lineRenderer.SetPosition(0, transform.InverseTransformPoint(_origin));
            _lineRenderer.SetPosition(1, transform.InverseTransformPoint(_end));

            if (Physics.Raycast(_origin, Vector3.up, out var hit, Mathf.Abs(_origin.y - _end.y), _ceilingLayer))
            {
                var newParent = hit.collider.transform;
                OnContact?.Invoke(newParent);
                _tween.Kill();
                _hit = true;
                
                transform.SetParent(newParent, true);
            }

        }).SetDelay(0.25f).SetEase(Ease.InSine).OnComplete(() =>
        {
            if (!_hit)
            {
                Destroy(gameObject);
            }
        });
    }

    public void UpdateOrigin(float upDistance)
    {
        var newOrigin = transform.InverseTransformPoint(_origin) + Vector3.up * upDistance;

        _lineRenderer.SetPosition(0, newOrigin);
    }

    private void OnDestroy()
    {
        OnDestroyed?.Invoke();
    }
}
