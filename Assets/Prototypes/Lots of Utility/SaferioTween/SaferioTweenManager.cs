using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Saferio.Util.SaferioTween
{
    public class SaferioTweenManager : MonoBehaviour
    {
        private static SaferioTweenManager instance;

        [SerializeField] private List<string> lists;

        public List<string> ListTween => lists;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {

            }

            lists = new List<string>();
        }

        public static void RunCoroutine(IEnumerator tween, Object target)
        {
            instance.StartCoroutine(tween);

            instance.lists.Add(target.name);
        }

        public static void RunCoroutine(IEnumerator tween)
        {
            instance.StartCoroutine(tween);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SaferioTweenManager))]
    public class SaferioTweenManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            SaferioTweenManager saferioTweenManager = (SaferioTweenManager)target;

            EditorGUI.BeginDisabledGroup(true);

            if (saferioTweenManager.ListTween != null)
            {
                foreach (var item in saferioTweenManager.ListTween)
                {
                    EditorGUILayout.LabelField(item.ToString());
                }
            }

            EditorGUI.EndDisabledGroup();
        }
    }
#endif
}
