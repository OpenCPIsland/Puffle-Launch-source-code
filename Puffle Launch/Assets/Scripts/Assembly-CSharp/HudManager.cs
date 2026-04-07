using UnityEngine;

public class HudManager
{
	private GameObject m_RefObj;

	private InGameHud m_InGameHud;

	public InGameHud InGameHud
	{
		get
		{
			return m_InGameHud;
		}
	}

	public HudManager(GameObject aRefObj)
	{
		m_RefObj = aRefObj;
	}

	public void Draw()
	{
		DrawInGameHud();
	}

	public void Update()
	{
		m_InGameHud.Update();
	}

	private void DrawInGameHud()
	{
		if (Utilities.Assert(m_InGameHud != null) && m_InGameHud.CanDraw())
		{
			m_InGameHud.Draw();
		}
	}

	public void ShowInGameHud(bool aShow)
	{
		if (aShow)
		{
			if (m_InGameHud == null)
			{
				m_InGameHud = new InGameHud(m_RefObj);
			}
		}
		else
		{
			m_InGameHud.SetVisible(false);
		}
	}

	public void CleanUp()
	{
		m_InGameHud = null;
	}
}
