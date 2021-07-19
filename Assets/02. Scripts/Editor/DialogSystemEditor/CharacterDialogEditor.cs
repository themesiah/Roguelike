using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Laresistance.EditorTools;
using GamedevsToolbox.ScriptableArchitecture.LocalizationV2;
using System.Linq;

namespace Laresistance.Systems.Dialog
{
    public class CharacterDialogEditor : EditorWindow
    {
        private static int NODE_WIDTH = 250;
        private static int NODE_HEIGHT = 80;
        
        private List<BaseDialogNode> nodes;
        private List<Connection> connections;

        private GUIStyle nodeStyle;
        private GUIStyle selectedNodeStyle;
        private GUIStyle inPointStyle;
        private GUIStyle outPointStyle;

        private ConnectionPoint selectedInPoint;
        private ConnectionPoint selectedOutPoint;

        private Vector2 offset;
        private Vector2 drag;

        private BaseDialogNode selectedNode = null;
        public TextAsset localizationTextAsset;
        private CharacterDialog characterDialog;
        private CharacterDialog lastCharacterDialog;

        [MenuItem("Dialog Editor", menuItem = "Laresistance/Dialog Editor")]
        private static void OpenWindow()
        {
            CharacterDialogEditor window = GetWindow<CharacterDialogEditor>();
            window.titleContent = new GUIContent("Node Based Editor");

            var assets = AssetDatabase.FindAssets("TextDB");
            if (assets.Length > 0)
            {
                window.localizationTextAsset = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(assets[0]), typeof(TextAsset)) as TextAsset;
                Texts.Init(window.localizationTextAsset);
            }
        }

        private void OnEnable()
        {
            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
            nodeStyle.border = new RectOffset(12, 12, 12, 12);
            nodeStyle.alignment = TextAnchor.MiddleCenter;
            nodeStyle.wordWrap = true;
            nodeStyle.padding = new RectOffset(10, 10, 5, 5);

            selectedNodeStyle = new GUIStyle();
            selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
            selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);
            selectedNodeStyle.alignment = TextAnchor.MiddleCenter;
            selectedNodeStyle.wordWrap = true;
            selectedNodeStyle.padding = new RectOffset(10, 10, 5, 5);

            inPointStyle = new GUIStyle();
            inPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
            inPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
            inPointStyle.border = new RectOffset(4, 4, 12, 12);
            inPointStyle.alignment = TextAnchor.MiddleCenter;
            inPointStyle.wordWrap = true;
            inPointStyle.padding = new RectOffset(10, 10, 5, 5);

