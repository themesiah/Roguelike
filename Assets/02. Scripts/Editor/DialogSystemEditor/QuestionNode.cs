using UnityEngine;
using System;
using Laresistance.EditorTools;
using System.Collections.Generic;
using UnityEditor;
using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;

namespace Laresistance.Systems.Dialog
{
    public class QuestionNode : BaseDialogNode
    {
        public QuestionNode(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle,
            GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint,
            Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode,
            Action<BaseDialogNode> OnNodeSelected, Action<BaseDialogNode> OnNodeUnselected,
            SingleDialogData singleDialogData) : 
            base(position, width, height, nodeStyle, selectedStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode, OnNodeSelected, OnNodeUnselected, singleDialogData)
        {
            SetTitle();
        }

        public override void Drag(Vector2 delta)
        {
            base.Drag(delta);
            singleDialogData.gridPosition += delta;
        }

        protected override void OnLeftClickInside()
        {
            base.OnLeftClickInside();
            OnNodeSelected?.Invoke(this);
        }

        protected override void OnLeftClickOutside()
        {
            base.OnLeftClickOutside();
            OnNodeUnselected?.Invoke(this);
        }

        protected override void SetTitle()
        {
            if (!string.IsNullOrEmpty(singleDialogData.questionDialog.dialogQuestionRef))
            {
                title = Texts.GetText(singleDialogData.questionDialog.dialogQuestionRef);
            }
            else
            {
                title = "EMPTY";
            }
        }

        public override void DrawDataBox()
        {
            EditorGUILayout.LabelField("Question and answers");

            EditorGUILayout.BeginHorizontal();
            singleDialogData.questionDialog.dialogQuestionRef = EditorGUILayout.TextField(singleDialogData.questionDialog.dialogQuestionRef);
            string localizedText = Texts.GetText(singleDialogData.questionDialog.dialogQuestionRef);
            EditorGUILayout.LabelField(new GUIContent(localizedText, localizedText));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            singleDialogData.questionDialog.answer1TextRef = EditorGUILayout.TextField(singleDialogData.questionDialog.answer1TextRef);
            string localizedText2 = Texts.GetText(singleDialogData.questionDialog.answer1TextRef);
            EditorGUILayout.LabelField(new GUIContent(localizedText2, localizedText2));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            singleDialogData.questionDialog.answer2TextRef = EditorGUILayout.TextField(singleDialogData.questionDialog.answer2TextRef);
            string localizedText3 = Texts.GetText(singleDialogData.questionDialog.answer2TextRef);
            EditorGUILayout.LabelField(new GUIContent(localizedText3, localizedText3));
            EditorGUILayout.EndHorizontal();

            SetTitle();
            EditorGUILayout.LabelField("Required variables");
            int requiredVars = EditorGUILayout.IntField(singleDialogData.RequiredVariables.Count);
            if (singleDialogData.RequiredVariables.Count != requiredVars)
            {
                while (singleDialogData.RequiredVariables.Count < requiredVars)
                {
                    singleDialogData.RequiredVariables.Add(new DialogVariablesStatus.VariableData() { name = "Var name", value = 0 });
                }
                while (singleDialogData.RequiredVariables.Count > requiredVars)
                {
                    singleDialogData.RequiredVariables.RemoveAt(singleDialogData.RequiredVariables.Count - 1);
                }
            }
            for (int i = 0; i < requiredVars; ++i)
            {
                EditorGUILayout.BeginHorizontal();
                singleDialogData.RequiredVariables[i].name = EditorGUILayout.TextField(singleDialogData.RequiredVariables[i].name);
                singleDialogData.RequiredVariables[i].value = EditorGUILayout.IntField(singleDialogData.RequiredVariables[i].value);
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.LabelField("Variable changes");
            int varChanges = EditorGUILayout.IntField(singleDialogData.questionDialog.variablesChange.Count);
            if (singleDialogData.questionDialog.variablesChange.Count != varChanges)
            {
                while (singleDialogData.questionDialog.variablesChange.Count < varChanges)
                {
                    singleDialogData.questionDialog.variablesChange.Add(new DialogVariablesStatus.VariableData() { name = "Var name", value = 0 });
                }
                while (singleDialogData.questionDialog.variablesChange.Count > varChanges)
                {
                    singleDialogData.questionDialog.variablesChange.RemoveAt(singleDialogData.questionDialog.variablesChange.Count - 1);
                }
            }
            for (int i = 0; i < varChanges; ++i)
            {
                EditorGUILayout.BeginHorizontal();
                singleDialogData.questionDialog.variablesChange[i].name = EditorGUILayout.TextField(singleDialogData.questionDialog.variablesChange[i].name);
                singleDialogData.questionDialog.variablesChange[i].value = EditorGUILayout.IntField(singleDialogData.questionDialog.variablesChange[i].value);
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}