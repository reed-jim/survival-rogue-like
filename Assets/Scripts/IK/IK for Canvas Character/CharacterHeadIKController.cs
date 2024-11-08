using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHeadIKController : MonoBehaviour
{
    [SerializeField] private Transform source;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mouseScreenPosition = Input.mousePosition;

            mouseScreenPosition.z = Camera.main.nearClipPlane;

            Debug.Log(mouseScreenPosition);

            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

            mouseWorldPosition.z = source.position.z;

            source.position = mouseWorldPosition;
        }
    }
}
