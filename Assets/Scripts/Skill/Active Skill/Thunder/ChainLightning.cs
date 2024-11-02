using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using ReedJim.RPG.Stat;
using UnityEngine;

public class ChainLighning : MonoBehaviour
{
    [SerializeField] private ActiveSkillIdentifer activeSkillIdentifer;
    [SerializeField] private GameObject caster;

    [Header("CUSTOMIZE")]
    [SerializeField] private float radiusCheck;
    [SerializeField] private LayerMask layerMaskCheck;

    #region PRIVATE FIELD
    private bool _isActivated;
    private int _casterInstanceId;
    private List<int> _checkedInstaceIds;

    private bool _isChaining;
    private int _numTargetHit;

    private ChainLightningComponent _chainLightningComponent;
    private int _chainLightningComponentInstanceId;

    private CharacterStat _stat;
    #endregion

    private void Awake()
    {
        ProjectileActiveSkill.activateActiveSkillEvent += ActivateSkill;
        ProjectileActiveSkill.castActiveSkillEvent += Cast;

        _checkedInstaceIds = new List<int>();

        // gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        ProjectileActiveSkill.activateActiveSkillEvent -= ActivateSkill;
        ProjectileActiveSkill.castActiveSkillEvent -= Cast;
    }

    private void ActivateSkill(ActiveSkillIdentifer activeSkillIdentifer, CharacterStat stat)
    {
        if (activeSkillIdentifer == this.activeSkillIdentifer)
        {
            _stat = stat;
            
            _isActivated = true;
        }
    }

    private void Cast(ActiveSkillIdentifer activeSkillIdentifer)
    {
        if (activeSkillIdentifer != this.activeSkillIdentifer)
        {
            return;
        }

        if (!_isActivated)
        {
            return;
        }

        if (_isChaining)
        {
            return;
        }

        _chainLightningComponent = ObjectPoolingEverything.GetFromPool("Chain Lightning").GetComponent<ChainLightningComponent>();

        _chainLightningComponent.SetStat(_stat);

        _chainLightningComponent.SetActive(true);

        transform.position = caster.transform.position;

        _isChaining = true;

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
                // MoveToNextTarget(collider.transform);

                _chainLightningComponent.ChainTo(collider.transform.position);

                _checkedInstaceIds.Add(instanceId);

                break;
            }
        }

        if (colliders.Length == 0)
        {
            _chainLightningComponent.SetActive(false);

            _numTargetHit = 0;

            _isChaining = false;
        }
    }

    // private void MoveToNextTarget(Transform target)
    // {
    //     Tween.LocalPosition(transform, target.position, duration: 1.5f)
    //     .OnComplete(() =>
    //     {
    //         if (_numTargetHit < 3)
    //         {
    //             _numTargetHit++;

    //             FindEnemy();
    //         }
    //         else
    //         {
    //             gameObject.SetActive(false);

    //             _numTargetHit = 0;

    //             _isChaining = false;
    //         }
    //     }
    //     );
    // }
}
