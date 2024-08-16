using UnityEngine;

public interface IExplodable
{
    public void Explode();
}

public class Barrel : MonoBehaviour, IExplodable
{
    public void Explode()
    {
        
    }
}
