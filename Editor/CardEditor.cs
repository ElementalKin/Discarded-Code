using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Card))]
public class CardEditor : Editor
{
    SerializedObject so;
    Card card;

    private void OnEnable()
    {
        so = serializedObject;
        card = (Card)target;
    }

    public override void OnInspectorGUI()
    {
        so.Update();
        EditorGUI.BeginChangeCheck();

        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
            GUILayout.Label("Card General", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(so.FindProperty(nameof(card.cardName)));
            //EditorGUILayout.PropertyField(so.FindProperty(nameof(card.cardDescription)));
            EditorGUILayout.PropertyField(so.FindProperty(nameof(card.cardArt)));
            //EditorGUILayout.PropertyField(so.FindProperty(nameof(card.cardWatermark)));
            EditorGUILayout.PropertyField(so.FindProperty(nameof(card.cardFrame)));
            EditorGUILayout.PropertyField(so.FindProperty(nameof(card.bodyPart)));
            EditorGUILayout.PropertyField(so.FindProperty(nameof(card.animationOnPlay)));

            EditorGUILayout.PropertyField(so.FindProperty(nameof(card.cardCost)));
            //EditorGUILayout.PropertyField(so.FindProperty(nameof(card.cardID)));

            //EditorGUILayout.PropertyField(so.FindProperty(nameof(card.cardUpgrade)));

            //EditorGUILayout.PropertyField(so.FindProperty(nameof(card.isFinisher)));

            EditorGUILayout.PropertyField(so.FindProperty(nameof(card.isTrapCard)));
        }

        GUILayout.Space(5);

        using (new GUILayout.VerticalScope(EditorStyles.helpBox))
        {
            GUILayout.Label("Card Sounds", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(so.FindProperty(nameof(card.attackSounds)));
        }

        GUILayout.Space(5);

        if (card.cardEffects.Count > 0)
        {
            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {
                GUILayout.Label("Card Effects", EditorStyles.boldLabel);
                for (int i = 0; i < card.cardEffects.Count; i++)
                {
                    using (new GUILayout.VerticalScope(EditorStyles.helpBox))
                    {
                        SerializedObject soCardEffect = new SerializedObject(card.cardEffects[i]);
                        CardEffect cardEffect = card.cardEffects[i];

                        soCardEffect.Update();
                        EditorGUI.BeginChangeCheck();

                        EditorGUILayout.PropertyField(soCardEffect.FindProperty(nameof(CardEffect.effectType)));
                        EditorGUILayout.PropertyField(soCardEffect.FindProperty(nameof(CardEffect.applyToSelf)));

                        if (cardEffect.effectType == CardHelpers.EffectType.Support)
                        {
                            EditorGUILayout.PropertyField(soCardEffect.FindProperty(nameof(CardEffect.statusToEdit)));
                        }

                        if (cardEffect.effectType == CardHelpers.EffectType.DeckEdit)
                        {
                            EditorGUILayout.PropertyField(soCardEffect.FindProperty(nameof(CardEffect.DeckAction)));

                            if (cardEffect.DeckAction == CardHelpers.DeckModify.AddSpecificCardToDeck)
                            {
                                EditorGUILayout.PropertyField(soCardEffect.FindProperty(nameof(CardEffect.cardToAdd)));
                            }
                        }

                        if (!(cardEffect.effectType == CardHelpers.EffectType.None))
                        {
                            using (new GUILayout.HorizontalScope())
                            {
                                EditorGUILayout.PropertyField(soCardEffect.FindProperty(nameof(CardEffect.referenceVariable)));
                                EditorGUILayout.PropertyField(soCardEffect.FindProperty(nameof(CardEffect.applyMultipleTimes)));
                            }

                            if (!cardEffect.referenceVariable)
                            {
                                EditorGUILayout.PropertyField(soCardEffect.FindProperty(nameof(CardEffect.appliedStacks)));
                            }
                            else
                            {
                                EditorGUILayout.PropertyField(soCardEffect.FindProperty(nameof(CardEffect.variable)));
                            }

                            if (cardEffect.applyMultipleTimes)
                            {
                                EditorGUILayout.PropertyField(soCardEffect.FindProperty(nameof(CardEffect.effectNumberTimes)));
                            }
                        }

                        if (GUILayout.Button("Remove Effect"))
                        {
                            RemoveEffect(i);
                        }

                        if (EditorGUI.EndChangeCheck())
                        {
                            soCardEffect.ApplyModifiedProperties();
                            Repaint();
                        }
                    }

                    if (i != card.cardEffects.Count - 1)
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
        CardEffect newCardEffect = ScriptableObject.CreateInstance<CardEffect>();
        newCardEffect.name = $"Card Effect {card.cardEffects.Count + 1}";
        card.cardEffects.Add(newCardEffect);

        AssetDatabase.AddObjectToAsset(newCardEffect, card);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void RemoveEffect(int index)
    {
        AssetDatabase.RemoveObjectFromAsset(card.cardEffects[index]);
        card.cardEffects.RemoveAt(index);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}