using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class GSystem : MonoBehaviour
{
    private void Start()
    {
        // GUIManager.instance.ShowGUI(GUIName.GUI_UIHome);
        //StartCoroutine(IShow());
    }
    [ContextMenu("Show")]
    void Show()
    {
       //GUIManager.instance.ShowGUI(GUIName.GUI_UIHome);
    }
    IEnumerator IShow()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(3);
        GUIManager.instance.ShowGUI(GUIName.GUI_UIHome);

    }
}
