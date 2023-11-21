using UnityEngine;
using System.Collections;
using GFramework.GUI;

public class UIHome : GUIBase
{
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public override bool Show(params object[] @parameter)
	{
		return base.Show(@parameter);
	}

	public override void Hide(params object[] @parameter)
	{
		base.Hide(@parameter);
	}
}
