using MenuExample.Installers;
using Zenject;
using Zenject.Extensions.MenuSystem;

namespace MenuExample
{
    public class AppMenuManager : MenuManager<AppMenuManager>
    {
        [Inject] private TestMenu.Factory _testMenuFactory;
        [Inject] private LoginMenu.Factory _loginMenuMenuFactory;

        public void OpenMenu(MenuName menuName, IMenuArg arg) 
        {
            OpenMenu(menuName.ToString(), arg);
        }

        public override void OpenMenu(string menuName, IMenuArg arg)
        {
            switch (menuName)
            {
                case "TestMenu":
                    _testMenuFactory.Create().Open(arg);
                    break;
                case "Login":
                    _loginMenuMenuFactory.Create().Open(arg);
                    break;
            }
        }
    }
}