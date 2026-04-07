using System.Collections.Generic;

public class UIControlHandler<T>
{
	public List<T> m_ControlList;

	public List<string> m_ControlName;

	public UIControlHandler(List<T> aControlList, List<string> aControlName)
	{
		m_ControlList = aControlList;
		m_ControlName = aControlName;
	}

	public virtual void EnableAll(bool aEnable)
	{
		for (int i = 0; i < m_ControlList.Count; i++)
		{
			Enable(i, aEnable);
		}
	}

	public virtual void Enable(string aControlName, bool aEnable)
	{
		int indexByName = GetIndexByName(aControlName);
		if (indexByName >= 0)
		{
			Enable(indexByName, aEnable);
		}
	}

	public virtual void Enable(int aControlIndex, bool aEnable)
	{
		IUIControlExtension iUIControlExtension = m_ControlList[aControlIndex] as IUIControlExtension;
		Utilities.AssertMsg(iUIControlExtension != null, "Cannot use UIController to handle a control that is not using IUIControlExtension: " + m_ControlList[aControlIndex]);
		if (iUIControlExtension != null)
		{
			iUIControlExtension.Enable(aEnable);
		}
	}

	public virtual void ShowAll(bool aShow)
	{
		for (int i = 0; i < m_ControlList.Count; i++)
		{
			Show(i, aShow);
		}
	}

	public virtual void Show(string aControlName, bool aShow)
	{
		int indexByName = GetIndexByName(aControlName);
		if (indexByName >= 0)
		{
			Show(indexByName, aShow);
		}
	}

	public virtual void Show(int aControlIndex, bool aShow)
	{
		IUIControlExtension iUIControlExtension = m_ControlList[aControlIndex] as IUIControlExtension;
		Utilities.AssertMsg(iUIControlExtension != null, "Cannot use generic controller to handle a control that is not using IUIControlExtension: " + m_ControlList[aControlIndex]);
		if (iUIControlExtension != null)
		{
			iUIControlExtension.Show(aShow);
		}
	}

	protected virtual int GetIndexByName(string aControlName)
	{
		int num = m_ControlName.FindIndex((string name) => name == aControlName);
		Utilities.AssertMsg(num >= 0, "Control name '" + aControlName + "' not found!");
		return num;
	}

	public virtual T GetControl(string aControlName)
	{
		int indexByName = GetIndexByName(aControlName);
		if (indexByName >= 0)
		{
			return m_ControlList[indexByName];
		}
		return default(T);
	}
}
