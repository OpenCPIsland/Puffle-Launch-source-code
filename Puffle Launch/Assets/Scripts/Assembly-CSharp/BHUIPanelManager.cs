using System;
using UnityEngine;

[AddComponentMenu("EZ GUI/Management/Panel Manager")]
public class BHUIPanelManager : UIPanelManager
{
	public UIPanelBase GetPanel(int aPanelIndex)
	{
		for (int i = 0; i < panels.Count; i++)
		{
			if (panels[i].index == aPanelIndex)
			{
				return panels[i];
			}
		}
		Utilities.AssertMsg(false, "Panel (index = " + aPanelIndex + ") not found!");
		return null;
	}

	public UIPanelBase GetPanel(string aPanelName)
	{
		for (int i = 0; i < panels.Count; i++)
		{
			if (string.Equals(panels[i].name, aPanelName, StringComparison.CurrentCultureIgnoreCase))
			{
				return panels[i];
			}
		}
		Utilities.AssertMsg(false, "Panel (name = " + aPanelName + ") not found!");
		return null;
	}
}
