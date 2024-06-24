using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private GameObject fx;

    [SerializeField] private PlayerRuntime playerRuntime;

    private void Update()
    {
        FindPlayer();
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            // fx.transform.position = contact.point;
            // fx.SetActive(true);

            gameObject.SetActive(false);

            break;
        }
    }

    private void FindPlayer()
    {
        transform.LookAt(player);

        transform.position = Vector3.Lerp(transform.position, playerRuntime.player.position, 0.002f);
    }
}
