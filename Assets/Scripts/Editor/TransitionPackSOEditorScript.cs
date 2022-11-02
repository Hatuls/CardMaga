using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TransitionPackSO))]
public class TransitionPackSOEditorScript : Editor
{
    public override void OnInspectorGUI()
    {
        var obj = (TransitionPackSO)target;

        #region Movement

        obj.HaveMovement = EditorGUILayout.Toggle("Have Movement", obj.HaveMovement);

        if (obj.HaveMovement)
        {
            switch (obj.MovePositionType = (TransitionPackSO.PositionType)EditorGUILayout.EnumPopup(obj.MovePositionType))
            {
                case TransitionPackSO.PositionType.AnchoredPosition:
                    break;
                case TransitionPackSO.PositionType.WordPosition:
                    break;
                case TransitionPackSO.PositionType.LocalPosition:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            EditorGUILayout.Space();
            obj.MoveOffSet =
                EditorGUILayout.Vector3Field("Move Off Set:", obj.MoveOffSet);
            EditorGUILayout.Space();
            obj.Movement.TimeToTransition =
                EditorGUILayout.FloatField("Time To Transition:", obj.Movement.TimeToTransition);
            EditorGUILayout.Space();
            obj.Movement.AnimationCurveX = EditorGUILayout.CurveField("AnimationCurveX", obj.Movement.AnimationCurveX);
            obj.Movement.AnimationCurveY = EditorGUILayout.CurveField("AnimationCurveY", obj.Movement.AnimationCurveY);
            obj.Movement.AnimationCurveZ = EditorGUILayout.CurveField("AnimationCurveZ", obj.Movement.AnimationCurveZ);
        }

        #endregion

        #region Scale

        EditorGUILayout.Space();
        obj.HaveScale = EditorGUILayout.Toggle("Have Scale:", obj.HaveScale);

        if (obj.HaveScale)
        {
            switch (obj.ScaleType = (TransitionPackSO.ScaleTypeEnum)EditorGUILayout.EnumPopup(obj.ScaleType))
            {
                case TransitionPackSO.ScaleTypeEnum.ByFloat:
                    obj.ScaleMultiplier = EditorGUILayout.FloatField("Scale Multiplier:", obj.ScaleMultiplier);
                    break;
                case TransitionPackSO.ScaleTypeEnum.ByVector:
                    obj.ScaleVector = EditorGUILayout.Vector3Field("Scale Vector:", obj.ScaleVector);
                    break;
            }

            EditorGUILayout.Space();
            obj.Scale.TimeToTransition = EditorGUILayout.FloatField("Time To Transition:", obj.Scale.TimeToTransition);
            EditorGUILayout.Space();
            obj.Scale.AnimationCurveX = EditorGUILayout.CurveField("AnimationCurveX", obj.Scale.AnimationCurveX);
            obj.Scale.AnimationCurveY = EditorGUILayout.CurveField("AnimationCurveY", obj.Scale.AnimationCurveY);
            obj.Scale.AnimationCurveZ = EditorGUILayout.CurveField("AnimationCurveZ", obj.Scale.AnimationCurveZ);
        }

        #endregion

        #region Rotation

        EditorGUILayout.Space();
        obj.HaveRotation = EditorGUILayout.Toggle("Have Rotation:", obj.HaveRotation);

        if (obj.HaveRotation)
        {
            obj.Rotate = EditorGUILayout.Vector3Field("Rotation Vector", obj.Rotate);
            EditorGUILayout.Space();
            obj.Rotation.TimeToTransition =
                EditorGUILayout.FloatField("Time To Transition:", obj.Rotation.TimeToTransition);
            EditorGUILayout.Space();
            obj.Rotation.AnimationCurveX = EditorGUILayout.CurveField("AnimationCurveX", obj.Rotation.AnimationCurveX);
            obj.Rotation.AnimationCurveY = EditorGUILayout.CurveField("AnimationCurveY", obj.Rotation.AnimationCurveY);
            obj.Rotation.AnimationCurveZ = EditorGUILayout.CurveField("AnimationCurveZ", obj.Rotation.AnimationCurveZ);
        }

        #endregion

        if (GUILayout.Button("Save"))
        {
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
            Debug.Log(target.name + " Save");
        }
    }
}