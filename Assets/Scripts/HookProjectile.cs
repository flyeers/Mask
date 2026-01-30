using System;
using DG.Tweening;
using UnityEngine;

public class HookProjectile : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _upDistance;
    [SerializeField] private float _upTime;
    [SerializeField] private Collider _collider;

    public event Action OnContact;

    private void Start()
    {
        MoveUp();
    }

    private void MoveUp()
    {
        DOVirtual.Float(0f, _upDistance, _upTime, distance =>
        {
            var origin = transform.position;
            var end = origin + Vector3.up * distance;

            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, origin);
            _lineRenderer.SetPosition(1, end);

            _collider.transform.position = end;
            
            Debug.Log(end);

        }).SetEase(Ease.InSine);
    }

    public void UpdateOrigin(float upDistance)
    {
        var origin = transform.position;
        var newOrigin = origin + Vector3.up * upDistance;

        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, origin);
    }
}
