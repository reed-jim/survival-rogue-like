using UnityEngine;

public class CustomDelegate
{
    public delegate bool BoolAction();
    public delegate Rigidbody GetRigidbodyAction();
    public delegate CharacterStat GetCharacterStatAction<in T>(T arg);
}
