using System.Collections;
using System.Collections.Generic;
using PrimeTween;
using UnityEngine;
using static CustomDelegate;

public class Meteor : MonoBehaviour
{
    [Header("EXPLOSIVE")]
    [SerializeField] private GameObject explosiveArea;
    [SerializeField] private SphereCollider explosiveCollider;

    [Header("MODEL")]
    [SerializeField] private GameObject bulletModel;

    [Header("FX")]
    [SerializeField] private ParticleSystem explosionFx;

    #region PRIVATE FIELD
    private Rigidbody _rigidBody;
    #endregion

    #region ACTION
    public static GetExplosiveAreaIndicatorAction getExplosiveAreaIndicatorAction;
    #endregion

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();

        explosionFx.gameObject.SetActive(false);
        explosiveArea.SetActive(false);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Reset();

        Tween.Delay(3f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
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
