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

    public void MoveUp()
    {
        _origin = transform.position;
        
        DOVirtual.Float(0f, _upDistance, _upTime, distance =>
        {
            var end = _origin + Vector3.up * distance;

            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, _origin);
            _lineRenderer.SetPosition(1, end);

            if (Physics.Raycast(_origin, end, out var hit, Mathf.Infinity, _ceilingLayer))
            {
                OnContact?.Invoke();
            }

        }).SetEase(Ease.InSine);
    }

    public void UpdateOrigin(float upDistance)
    {
        var newOrigin = _origin + Vector3.up * upDistance;

        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, newOrigin);
    }
}
