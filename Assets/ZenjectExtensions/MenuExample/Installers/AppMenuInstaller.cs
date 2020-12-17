using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Zenject.Extensions.MenuSystem.Installers;

namespace MenuExample.Installers
{
    public class AppMenuInstaller : MenuInstaller
    {
        [Inject] private AppMenus _appMenus;

        public override void InstallBindings()
        {
            foreach (var o in _appMenus.menuList)
            {
                switch (o.name)
                {
                    case "Login":
                        Container.Bind<GameObject>().FromInstance(o.menuItem).WhenInjectedInto<LoginMenu.Factory>();
                        Container.BindFactory<LoginMenu, LoginMenu.Factory>().FromFactory<LoginMenu.Factory>();
                        break;
                    
                    case "TestMenu":
                        Container.Bind<GameObject>().FromInstance(o.menuItem).WhenInjectedInto<TestMenu.Factory>();
                        Container.BindFactory<TestMenu, TestMenu.Factory>().FromFactory<TestMenu.Factory>();
                        break;
                }
            }
        }

        [Serializable]
        public class AppMenus : MenuPrefabs
        {
            public List<MenuPrefabItem> menuList;
            
        }

        [Serializable]
        public class MenuPrefabItem
        {
            public string name;
            public GameObject menuItem;
        }
    }

    public enum MenuName
    {
        TestMenu = 0,
        Login = 1
    }
}