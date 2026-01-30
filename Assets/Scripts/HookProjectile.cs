using System;
using DG.Tweening;
using UnityEngine;

public class HookProjectile : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _upDistance;
    [SerializeField] private float _upTime;
    [SerializeField] private LayerMask _ceilingLayer;

    public event Action OnContact;
    
    private Vector3 _origin;
    private Tween _tween;

    public void MoveUp()
    {
        _origin = transform.position;
        
        _tween = DOVirtual.Float(0f, _upDistance, _upTime, distance =>
        {
            var end = _origin + Vector3.up * distance;

            _lineRenderer.SetPosition(0, _origin);
            _lineRenderer.SetPosition(1, end);

            if (Physics.Raycast(_origin, Vector3.up, out var hit, Vector3.Distance(_origin, end), _ceilingLayer))
            {
                OnContact?.Invoke();
                _tween.Kill();
            }

        }).SetEase(Ease.InSine);
    }

    public void UpdateOrigin(float upDistance)
    {
        var newOrigin = _origin + Vector3.up * upDistance;

        _lineRenderer.SetPosition(0, newOrigin);
    }
}
