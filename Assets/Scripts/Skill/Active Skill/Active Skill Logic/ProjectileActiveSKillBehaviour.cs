using System.Collections;
using System.Collections.Generic;
using ReedJim.RPG.Stat;
using UnityEngine;

public enum ActiveSkillIdentifer
{
    RedEnergyExplosion,
    EnergySlashProjectile,
    ChainLighning
}

public class ProjectiveActiveSkillBehaviour : MonoBehaviour
{
    [SerializeField] private ActiveSkillIdentifer activeSkillIdentifer;
    [SerializeField] private GameObject caster;
    private int _casterInstanceId;

    private Transform _target;

    #region PRIVATE FIELD
    private bool _isActivated;
    private CharacterStat _stat;
    #endregion

    private void Awake()
    {
        ProjectileActiveSkill.activateActiveSkillEvent += ActivateSkill;
        ProjectileActiveSkill.castActiveSkillEvent += Cast;
        CharacterVision.setTargetEvent += SetTarget;

        _casterInstanceId = caster.GetInstanceID();
    }

    private void OnDestroy()
    {
        ProjectileActiveSkill.activateActiveSkillEvent -= ActivateSkill;
        ProjectileActiveSkill.castActiveSkillEvent -= Cast;
        CharacterVision.setTargetEvent -= SetTarget;
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
        if (_target == null)
        {
            return;
        }

        if (activeSkillIdentifer != this.activeSkillIdentifer)
        {
            return;
        }

        if (!_isActivated)
        {
            return;
        }

        Bullet projectile = ObjectPoolingEverything.GetFromPool("Thunder Slash Projectile").GetComponent<Bullet>();

        projectile.SetAttackInstanceId(_casterInstanceId);

        projectile.gameObject.SetActive(true);

        projectile.Shoot(_target, caster.transform.position + 2 * caster.transform.forward, _casterInstanceId);
    }

    private void SetTarget(int instanceId, Transform target)
    {
        if (instanceId == caster.GetInstanceID())
        {
            _target = target;
        }
    }
}
