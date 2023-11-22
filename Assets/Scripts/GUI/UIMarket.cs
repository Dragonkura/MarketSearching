using UnityEngine;
using System.Collections;
using GFramework.GUI;
using EnhancedUI.EnhancedScroller;
using System.Collections.Generic;
using Element;
using UnityEngine.UIElements;

public class UIMarket : GUIBase, IEnhancedScrollerDelegate
{
	[SerializeField] private EnhancedScroller scroller;
    [SerializeField] private PlayerItem playerItemPrefabs;

    private List<Player> players = new List<Player>() {
            new Player("asd", "sada", "sadas", 7.6f, "asdas"),
            new Player("asd", "sada", "sadas", 7.6f, "asdas"),
            new Player("asd", "sada", "sadas", 7.6f, "asdas"),
            new Player("asd", "sada", "sadas", 7.6f, "asdas"),
            new Player("asd", "sada", "sadas", 7.6f, "asdas"),
            new Player("asd", "sada", "sadas", 7.6f, "asdas"),
            new Player("asd", "sada", "sadas", 7.6f, "asdas"),
            new Player("asd", "sada", "sadas", 7.6f, "asdas"),
        };
    private void Start()
    {
    }
    public override bool Show(params object[] @parameter)
	{
        scroller.Delegate = this;
        scroller.ReloadData();
        return base.Show(@parameter);
	}

	public override void Hide(params object[] @parameter)
	{
		base.Hide(@parameter);
	}

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return players.Count;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return playerItemPrefabs.GetComponent<RectTransform>().sizeDelta.y;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        PlayerItem item = scroller.GetCellView(playerItemPrefabs) as PlayerItem;
        item.gameObject.SetActive(true);
        return item;

    }
}
