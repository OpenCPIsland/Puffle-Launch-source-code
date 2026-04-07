using UnityEngine;

public class ErrorButtonController : MonoBehaviour
{
	public Transform errorButton;

	public Transform buttonToDisable;

	private bool mErrorHappened;

	public bool ErrorHappened
	{
		get
		{
			return mErrorHappened;
		}
		set
		{
			mErrorHappened = value;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (ErrorHappened)
		{
			buttonToDisable.GetComponent<Button3DPressStateController>().Enabled = false;
		}
		else
		{
			buttonToDisable.GetComponent<Button3DPressStateController>().Enabled = true;
		}
	}
}
