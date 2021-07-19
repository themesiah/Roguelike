using UnityEngine;
using System;
using Laresistance.EditorTools;
using System.Collections.Generic;
using UnityEditor;
using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;

namespace Laresistance.Systems.Dialog
{
    public class DialogNode : BaseDialogNode
    {

        public DialogNode(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle,
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
            if (singleDialogData.dialog.dialogTextRef.Count > 0)
            {
                title = Texts.GetText(singleDialogData.dialog.dialogTextRef[0]);
            }
            else
            {
                title = "EMPTY";
            }
        }

        public override void DrawDataBox()
        {
            EditorGUILayout.LabelField("Dialogs");
            int dialogs = EditorGUILayout.IntField(singleDialogData.dialog.dialogTextRef.Count);
            if (singleDialogData.dialog.dialogTextRef.Count != dialogs)
            {
                while (singleDialogData.dialog.dialogTextRef.Count < dialogs)
                {
                    singleDialogData.dialog.dialogTextRef.Add("NEW TEXT");
                }
                while (singleDialogData.dialog.dialogTextRef.Count > dialogs)
                {
                    singleDialogData.dialog.dialogTextRef.RemoveAt(singleDialogData.dialog.dialogTextRef.Count - 1);
                }
            }
            for (int i = 0; i < dialogs; ++i)
            {
                EditorGUILayout.BeginHorizontal();
                singleDialogData.dialog.dialogTextRef[i] = EditorGUILayout.TextField(singleDialogData.dialog.dialogTextRef[i]);
                string localizedText = Texts.GetText(singleDialogData.dialog.dialogTextRef[i]);
                EditorGUILayout.LabelField(new GUIContent(localizedText, localizedText));
                EditorGUILayout.EndHorizontal();
            }
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
            int varChanges = EditorGUILayout.IntField(singleDialogData.VariableChanges.Count);
            if (singleDialogData.VariableChanges.Count != varChanges)
            {
                while (singleDialogData.VariableChanges.Count < varChanges)
                {
                    singleDialogData.VariableChanges.Add(new DialogVariablesStatus.VariableData() { name = "Var name", value = 0 });
                }
                while (singleDialogData.VariableChanges.Count > varChanges)
                {
                    singleDialogData.VariableChanges.RemoveAt(singleDialogData.VariableChanges.Count - 1);
                }
            }
            for (int i = 0; i < varChanges; ++i)
            {
                EditorGUILayout.BeginHorizontal();
                singleDialogData.VariableChanges[i].name = EditorGUILayout.TextField(singleDialogData.VariableChanges[i].name);
                singleDialogData.VariableChanges[i].value = EditorGUILayout.IntField(singleDialogData.VariableChanges[i].value);
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}