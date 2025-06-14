﻿// ActivePack Library
// Copyright (C) BigBlit Assets Michal Kalinowski
// http://bigblit.fun
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BigBlit.ShootingRange;
using System;

namespace BigBlit.ShotingRangeEditor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ScriptableAnimator))]
    public class ScriptableAnimatorEditor : Editor
    {

        internal static class Contents
        {
            public static readonly GUIContent AnimClipRange = EditorGUIUtility.TrTextContent("Clip Range", "Animation Clip Range");
        }

        SerializedProperty _spAnimationSmoothing;
        SerializedProperty _spAnimationClip;
        SerializedProperty _spInvert;
        SerializedProperty _spAnimClipRangeMin;
        SerializedProperty _spAnimClipRangeMax;
        SerializedProperty _spFreezePosition;
        SerializedProperty _spRelativePosition;

        private void OnEnable() {
            
        
        _spAnimationSmoothing = serializedObject.FindProperty("_animationSmoothing");
            _spAnimationClip = serializedObject.FindProperty("_animationClip");
            _spInvert = serializedObject.FindProperty("_invert");
            _spAnimClipRangeMin = serializedObject.FindProperty("_animClipRangeMin");
            _spAnimClipRangeMax = serializedObject.FindProperty("_animClipRangeMax");

        }
         
        public override void OnInspectorGUI() {
        
        serializedObject.UpdateIfRequiredOrScript();

            EditorGUILayout.PropertyField(_spAnimationClip);
            EditorGUILayout.PropertyField(_spAnimationSmoothing);
            EditorGUILayout.PropertyField(_spInvert);

            drawMinMaxSlider(Contents.AnimClipRange, _spAnimClipRangeMin, _spAnimClipRangeMax);

            serializedObject.ApplyModifiedProperties();
        }

        private void drawMinMaxSlider(GUIContent label, SerializedProperty spAnimMinTime, SerializedProperty spAnimMaxTime) {
            float min = spAnimMinTime.floatValue;
            float max = spAnimMaxTime.floatValue;

            Rect rect = GUILayoutUtility.GetRect(EditorGUIUtility.fieldWidth, 18.0f, 28.0f, 28.0f, EditorStyles.label);
            int controlId = GUIUtility.GetControlID(label, FocusType.Keyboard, rect);
           
            EditorGUI.PrefixLabel(new Rect(rect.x, rect.y, EditorGUIUtility.labelWidth, 28.0f), label);

            rect.x += EditorGUIUtility.labelWidth;
            rect.width -= EditorGUIUtility.labelWidth;
            EditorGUI.MinMaxSlider(new Rect(rect.x, rect.y+14, rect.width, 14.0f), ref min, ref max, 0.0f, 1.0f);
            min = EditorGUI.FloatField(new Rect(rect.x, rect.y, 42.0f, 14.0f), Mathf.Round(min * 1000f) / 1000f);
            max = EditorGUI.FloatField(new Rect(EditorGUIUtility.labelWidth+rect.width-42.0f, rect.y, 42.0f, 14.0f), Mathf.Round(max * 1000f) / 1000f);


            if (min != spAnimMinTime.floatValue) {
                spAnimMinTime.floatValue = min;
            }
            if (max != spAnimMaxTime.floatValue) {
                spAnimMaxTime.floatValue = max;
            }
        }


        private void drawToggleField(Rect position, GUIContent label, SerializedProperty spBool) {
            int indentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            bool newVal = EditorGUI.ToggleLeft(position, label, spBool.boolValue);
            EditorGUI.indentLevel = indentLevel;
            if (newVal != spBool.boolValue) {
                spBool.boolValue = newVal;
            }
        }
    }
}