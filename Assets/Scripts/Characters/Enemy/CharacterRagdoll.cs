using UnityEngine;

public class CharacterRagdoll : MonoBehaviour
{
    [Header("RAGDOLL")]
    [SerializeField] private Collider[] ragdollColliders;
    private Rigidbody[] _ragdollRigibodies;

    #region PRIVATE FIELD
    private Animator _playerAnimator;
    private Rigidbody _rigidBody;
    #endregion

    private void Awake()
    {
        _playerAnimator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody>();

        SetUpRagdoll();
        EnableRagdoll(false);
    }

    #region RAGDOLL
    private void SetUpRagdoll()
    {
        _ragdollRigibodies = new Rigidbody[ragdollColliders.Length];

        for (int i = 0; i < ragdollColliders.Length; i++)
        {
            _ragdollRigibodies[i] = ragdollColliders[i].GetComponent<Rigidbody>();
        }
    }

    public void EnableRagdoll(bool enableRagdoll)
    {
        if (_playerAnimator == null)
        {
            _playerAnimator = GetComponent<Animator>();
            _rigidBody = GetComponent<Rigidbody>();

            SetUpRagdoll();
        }

        _playerAnimator.enabled = !enableRagdoll;

        foreach (Collider item in ragdollColliders)
        {
            item.enabled = false;
        }

        foreach (var ragdollRigidBody in _ragdollRigibodies)
        {
            ragdollRigidBody.useGravity = enableRagdoll;
            ragdollRigidBody.isKinematic = !enableRagdoll;
        }

        _rigidBody.velocity = Vector3.zero;
    }
    #endregion
}
