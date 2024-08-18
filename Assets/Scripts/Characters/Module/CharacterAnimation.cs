using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        CharacterMovement.setSpeedPropertyAnimation += SetSpeedPropertyAnimation;
        CharacterAttack.setCharacterAnimationIntProperty += SetIntPropertyAnimation;
        CharacterAttack.setCharacterAnimationFloatProperty += SetFloatPropertyAnimation;
        Enemy.setCharacterAnimationFloatProperty += SetFloatPropertyAnimation;
    }

    private void OnDestroy()
    {
        CharacterMovement.setSpeedPropertyAnimation -= SetSpeedPropertyAnimation;
        CharacterAttack.setCharacterAnimationIntProperty -= SetIntPropertyAnimation;
        CharacterAttack.setCharacterAnimationFloatProperty -= SetFloatPropertyAnimation;
        Enemy.setCharacterAnimationFloatProperty -= SetFloatPropertyAnimation;
    }

    private void SetAnimationState(int state)
    {
        _animator.SetInteger("State", state);
    }

    private void SetSpeedPropertyAnimation(int instanceId, float speed)
    {
        if (gameObject.GetInstanceID() == instanceId)
        {
            _animator.SetFloat("Speed", speed);
        }
    }

    private void SetIntPropertyAnimation(int instanceId, string name, int value)
    {
        if (gameObject.GetInstanceID() == instanceId)
        {
            _animator.SetInteger(name, value);
        }
    }

    private void SetFloatPropertyAnimation(int instanceId, string name, float value)
    {
        if (gameObject.GetInstanceID() == instanceId)
        {
            _animator.SetFloat(name, value);
        }
    }
}
