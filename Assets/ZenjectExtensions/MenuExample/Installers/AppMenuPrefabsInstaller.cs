using UnityEngine;
using Zenject.Extensions.MenuSystem.Installers;

namespace MenuExample.Installers
{
    [CreateAssetMenu(fileName = "MenuPrefabs", menuName = "Installers/MenuPrefabs")]
    public class AppMenuPrefabsInstaller  : MenuPrefabsInstaller<AppMenuPrefabsInstaller>
    {
        public AppMenuInstaller.AppMenus appMenus;

        public override void InstallBindings()
        {
            Container.BindInstance(appMenus);
        }
    }
}