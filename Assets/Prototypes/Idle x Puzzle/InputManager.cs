using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region ACTION 
    public static event Action<ITile> spawnBlockEvent;
    #endregion

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                AnimationUtil.ScaleUpDown(hit.collider.transform, 1.05f, duration: 0.1f);

                spawnBlockEvent?.Invoke(hit.collider.GetComponent<ITile>());
            }
        }
    }
}
