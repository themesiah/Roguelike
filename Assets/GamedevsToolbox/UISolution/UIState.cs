using UnityEngine;
using GamedevsToolbox.StateMachine;
using System;
using System.Collections;

namespace GamedevsToolbox.UISolution
{
    public class UIState : ICoroutineState
    {
        private string nextMenu = null;
        private UIMenuData menuData;

        public UIState(UIMenuData data)
        {
            menuData = data;
        }

        public IEnumerator EnterState()
        {
            menuData.menuObject?.SetActive(true);
            menuData.anim?.SetTrigger(menuData.inTrigger);
            while(menuData.anim != null && menuData.anim.IsInTransition(0))
            {
                yield return null;
            }            
            yield return new WaitForSeconds(menuData.anim.GetCurrentAnimatorStateInfo(0).length);
        }

        public IEnumerator ExitState()
        {
            menuData.anim?.SetTrigger(menuData.outTrigger);
            while (menuData.anim != null && menuData.anim.IsInTransition(0))
            {
                yield return null;
            }
            yield return new WaitForSeconds(menuData.anim.GetCurrentAnimatorStateInfo(0).length);
            menuData.menuObject.SetActive(false);
            nextMenu = null;
        }

        public virtual void ReceiveSignal(string signal)
        {
            nextMenu = signal;
        }

        public IEnumerator Update(Action<string> resolve)
        {
            if (nextMenu != null)
            {
                resolve(nextMenu);
            }
            yield return null;
        }

        public void Resume()
        {
            if (menuData.anim != null)
                menuData.anim.speed = 1f;
        }

        public void Pause()
        {
            if (menuData.anim != null)
                menuData.anim.speed = 0f;
        }
    }
}