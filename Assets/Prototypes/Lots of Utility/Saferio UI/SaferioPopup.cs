using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SaferioPopup : MonoBehaviour
{
    [SerializeReference] private ISaferioUIAnimation[] transitionAnimations;

    public ISaferioUIAnimation[] TransitionAnimations
    {
        get => transitionAnimations;
        set => transitionAnimations = value;
    }

    // protected abstract void Show();

    private void OnValidate()
    {
        for (int i = 0; i < transitionAnimations.Length; i++)
        {
            if (transitionAnimations[i] == null)
            {
                transitionAnimations[i] = new SaferioSlideAnimation();

                if (i == 1)
                {
                    transitionAnimations[i] = new SaferioFadeAnimation();
                }
            }
        }
    }

    protected virtual void Show()
    {
        gameObject.SetActive(true);

        foreach (var transitionAnimation in transitionAnimations)
        {
            if (transitionAnimation != null)
            {
                transitionAnimation.Play();
            }
        }
    }

    // protected abstract void Close();
}

// [CustomEditor(typeof(SaferioPopup))]
// public class SaferioPopupEditor : Editor
// {
//     SaferioPopup popup;
//     int[] selectedIndexs;
//     int[] previousSelectedIndexs;

//     public override void OnInspectorGUI()
//     {
//         popup = (SaferioPopup)target;

//         DrawDefaultInspector();

//         SerializedProperty referenceProperty = serializedObject.FindProperty("transitionAnimations");

//         selectedIndexs = new int[1];

//         ShowReferenceSelection(referenceProperty);
//         serializedObject.ApplyModifiedProperties();
//     }

//     private void ShowReferenceSelection(SerializedProperty referenceProperty)
//     {
//         // ISaferioUIAnimation currentReference = (ISaferioUIAnimation)referenceProperty.objectReferenceValue;

//         System.Type[] derivedTypes = new System.Type[] { typeof(SaferioSlideAnimation), typeof(SaferioFadeAnimation) };
//         string[] typeNames = new string[derivedTypes.Length];
//         for (int i = 0; i < derivedTypes.Length; i++) typeNames[i] = derivedTypes[i].Name;

//         // int currentIndex = -1;
//         // if (referenceProperty.objectReferenceValue != null)
//         // {
//         //     for (int i = 0; i < derivedTypes.Length; i++)
//         //     {
//         //         if (derivedTypes[i] == currentReference.GetType())
//         //         {
//         //             currentIndex = i;
//         //             break;
//         //         }
//         //     }
//         // }

//         if (selectedIndexs == null)
//         {
//             return;
//         }

//         for (int i = 0; i < selectedIndexs.Length; i++)
//         {
//             selectedIndexs[i] = EditorGUILayout.Popup($"Select Reference Type {i}", selectedIndexs[i], typeNames);

//             // Debug.Log(i + " / " + selectedIndexs[i]);
//         }

//         // if (previousSelectedIndexs != null)
//         // {
//         //     for (int i = 0; i < previousSelectedIndexs.Length; i++)
//         //     {
//         //         if (selectedIndexs[i] != previousSelectedIndexs[i])
//         //         {
//         //             Debug.Log(selectedIndexs[i]);
//         //             popup.TransitionAnimations[i] = CreateConcreteInstance(selectedIndexs[i]);
//         //             Debug.Log(popup.TransitionAnimations[i]);
//         //         }
//         //     }
//         // }

//         previousSelectedIndexs = selectedIndexs;

//         // if (selectedIndex != currentIndex)
//         // {
//         //     Debug.Log(selectedIndex);
//         //     // System.Type selectedType = derivedTypes[selectedIndex];
//         //     // referenceProperty.objectReferenceValue = CreateInstance(selectedType);
//         // }
//     }

//     private ISaferioUIAnimation CreateConcreteInstance(int index)
//     {
//         Debug.Log(index);
//         if (index == 0) return new SaferioSlideAnimation();
//         if (index == 1) return new SaferioFadeAnimation();
//         return null;
//     }
// }
