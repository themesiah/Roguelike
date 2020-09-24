using GamedevsToolbox.StateMachine;
using System;
using System.Threading.Tasks;

namespace GamedevsToolbox.UISolution
{
    public class AsyncUIState : IAsyncState
    {
        private string nextMenu = null;
        private UIMenuData menuData;

        public AsyncUIState(UIMenuData data)
        {
            menuData = data;
        }

        async public Task EnterState()
        {
            menuData.menuObject?.SetActive(true);
            menuData.anim.SetTrigger(menuData.inTrigger);
            while(menuData.anim.IsInTransition(0))
            {
                await Task.Yield();
            }
            await Task.Delay(TimeSpan.FromSeconds(menuData.anim.GetCurrentAnimatorStateInfo(0).length));
        }

        async public Task ExitState()
        {
            nextMenu = null;
            menuData.anim.SetTrigger(menuData.outTrigger);
            while (menuData.anim.IsInTransition(0))
            {
                await Task.Yield();
            }
            await Task.Delay(TimeSpan.FromSeconds(menuData.anim.GetCurrentAnimatorStateInfo(0).length));
            menuData.menuObject.SetActive(false);
        }

        public virtual void ReceiveSignal(string signal)
        {
            nextMenu = signal;
        }

#pragma warning disable CS1998
        async public Task<string> Update()
#pragma warning restore CS1998
        {
            if (nextMenu != null)
            {
                return nextMenu;
            }
            else
            {
                return null;
            }
        }
    }
}