using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;
using Unity.VisualScripting;
using Amanotes.Utils;

namespace GFramework.GUI
{
    public abstract class GUIManagerBase : SingletonMono<GUIManagerBase>
    {
        public static Action<string> OnGUICallShow;
        public System.Action OnHide;

        public Camera guiCamera = null;

        public EventSystem eventSystem;

        public List<GUIHandlerBase> listHandler = new List<GUIHandlerBase>();

        //private int currentGUIIndex;
        //public int CurrentGUIIndex => currentGUIIndex;
        //private int lastGUIIndex;

        private string currentGUIName;
        public string CurrentGUIName => currentGUIName;
        private string lastGUIName;


        [HideInInspector]
        public string prefabPath = "Prefabs/GUI";

        [HideInInspector]
        public string sourcePath = "Scripts/GUI";

        [HideInInspector]
        public string newGuiName = "";

        [HideInInspector]
        public string destPath = "Scripts";

        [HideInInspector]
        public string destName = "GUIName.cs";

#if UNITY_EDITOR
        //Create GUI with template
        [HideInInspector]
        public bool createWithTemplate = false;

        [HideInInspector]
        public GUIBase template = null;
#endif

        public T GetHandler<T>(string guiName) where T : GUIHandlerBase
        {
            foreach (var handler in listHandler)
            {
                if (handler.guiName == guiName && handler is T)
                {
                    return (T)handler;
                }
            }
            return null;
        }

        public T GetGUI<T>(string guiName) where T : GUIBase
        {
            foreach (var handler in listHandler)
            {
                if (handler.guiName == guiName)
                {
                    return handler.GetGUI<T>();
                }
            }
            return null;
        }

        //public virtual void ShowGUI(int index, params object[] @parameter)
        //{
        //    if (listHandler.Count <= index || index < 0)
        //        return;
        //    if (listHandler[index] == null)
        //        return;

        //    if (listHandler[index].guiPrefabObj == null)
        //    {
        //        ShowGUIAsync<GUIBase>(index, null, parameter);
        //        return;
        //    }

        //    ShowAndUpdateGuiIndex(index, listHandler[index], parameter);
        //}
        public virtual void ShowGUI(string guiName, params object[] @parameter)
        {
            GUIHandlerBase handle = new GUIHandlerBase();
            foreach (var item in listHandler)
            {
                if (item.guiName == guiName)
                {
                    handle = item;
                }
            }

            if (handle.guiPrefabObj == null)
            {
                ShowGUIAsync<GUIBase>(handle.guiName, null, parameter);
                return;
            }

            ShowAndUpdateGuiIndex(handle, parameter);
        }

        public virtual GUIBase ShowAGetGUI(string guiName, params object[] @parameter)
        {
            GUIHandlerBase handle = new GUIHandlerBase();
            foreach (var item in listHandler)
            {
                if (item.guiName == guiName)
                {
                    handle = item;
                }
            }
            var isShow = ShowAndUpdateGuiIndex(handle, parameter);

            if (isShow) return handle.gui;
            else return null;
        }

        public void HideGUI(string guiName, params object[] @parameter)
        {
            for (int i = 0; i < listHandler.Count; i++)
            {
                if (listHandler[i].guiName == guiName)
                {
                    listHandler[i].Hide(@parameter);
                }
            }
            if (guiName == currentGUIName)
            {
                currentGUIName = lastGUIName;
            }

            //if (listHandler.Count <= index || index < 0)
            //    return;
            //if (listHandler[index] == null)
            //    return;
            //listHandler[index].Hide(@parameter);
            //if (index == currentGUIIndex)
            //{
            //    currentGUIIndex = lastGUIIndex;
            //}
            OnHide?.Invoke();
        }

        public void HideGUIWithoutAnimation(string guiName, params object[] @parameter)
        {
            for (int i = 0; i < listHandler.Count; i++)
            {
                if (listHandler[i].guiName == guiName)
                {
                    listHandler[i].HideWithoutAnimation(@parameter);
                }
            }
            if (guiName == currentGUIName)
            {
                currentGUIName = lastGUIName;
            }

            OnHide?.Invoke();
        }

        public void HideAllGUI(List<string> ignores, List<string> forceDestroy)
        {
            for (int i = 0; i < listHandler.Count; i++)
            {
                if (listHandler[i] == null) continue;
                if (forceDestroy != null && forceDestroy.Contains(listHandler[i].guiName))
                {
                    listHandler[i].ForceDestroy();
                }
                else if (ignores == null || !ignores.Contains(listHandler[i].guiName))
                {
                    listHandler[i].Hide();
                }
            }
        }

