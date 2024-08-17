using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState
{
    NONE,
    IDLE,
    DIE,
    ATTACK
}

public class CharacterStateManager : MonoBehaviour
{
    private CharacterState _state;

    public CharacterState State
    {
        get => _state;
        set => _state = value;
    }

    private void Awake()
    {
        CharacterAttack.setCharacterState += SetCharacterState;
    }

    private void OnDestroy()
    {
        CharacterAttack.setCharacterState -= SetCharacterState;
    }

    private void SetCharacterState(CharacterState state)
    {
        _state = state;
    }
}
