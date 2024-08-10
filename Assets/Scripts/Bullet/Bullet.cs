using PrimeTween;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("MANAGEMENT")]
    [SerializeField] protected TrailRenderer bulletTrail;
    protected Rigidbody _rigidBody;

    protected virtual void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    protected virtual void OnEnable()
    {
        Tween.Delay(0.5f).OnComplete(() => gameObject.SetActive(false));
    }

    private void OnDisable()
    {
        bulletTrail.Clear();
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != Constants.PLAYER_TAG)
        {
            gameObject.SetActive(false);
        }
    }
}