        public void DestroyAllGUI(List<string> ignores)
        {
            for (int i = 0; i < listHandler.Count; i++)
            {
                if (listHandler[i] == null) continue;
                if (ignores != null && ignores.Contains(listHandler[i].guiName)) continue;

                listHandler[i].ForceDestroy();
            }
        }

        public void EnableCanvasGUI(List<string> ignores, bool value)
        {
            for (int i = 0; i < listHandler.Count; i++)
            {
                if (listHandler[i] == null) continue;

                if (ignores == null || !ignores.Contains(listHandler[i].guiName))
                {
                    var canvas = listHandler[i].guiCanvas;
                    if (canvas != null)
                    {
                        canvas.enabled = value;
                        Debug.Log("[EnableCanvasGUI] |" + canvas.name + "|" + value);
                    }
                }
            }
        }

        public bool IsShowed(string guiName, bool includeAnimation = false)
        {
            for (int i = 0; i < listHandler.Count; i++)
            {
                if (listHandler[i].guiName == guiName)
                {
                    if (includeAnimation)
                    {
                        return listHandler[i].IsShowed() || listHandler[i].IsShowing() || listHandler[i].IsHiding();
                    }

                    return listHandler[i].IsShowed();
                }
            }
            return false;
        }

        public bool IsGUI(string guiName, GUIBase gUIBase)
        {
            for (int i = 0; i < listHandler.Count; i++)
            {
                if (listHandler[i].guiName == guiName)
                {
                    return listHandler[i] == gUIBase.handler;
                }
            }
            return false;

            //if (listHandler.Count <= index || index < 0)
            //    return false;
            //if (listHandler[index] == null)
            //    return false;
            //return listHandler[index] == gUIBase.handler;
        }

        public bool IsGUIOnTop(string guiName, params string[] ignoreGui) //note: need check show hide.
        {
            if (string.IsNullOrEmpty(guiName))
            {
                return false;
            }

            if (IsShowed(guiName) == false) return false;

            int layerThis = 0;
            int highedLayer = 0;

            for (int i = 0; i < listHandler.Count; i++)
            {
                if (listHandler[i] == null) continue;
                if (listHandler[i].guiName == guiName)
                {
                    layerThis = listHandler[i].guiCanvas.sortingOrder;
                }
            }

            for (int i = 0; i < listHandler.Count; i++)
            {
                if (listHandler[i] == null) continue;
                if (IsShowed(listHandler[i].guiName) == false)
                {
                    continue;
                }

                if (System.Array.IndexOf(ignoreGui, listHandler[i].guiName) > -1)
                {
                    continue;
                }

                if (listHandler[i].guiCanvas == null)
                {
                    continue;
                }

                if (listHandler[i].processBackKey == false)
                {
                    continue;
                }

                int layer = listHandler[i].guiCanvas.sortingOrder;
                if (layer > highedLayer)
                {
                    highedLayer = layer;
                }
            }

            if (layerThis == highedLayer)
            {
                return true;
            }

            return false;
        }

        public virtual void ShowGUIAsync<T>(string guiName, Action<T> onShowed, params object[] @parameter) where T : GUIBase
        {
            GUIHandlerBase handle = new GUIHandlerBase();
            for (int i = 0; i < listHandler.Count; i++)
            {
                if (listHandler[i].guiName == guiName)
                {
                    handle = listHandler[i];
                }
            }

            if (handle == null) return;

            if (handle.guiPrefabObj != null)
            {
                ShowAndUpdateGuiIndex(handle, parameter);
                onShowed?.Invoke(GetGUI<T>(handle.guiName));
                return;
            }
            else
            {
                LoadGuiAsync(handle, (go) =>
                {
                    if (go != null)
                    {
                        handle.guiPrefabObj = go;
                        ShowAndUpdateGuiIndex(handle, parameter);
                        onShowed?.Invoke(GetGUI<T>(handle.guiName));
                    }
                    else
                    {
                        onShowed?.Invoke(null);
                    }
                });
            }
        }

        private bool ShowAndUpdateGuiIndex(GUIHandlerBase handle, params object[] @parameter)
        {
            var res = handle.Show(@parameter);
            lastGUIName = currentGUIName;
            currentGUIName = handle.guiName;
            OnGUICallShow?.Invoke(currentGUIName);
            return res;
        }

        private void LoadGuiAsync(GUIHandlerBase uiHandlerBase, Action<GameObject> onLoaded)
        {
            // if u dont want to get on addressable uncomment this
            //return uiHandlerBase.guiPrefabObj;

            // use for Addressable load
            //BBT.AddressableHelper.instance.LoadAsset<GameObject>("Popup/" + uiHandlerBase.guiName, onLoaded);

        }
    }
}