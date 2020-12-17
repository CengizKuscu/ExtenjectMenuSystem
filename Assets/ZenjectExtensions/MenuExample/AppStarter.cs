using MenuExample.Installers;
using UnityEngine;
using Zenject;
using Zenject.Extensions.MenuSystem;

namespace MenuExample
{
    public class AppStarter : MonoBehaviour
    {
        [Inject] private AppMenuManager _menuManager;

        [Inject]
        public void Init(TestMenu.Factory testmenuFactory)
        {
            _menuManager.Init();
        
            TestMenuArg arg = new TestMenuArg();
            arg.message = "Hello world";
            arg.Mode = MenuMode.Additive;

            _menuManager.OpenMenu(MenuName.TestMenu, arg);
        }
    }
}