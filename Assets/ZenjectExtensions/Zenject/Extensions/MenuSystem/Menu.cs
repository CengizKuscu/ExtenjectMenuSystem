using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Zenject.Extensions.MenuSystem
{
    public interface IMenu
    {
    }

    public abstract class Menu : MonoBehaviour, IMenu
    {
        [Tooltip("Destroy the Game Object when menu is closed(reduced memory usage")]
        public bool destroyWhenClosed = false;

        [Tooltip("Defines it as an popup menu.")]
        public bool isPopup = false;

        [FormerlySerializedAs("menuName")] [Tooltip("Menu Name"), SerializeField] private string _menuName;

        public string menuName
        {
            get => _menuName;
            private set => _menuName = value;
        }

        protected abstract void OnShowBefore<T>(T e) where T : IMenuArg;
        protected abstract void OnShowAfter<T>(T e) where T : IMenuArg;

        protected abstract void OnHideBefore();
        protected abstract void OnHideAfter();
        public abstract void Close();
    }

    public class Menu<T, TManager> : Menu where T : Menu<T, TManager> where TManager : MenuManager<TManager>
    {
        public static T Instance { get; private set; }

        private TManager _menuManager;

        private IMenuArg _menuArg;

        public bool isOpenFirst = true;

        [Inject]
        public void Init(TManager menuManager)
        {
            if (string.IsNullOrEmpty(menuName))
                throw new Exception("menuName is not defined from " + gameObject.name);

            _menuManager = menuManager;
        }

        public virtual TManager menuManager => _menuManager;

        protected void Awake()
        {
            Instance = (T) this;
            gameObject.SetActive(false);
        }

        public void Open(IMenuArg e)
        {
            _menuArg = e;
            gameObject.SetActive(false);
            OnShowBefore(_menuArg);
            gameObject.SetActive(true);

            _menuManager.Open(Instance, _menuArg);
        }

        public override void Close()
        {
            OnHideBefore();
            gameObject.SetActive(false);
        }


        protected virtual void OnEnable()
        {
            if (isOpenFirst)
            {
                OnShowAfter(_menuArg);
                isOpenFirst = false;
            }
        }

        protected virtual void OnDisable()
        {
            if (!isOpenFirst)
            {
                isOpenFirst = true;
                OnHideAfter();

                if (destroyWhenClosed)
                    Destroy(gameObject);
            }
        }

        protected override void OnShowBefore<TMenuArg>(TMenuArg e)
        {
        }

        protected override void OnShowAfter<TMenuArg>(TMenuArg e)
        {
        }

        protected override void OnHideBefore()
        {
        }

        protected override void OnHideAfter()
        {
        }


        public class Factory : PlaceholderFactory<T>
        {
            private DiContainer _container;
            private GameObject _prefab;
            private IMenuManager _menuManager;

            public Factory(DiContainer container, GameObject prefab, IMenuManager menuManager)
            {
                _container = container;
                _prefab = prefab;
                //_menuManager = _container.GetDependencyContracts(typeof(MenuManager)) as MenuManager;
                _menuManager = menuManager;
            }

            public override T Create()
            {
                Menu menu = _prefab.GetComponent<Menu<T, TManager>>();
                Transform menuTransform = !menu.isPopup ? _menuManager.MenuContainer : _menuManager.PopupContainer;

                if (Instance == null)
                {
                    Debug.LogFormat("In customMenuFactory, return new prefab : {0}", _prefab.name);
                    return _container.InstantiatePrefab(_prefab, menuTransform).GetComponent<T>();
                }
                else
                {
                    Debug.LogFormat("In CustormMenuFactory, return existing Instance : {0}", Instance.name);
                    return Instance;
                }
            }

            public override void Validate()
            {
                GameObject instance = _container.InstantiatePrefab(_prefab);
                instance.GetComponent<T>();
            }
        }
    }

    public class MenuArg : IMenuArg
    {
        private MenuMode _mode;

        public MenuMode Mode
        {
            get => _mode;
            set => _mode = value;
        }
    }
}