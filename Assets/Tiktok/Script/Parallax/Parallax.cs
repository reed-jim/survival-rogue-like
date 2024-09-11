using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ParallaxItem
{
    [SerializeField] private Transform target;
    [SerializeField] private float virtualDistanceToViewer;

    public Transform Target => target;
    public float VirtualDistanceToViewer => virtualDistanceToViewer;

    public ParallaxItem(Transform target, float virtualDistanceToViewer)
    {
        this.target = target;
        this.virtualDistanceToViewer = virtualDistanceToViewer;
    }
}

public class Parallax : MonoBehaviour
{
    [SerializeField] private GameObject containerPrefab;

    [Header("CUSTOMIZE")]
    [SerializeField] private float speedMultiplier;

    #region PRIVATE FIELD
    [SerializeField] private ParallaxItem[] parallaxItems;
    private List<ParallaxItem> _tempParallaxItems;
    #endregion

    private void Awake()
    {
        Init();

        StartCoroutine(RunParallax());
    }

    private void Init()
    {
        _tempParallaxItems = new List<ParallaxItem>();

        foreach (var item in parallaxItems)
        {
            Transform container = Instantiate(containerPrefab).transform;

            container.name = $"Container {item.Target.name}";

            Transform clone1 = Instantiate(item.Target, container.transform);
            Transform clone2 = Instantiate(item.Target, container.transform);

            item.Target.SetParent(container.transform);

            clone1.position += new Vector3(20, 0, 0);
            clone2.position -= new Vector3(20, 0, 0);

            _tempParallaxItems.Add(new ParallaxItem(item.Target, item.VirtualDistanceToViewer));
            _tempParallaxItems.Add(new ParallaxItem(clone1, item.VirtualDistanceToViewer));
            _tempParallaxItems.Add(new ParallaxItem(clone2, item.VirtualDistanceToViewer));
        }

        parallaxItems = _tempParallaxItems.ToArray();
    }

    private IEnumerator RunParallax()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.02f);

        while (true)
        {
            for (int i = 0; i < parallaxItems.Length; i++)
            {
                parallaxItems[i].Target.position -= speedMultiplier * (1 / parallaxItems[i].VirtualDistanceToViewer) * Vector3.right;

                if (parallaxItems[i].Target.position.x < -23)
                {
                    parallaxItems[i].Target.position = new Vector3(23, parallaxItems[i].Target.position.y, parallaxItems[i].Target.position.z);
                }
            }

            yield return waitForSeconds;
        }
    }
}