            outPointStyle = new GUIStyle();
            outPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
            outPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
            outPointStyle.border = new RectOffset(4, 4, 12, 12);
            outPointStyle.alignment = TextAnchor.MiddleCenter;
            outPointStyle.wordWrap = true;
            outPointStyle.padding = new RectOffset(10, 10, 5, 5);
        }

        [System.Obsolete]
        private void OnGUI()
        {
            characterDialog = EditorGUILayout.ObjectField(characterDialog, typeof(CharacterDialog)) as CharacterDialog;

            if (characterDialog != null)
            {
                if (characterDialog != lastCharacterDialog)
                {
                    lastCharacterDialog = characterDialog;
                    nodes?.Clear();
                    connections?.Clear();
                }

                DrawGrid(20, 0.2f, Color.gray);
                DrawGrid(100, 0.4f, Color.gray);

                DrawNodes();
                DrawConnections();

                DrawConnectionLine(Event.current);
                DrawSelectedNode();

                ProcessNodeEvents(Event.current);
                ProcessEvents(Event.current);                
            }
            if (GUI.changed) Repaint();
        }

        private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
        {
            int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
            int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

            Handles.BeginGUI();
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

            offset += drag * 0.5f;
            Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

            for (int i = 0; i < widthDivs; i++)
            {
                Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
            }

            for (int j = 0; j < heightDivs; j++)
            {
                Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
            }

            Handles.color = Color.white;
            Handles.EndGUI();
        }

        private void DrawNodes()
        {
            if (nodes == null || nodes.Count == 0)
            {
                nodes = new List<BaseDialogNode>();
                for (int i = 0; i < characterDialog.ListOfDialogs.Count; ++i)
                {
                    if (characterDialog.ListOfDialogs[i].normalDialog)
                    {
                        nodes.Add(new DialogNode(characterDialog.ListOfDialogs[i].gridPosition, NODE_WIDTH, NODE_HEIGHT, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode, OnNodeSelected, OnNodeUnselected, characterDialog.ListOfDialogs[i]));
                    } else
                    {
                        nodes.Add(new QuestionNode(characterDialog.ListOfDialogs[i].gridPosition, NODE_WIDTH, NODE_HEIGHT, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode, OnNodeSelected, OnNodeUnselected, characterDialog.ListOfDialogs[i]));
                    }
                }
            }

            if (nodes != null)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    nodes[i].Draw();
                }
            }
        }

        private void DrawConnections()
        {
            if (connections != null)
            {
                for (int i = 0; i < connections.Count; i++)
                {
                    connections[i].Draw();
                }
            }
        }

        private void ProcessEvents(Event e)
        {
            drag = Vector2.zero;

            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        ClearConnectionSelection();
                    }

                    if (e.button == 1)
                    {
                        ProcessContextMenu(e.mousePosition);
                    }
                    break;

                case EventType.MouseDrag:
                    if (e.button == 0 || e.button == 2)
                    {
                        OnDrag(e.delta);
                    }
                    break;
            }
        }

        private void ProcessNodeEvents(Event e)
        {
            if (nodes != null)
            {
                for (int i = nodes.Count - 1; i >= 0; i--)
                {
                    bool guiChanged = nodes[i].ProcessEvents(e);

                    if (guiChanged)
                    {
                        GUI.changed = true;
                    }
                }
            }
        }

        private void DrawConnectionLine(Event e)
        {
            if (selectedInPoint != null && selectedOutPoint == null)
            {
                Handles.DrawBezier(
                    selectedInPoint.rect.center,
                    e.mousePosition,
                    selectedInPoint.rect.center + Vector2.left * 50f,
                    e.mousePosition - Vector2.left * 50f,
                    Color.white,
                    null,
                    2f
                );

                GUI.changed = true;
            }

            if (selectedOutPoint != null && selectedInPoint == null)
            {
                Handles.DrawBezier(
                    selectedOutPoint.rect.center,
                    e.mousePosition,
                    selectedOutPoint.rect.center - Vector2.left * 50f,
                    e.mousePosition + Vector2.left * 50f,
                    Color.white,
                    null,
                    2f
                );

                GUI.changed = true;
            }
        }

        private void DrawSelectedNode()
        {
            if (selectedNode != null)
            {
                Rect rect = EditorGUILayout.BeginVertical();
                selectedNode.DrawDataBox();
                EditorGUILayout.EndVertical();
                GUI.Box(rect, "");
            }
        }

        private void ProcessContextMenu(Vector2 mousePosition)
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Add normal dialog node"), false, () => OnClickAddNormalDialog(mousePosition));
            genericMenu.AddItem(new GUIContent("Add question dialog node"), false, () => OnClickAddQuestionDialog(mousePosition));
            genericMenu.ShowAsContext();
        }

        private void OnDrag(Vector2 delta)
        {
            drag = delta;

            if (nodes != null)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    nodes[i].Drag(delta);
                }
            }

            GUI.changed = true;
        }

        private void OnClickAddNormalDialog(Vector2 mousePosition)
        {
            SingleDialogData sdd = new SingleDialogData();
            sdd.normalDialog = true;
            sdd.dialog = new NormalDialog();
            sdd.VariableChanges = new List<DialogVariablesStatus.VariableData>();
            sdd.RequiredVariables = new List<DialogVariablesStatus.VariableData>();
            sdd.dialog.dialogTextRef = new List<string>();
            sdd.gridPosition = mousePosition;
            characterDialog.ListOfDialogs.Add(sdd);
            nodes.Add(new DialogNode(mousePosition, NODE_WIDTH, NODE_HEIGHT, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode, OnNodeSelected, OnNodeUnselected, sdd));
        }

        private void OnClickAddQuestionDialog(Vector2 mousePosition)
        {
            SingleDialogData sdd = new SingleDialogData();
            sdd.normalDialog = false;
            sdd.questionDialog = new QuestionDialog();
            sdd.questionDialog.variablesChange = new List<DialogVariablesStatus.VariableData>();
            sdd.RequiredVariables = new List<DialogVariablesStatus.VariableData>();
            sdd.gridPosition = mousePosition;
            sdd.questionDialog.answer1TextRef = "";
            sdd.questionDialog.answer2TextRef = "";
            sdd.questionDialog.dialogQuestionRef = "";
            characterDialog.ListOfDialogs.Add(sdd);
            nodes.Add(new QuestionNode(mousePosition, NODE_WIDTH, NODE_HEIGHT, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode, OnNodeSelected, OnNodeUnselected, sdd));
        }

        private void OnNodeSelected(BaseDialogNode node)
        {
            selectedNode = node;
            GUI.FocusControl("");
            GUI.changed = true;
            UpdateConnections();
        }

        private void OnNodeUnselected(BaseDialogNode node)
        {
            if (selectedNode == node)
            {
                selectedNode = null;
            }
            UpdateConnections();
        }

        private void UpdateConnections()
        {
            if (connections == null)
            {
                connections = new List<Connection>();
            }
            connections.Clear();
            foreach(var singleDialogNode in nodes)
            {
                var singleDialog = singleDialogNode.DialogData;
                foreach (var singleDialogNode2 in nodes)
                {
                    var singleDialog2 = singleDialogNode2.DialogData;
                    List<DialogVariablesStatus.VariableData> variableChanges;
                    if (singleDialog.normalDialog) {
                        variableChanges = singleDialog.VariableChanges;
                    } else
                    {
                        variableChanges = singleDialog.questionDialog.variablesChange;
                    }
                    foreach(var variableChange in variableChanges)
                    {
                        foreach(var variableRequirement in singleDialog2.RequiredVariables)
                        {
                            if (variableChange.name == variableRequirement.name && variableChange.value == variableRequirement.value)
                            {
                                connections.Add(new Connection(singleDialogNode2.inPoint, singleDialogNode.outPoint, null));
                            }
                        }
                    }
                }
            }
        }

        private void OnClickInPoint(ConnectionPoint inPoint)
        {
            selectedInPoint = inPoint;

            if (selectedOutPoint != null)
            {
                if (selectedOutPoint.node != selectedInPoint.node)
                {
                    CreateConnection();
                    ClearConnectionSelection();
                }
                else
                {
                    ClearConnectionSelection();
                }
            }
        }

        private void OnClickOutPoint(ConnectionPoint outPoint)
        {
            selectedOutPoint = outPoint;

            if (selectedInPoint != null)
            {
                if (selectedOutPoint.node != selectedInPoint.node)
                {
                    CreateConnection();
                    ClearConnectionSelection();
                }
                else
                {
                    ClearConnectionSelection();
                }
            }
        }

        private void OnClickRemoveNode(Node node)
        {
            if (connections != null)
            {
                List<Connection> connectionsToRemove = new List<Connection>();

                for (int i = 0; i < connections.Count; i++)
                {
                    if (connections[i].inPoint == node.inPoint || connections[i].outPoint == node.outPoint)
                    {
                        connectionsToRemove.Add(connections[i]);
                    }
                }

                for (int i = 0; i < connectionsToRemove.Count; i++)
                {
                    connections.Remove(connectionsToRemove[i]);
                }

                connectionsToRemove = null;
            }

            characterDialog.ListOfDialogs.Remove(((BaseDialogNode)node).DialogData);
            nodes.Remove((BaseDialogNode)node);
            OnNodeUnselected((BaseDialogNode)node);
        }

        private void OnClickRemoveConnection(Connection connection)
        {
            connections.Remove(connection);
        }

        private void CreateConnection()
        {
            if (connections == null)
            {
                connections = new List<Connection>();
            }

            connections.Add(new Connection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection));
        }

        private void ClearConnectionSelection()
        {
            selectedInPoint = null;
            selectedOutPoint = null;
        }
    }
}