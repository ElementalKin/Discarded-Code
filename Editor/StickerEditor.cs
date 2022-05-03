using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Sticker))]
public class StickerEditor : Editor
{
    SerializedObject so;
    Sticker sticker;

    private void OnEnable()
    {
        so = serializedObject;
        sticker = (Sticker)target;
    }

    public override void OnInspectorGUI()
    {
        so.Update();
        EditorGUI.BeginChangeCheck();

        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
            EditorGUILayout.PropertyField(so.FindProperty(nameof(sticker.stickerName)));
            EditorGUILayout.PropertyField(so.FindProperty(nameof(sticker.stickerDescription)));
            EditorGUILayout.PropertyField(so.FindProperty(nameof(sticker.stickerArt)));
        }

        GUILayout.Space(5);

        if (sticker.stickerEffects.Count > 0)
        {
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                for (int i = 0; i < sticker.stickerEffects.Count; i++)
                {
                    using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                    {
                        SerializedObject soStickerEffect = new SerializedObject(sticker.stickerEffects[i]);
                        StickerEffect stickerEffect = sticker.stickerEffects[i];

                        soStickerEffect.Update();
                        EditorGUI.BeginChangeCheck();

                        EditorGUILayout.PropertyField(soStickerEffect.FindProperty(nameof(StickerEffect.stickerEffectType)));

                        if (stickerEffect.stickerEffectType == StickerEffectType.Support)
                        {
                            EditorGUILayout.PropertyField(soStickerEffect.FindProperty(nameof(StickerEffect.statusToEdit)));
                            EditorGUILayout.PropertyField(soStickerEffect.FindProperty(nameof(StickerEffect.applyToSelf)));
                        }

                        if (!(stickerEffect.stickerEffectType == StickerEffectType.None))
                        {
                            using (new GUILayout.HorizontalScope())
                            {
                                EditorGUILayout.PropertyField(soStickerEffect.FindProperty(nameof(CardEffect.appliedStacks)));
                            }
                        }

                        if (GUILayout.Button("Remove Effect"))
                        {
                            RemoveEffect(i);
                        }

                        if (EditorGUI.EndChangeCheck())
                        {
                            soStickerEffect.ApplyModifiedProperties();
                            Repaint();
                        }
                    }

                    if (i != sticker.stickerEffects.Count - 1)
                    {
                        GUILayout.Space(5);
                    }
                }
            }
        }

        if (GUILayout.Button("Add Effect"))
        {
            AddEffect();
        }

        if (EditorGUI.EndChangeCheck())
        {
            so.ApplyModifiedProperties();
            Repaint();
        }
    }

    private void AddEffect()
    {
        StickerEffect newStickerEffect = ScriptableObject.CreateInstance<StickerEffect>();
        newStickerEffect.name = $"Card Effect {sticker.stickerEffects.Count + 1}";
        sticker.stickerEffects.Add(newStickerEffect);

        AssetDatabase.AddObjectToAsset(newStickerEffect, sticker);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void RemoveEffect(int index)
    {
        AssetDatabase.RemoveObjectFromAsset(sticker.stickerEffects[index]);
        sticker.stickerEffects.RemoveAt(index);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
