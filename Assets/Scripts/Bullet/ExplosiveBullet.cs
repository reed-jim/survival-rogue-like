using PrimeTween;
using UnityEngine;
using static CustomDelegate;

public class ExplosiveBullet : Bullet
{
    [Header("EXPLOSIVE")]
    [SerializeField] private GameObject explosiveArea;
    [SerializeField] private SphereCollider explosiveCollider;

    [Header("MODEL")]
    [SerializeField] private GameObject bulletModel;

    [Header("FX")]
    [SerializeField] private ParticleSystem explosionFx;

    #region ACTION
    public static GetExplosiveAreaIndicatorAction getExplosiveAreaIndicatorAction;
    #endregion

    protected override void Awake()
    {
        base.Awake();

        explosionFx.gameObject.SetActive(false);
        explosiveArea.SetActive(false);
        gameObject.SetActive(false);
    }

    protected override void OnEnable()
    {
        Reset();

        Tween.Delay(3f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        Explode();

        bulletTrail.Clear();
    }

    private void Reset()
    {
        bulletModel.SetActive(true);
        explosiveArea.SetActive(false);
        explosionFx.gameObject.SetActive(false);

        _rigidBody.useGravity = true;
        _rigidBody.isKinematic = false;
    }

    private void Explode()
    {
        explosionFx.gameObject.SetActive(true);
        explosionFx.Play();

        bulletModel.SetActive(false);
        explosiveArea.SetActive(true);

        Tween.Delay(0.2f).OnComplete(() => explosiveArea.SetActive(false));

        _rigidBody.velocity = Vector3.zero;
        _rigidBody.useGravity = false;
        _rigidBody.isKinematic = true;
    }

    public override void Shoot(Transform target, Vector3 shotPosition, int attackInstanceId)
    {
        gameObject.SetActive(true);

        SetAttackInstanceId(attackInstanceId);

        transform.position = shotPosition;

        Vector3 shootDirection = target.position - shotPosition;

        shootDirection.y = 1;

        _rigidBody.AddForce(forceMultiplier * shootDirection);

        PredictTrajectory(shotPosition, forceMultiplier * shootDirection, shootDirection);
    }

    private void PredictTrajectory(Vector3 shotLocation, Vector3 force, Vector3 direction)
    {
        float deltaTime = Time.fixedDeltaTime;

        Vector3 velocity = force / _rigidBody.mass * deltaTime;

        for (float i = 0; i < 10; i += deltaTime)
        {
            Vector3 position = new Vector3();

            position.x = shotLocation.x + velocity.x * i;
            position.y = shotLocation.y + (velocity.y * i - 0.5f * Mathf.Abs(Physics.gravity.y) * i * i);
            position.z = shotLocation.z + velocity.z * i;

            if (position.y <= 0)
            {
                ExplosiveAreaIndicator explosiveAreaIndicator = getExplosiveAreaIndicatorAction?.Invoke();

                float countdown = i;

                explosiveAreaIndicator.ShowExplosiveArea(position, explosiveCollider.radius, countdown);

                break;
            }
        }
    }
}
