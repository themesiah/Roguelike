using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GamedevsToolbox.Utils
{
    public class WaitForUIButtonsCoroutine : CustomYieldInstruction, IDisposable
    {
        private struct ButtonCallback
        {
            public Button button;
            public UnityAction listener;
        }
        private List<ButtonCallback> buttonsList = new List<ButtonCallback>();
        private Action<Button> callback = null;

        public override bool keepWaiting { get { return PressedButton == null; } }
        public Button PressedButton { get; private set; } = null;

        public WaitForUIButtonsCoroutine(Action<Button> aCallback, params Button[] aButtons)
        {
            callback = aCallback;
            buttonsList.Capacity = aButtons.Length;
            foreach (var b in aButtons)
            {
                if (b == null)
                    continue;
                var bc = new ButtonCallback { button = b };
                bc.listener = () => OnButtonPressed(bc.button);
                buttonsList.Add(bc);
            }
            Reset();
        }
        public WaitForUIButtonsCoroutine(params Button[] aButtons) : this(null, aButtons) { }

        private void OnButtonPressed(Button button)
        {
            PressedButton = button;
            RemoveListeners();
            callback?.Invoke(button);
        }
        private void InstallListeners()
        {
            foreach (var bc in buttonsList)
                if (bc.button != null)
                    bc.button.onClick.AddListener(bc.listener);
        }
        private void RemoveListeners()
        {
            foreach (var bc in buttonsList)
                if (bc.button != null)
                    bc.button.onClick.RemoveListener(bc.listener);
        }
        public new WaitForUIButtonsCoroutine Reset()
        {
            RemoveListeners();
            PressedButton = null;
            InstallListeners();
            base.Reset();
            return this;
        }
        public WaitForUIButtonsCoroutine ReplaceCallback(Action<Button> aCallback)
        {
            callback = aCallback;
            return this;
        }

        public void Dispose()
        {
            RemoveListeners();
            callback = null;
            buttonsList.Clear();
        }
    }
}
