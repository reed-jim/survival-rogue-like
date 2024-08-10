using PrimeTween;
using UnityEngine;

public class ExplosiveBullet : Bullet
{
    [Header("EXPLOSIVE")]
    [SerializeField] private GameObject explosiveArea;

    [Header("MODEL")]
    [SerializeField] private GameObject bulletModel;

    [Header("FX")]
    [SerializeField] private ParticleSystem explosionFx;

    protected override void Awake()
    {
        base.Awake();

        explosionFx.gameObject.SetActive(false);
        explosiveArea.SetActive(false);
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
}
