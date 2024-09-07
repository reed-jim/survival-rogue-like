using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;

public class ChainLighning : MonoBehaviour
{
    [Header("CUSTOMIZE")]
    [SerializeField] private float radiusCheck;
    [SerializeField] private LayerMask layerMaskCheck;

    #region PRIVATE FIELD
    private List<int> _checkedInstaceIds;
    #endregion

    private void Awake()
    {
        _checkedInstaceIds = new List<int>();

        FindEnemy();
    }

    public void FindEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radiusCheck, layerMaskCheck);

        int instanceId;

        foreach (var collider in colliders)
        {
            instanceId = collider.transform.GetInstanceID();

            if (!_checkedInstaceIds.Contains(instanceId))
            {
                MoveToNextTarget(collider.transform);

                _checkedInstaceIds.Add(instanceId);

                break;
            }
        }
    }

    private void MoveToNextTarget(Transform target)
    {
        Tween.LocalPosition(transform, target.position, duration: 0.3f)
        .OnComplete(() => FindEnemy());
    }
}
