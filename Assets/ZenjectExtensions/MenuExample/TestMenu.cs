using MenuExample.Installers;
using UnityEngine.UI;
using Zenject.Extensions.MenuSystem;

namespace MenuExample
{
    public class TestMenu : Menu<TestMenu, AppMenuManager>
    {
        public Text messageTxt;
        private TestMenuArg _arg;
        
        protected override void OnShowBefore<T1>(T1 e)
        {
            _arg = e as TestMenuArg;
            if (_arg != null) messageTxt.text = _arg.message;
        }

        protected override void OnShowAfter<T1>(T1 e)
        {
        }

        public void OnClick_Close()
        {
            menuManager.CloseMenu(MenuName.TestMenu.ToString());
            menuManager.OpenMenu(MenuName.Login, new LoginMenuArg {message = "Login Menu", Mode = MenuMode.Single});
            //_loginMenuFactory.Create().Open(new LoginMenuArg{ message = "Login Menu", Mode = MenuMode.Single});
        }
    }

    public class TestMenuArg : MenuArg
    {
        public string message = "Hello World";
    }
}