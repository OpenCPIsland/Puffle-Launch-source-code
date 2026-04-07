using UnityEngine;

public class LevelSelectManager : MonoBehaviour
{
	private BHUIButton[] m_ButtonList;

	private bool m_ChangeState;

	private bool m_NextState = true;

	private int FrameCount;

	private int FrameDelay = 2;

	private void Start()
	{
		m_ButtonList = Object.FindObjectsOfType(typeof(BHUIButton)) as BHUIButton[];
	}

	private void Update()
	{
		if (m_ChangeState)
		{
			FrameCount++;
			if (FrameCount >= FrameDelay)
			{
				FrameCount = 0;
				m_ChangeState = false;
				SetButtonsEnable(m_NextState);
			}
		}
	}

	public void RequestChangeButtonsState(bool aEnable)
	{
		m_ChangeState = true;
		m_NextState = aEnable;
	}

	private void SetButtonsEnable(bool aEnable)
	{
		BHUIButton[] buttonList = m_ButtonList;
		foreach (BHUIButton bHUIButton in buttonList)
		{
			bHUIButton.Enable(aEnable);
		}
	}
}
