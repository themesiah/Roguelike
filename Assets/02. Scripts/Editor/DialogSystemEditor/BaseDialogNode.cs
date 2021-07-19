using UnityEngine;
using Laresistance.EditorTools;
using System;

namespace Laresistance.Systems.Dialog
{
    public abstract class BaseDialogNode : Node
    {
        protected SingleDialogData singleDialogData;
        protected Action<BaseDialogNode> OnNodeSelected;
        protected Action<BaseDialogNode> OnNodeUnselected;

        public SingleDialogData DialogData => singleDialogData;

        public BaseDialogNode(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle,
            GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint,
            Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode,
            Action<BaseDialogNode> OnNodeSelected, Action<BaseDialogNode> OnNodeUnselected,
            SingleDialogData singleDialogData) :
            base(position, width, height, nodeStyle, selectedStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode)
        {
            this.singleDialogData = singleDialogData;
            this.OnNodeSelected = OnNodeSelected;
            this.OnNodeUnselected = OnNodeUnselected;
        }

        protected abstract void SetTitle();
        public abstract void DrawDataBox();
    }
}