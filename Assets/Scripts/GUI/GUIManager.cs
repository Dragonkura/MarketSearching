using UnityEngine;
using GFramework.GUI;

public class GUIManager : GUIManagerBase
{
    public new static GUIManager instance
    {
        get
        {
            return (GUIManager)GUIManagerBase.instance;
        }
    }

    public static bool IsAtHomeScreen
    {
        get
        {
            if (instance == null) return false;

            var tabShown = instance.IsShowed(GUIName.GUI_UIHome);
            return tabShown;
        }
    }

    public string GetShownGUINames()
    {
        var str = string.Empty;

        for (int i = 0; i < listHandler.Count; i++)
        {
            var handler = listHandler[i];
            if (handler == null) continue;

            if (handler.IsShowed()) str += handler.guiName + ", ";
        }

        return str;
    }

    //// Use this for initialization
    void Start()
    {

    }

    private void OnAddressableLoaded(string key, object o)
    {
        if (key.StartsWith("Popup/") && o is GameObject go)
        {
            var handle = listHandler.Find(x => x.guiName.Equals(go.name));
            if (handle != null && handle.guiPrefabObj == null)
            {
                handle.guiPrefabObj = go;
                Debug.Log("GUI AA:: " + handle.name + " Apply gui : " + go.name);
            }
        }
    }

    public bool loadAll = false;
    public bool unloadAll = false;
    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR
        if (loadAll)
        {
            loadAll = false;
            GUIHandlerBase[] collection = GetComponentsInChildren<GUIHandlerBase>();
            foreach (var item in collection)
            {
                item.Load();
            }

        }
        if (unloadAll)
        {
            unloadAll = false;
            GUIHandlerBase[] collection = GetComponentsInChildren<GUIHandlerBase>();
            foreach (var item in collection)
            {
                item.UnLoad();
            }

        }
#endif
    }

    public void SetUICameraClearFlags(CameraClearFlags clearFlags)
    {
        this.guiCamera.clearFlags = clearFlags;
    }
}
