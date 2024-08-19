using System;
using UnityEngine;
using static CustomDelegate;

public class CharacterRangedAttack : MonoBehaviour, ICharacterAttack
{
    [Header("CUSTOMIZE")]
    [SerializeField] private float forceMultiplier;

    public static event GetRigidbodyAction getRigidbodyEvent;
    public static event Action<int, int> setBulletAttackerInstanceId;

    private void Awake()
    {
        CharacterVision.attackEnemyEvent += RangedAttack;
    }

    private void OnDestroy()
    {
        CharacterVision.attackEnemyEvent -= RangedAttack;
    }

    public void Attack()
    {
        // Not neccesary to use interface here, cuz event already used
        // RangedAttack();
    }

    private void RangedAttack(int instanceId, Transform target)
    {
        if (instanceId == gameObject.GetInstanceID())
        {
            Rigidbody bullet = getRigidbodyEvent?.Invoke();

            bullet.gameObject.SetActive(true);

            setBulletAttackerInstanceId?.Invoke(bullet.gameObject.GetInstanceID(), gameObject.GetInstanceID());

            Vector3 shotPosition = transform.position + new Vector3(0, 2, 0);

            bullet.transform.position = shotPosition;

            bullet.AddForce(forceMultiplier * (target.position - shotPosition));
        }
    }
}
