using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Zenject.Extensions.MenuSystem
{
    public interface IMenuManager
    {
        Transform MenuContainer { get; }

        Transform PopupContainer { get; }

        void Init();

        void Open(Menu instance, IMenuArg menuArg);

        void OpenMenu(string menuName, IMenuArg arg);

        void Close(Menu instance);

        void CloseMenu(string menuName);
    }

    public abstract class AbsMenuManager : MonoBehaviour, IMenuManager
    {
        public Transform menuContainer;
        public Transform popupContainer;
        public GameObject menuLoadingAnim;

        protected LinkedList<Menu> menuLinkedList = new LinkedList<Menu>();

        public Transform MenuContainer => menuContainer;
        public Transform PopupContainer => popupContainer;

        public virtual void Init()
        {
            menuLoadingAnim.SetActive(false);

            foreach (Transform val in menuContainer.transform)
            {
                val.gameObject.SetActive(false);
            }

            foreach (Transform val in popupContainer.transform)
            {
                val.gameObject.SetActive(false);
            }
        }

        public void Open(Menu instance, IMenuArg menuArg)
        {
            menuLoadingAnim.SetActive(true);
            Debug.LogWarning("MenuManager: " + menuArg.Mode);

            if (menuArg.Mode == MenuMode.Single)
                CloseOthers(instance);

            if (menuLinkedList.Find(instance) != null)
            {
                menuLoadingAnim.SetActive(false);
                return;
            }

            RectTransform rectTransform = instance.GetComponent<RectTransform>();

            if (menuLinkedList.Count == 0)
            {
                menuLinkedList.AddLast(instance);
            }
            else
            {
                menuLinkedList.AddFirst(instance);
            }

            if (rectTransform != null)
                rectTransform.SetAsLastSibling();

            menuLoadingAnim.SetActive(false);
        }

        public virtual void OpenMenu(string menuName, IMenuArg arg) 
        {
            throw new NotImplementedException();
        }

        private void CloseOthers(Menu instance)
        {
            if (menuLinkedList.Count == 0)
                return;

            foreach (var menu in menuLinkedList.ToList())
            {
                if (menu != instance)
                {
                    Close(menu);
                }
            }
        }

        public void Close(Menu instance)
        {
            if (menuLinkedList.Count == 0)
            {
                Debug.LogErrorFormat(instance, "{0} cannot be closed because menu linked list is empty",
                    instance.GetType());
                return;
            }

            LinkedListNode<Menu> node = menuLinkedList.Find(instance);
            if (node == null)
            {
                Debug.LogErrorFormat(instance, "{0} cannot be closed because it is not in the linked list",
                    instance.GetType());
            }
            else
            {
                //node = node.Previous;
                instance.Close();
                if (instance.destroyWhenClosed)
                    menuLinkedList.Remove(node);
            }
        }

        public virtual void CloseMenu(string menuName)
        {
            Menu instance = menuLinkedList.FirstOrDefault(s => s.menuName == menuName);
            
            instance?.Close();
        }
    }

    public class MenuManager<T> : AbsMenuManager where T : MenuManager<T>
    {
        
    }
}