using MenuExample.Installers;
using UnityEngine;
using UnityEngine.UI;
using Zenject.Extensions.MenuSystem;

namespace MenuExample
{
    public class LoginMenu : Menu<LoginMenu, AppMenuManager>
    {
        [SerializeField] private Text message;

        private LoginMenuArg _arg;

        protected override void OnShowBefore<TMenuArg>(TMenuArg e)
        {
            _arg = e as LoginMenuArg;
            message.text = _arg.message;
        }
        
        public void OnClick_Close()
        {
            Close();
            menuManager.OpenMenu(MenuName.TestMenu, new TestMenuArg
            {
                message = "Test Menu",
                Mode = MenuMode.Single
            });
        }
    }

    public class LoginMenuArg : MenuArg
    {
        public string message;
        
    }
}