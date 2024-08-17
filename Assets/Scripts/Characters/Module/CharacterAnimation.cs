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
    }

    private void OnDestroy()
    {
        CharacterMovement.setSpeedPropertyAnimation -= SetSpeedPropertyAnimation;
        CharacterAttack.setCharacterAnimationIntProperty -= SetIntPropertyAnimation;
        CharacterAttack.setCharacterAnimationFloatProperty -= SetFloatPropertyAnimation;
    }

    private void SetAnimationState(int state)
    {
        _animator.SetInteger("State", state);
    }

    private void SetSpeedPropertyAnimation(float speed)
    {
        _animator.SetFloat("Speed", speed);
    }

    private void SetIntPropertyAnimation(string name, int value)
    {
        _animator.SetInteger(name, value);
    }

    private void SetFloatPropertyAnimation(string name, float value)
    {
        _animator.SetFloat(name, value);
    }
}
