using UnityEngine;

public class bgArt44_Behavior : MonoBehaviour
{
	private const float kReferenceScreenHeight = 640f;

	private void Start()
	{
		base.transform.Find("InstructionText").GetComponent<TextMesh>().text = LocalizationManager.Instance.GetString("TXT_Instructions7");
		float num = 640f / (float)Screen.height;
		base.transform.Find("InstructionText").transform.localScale *= num;
		if (SizeCategory.Instance.Category == "small")
		{
			Vector3 localPosition = base.transform.Find("ForegroundImage").transform.localPosition;
			localPosition.y = 6f;
			base.transform.Find("ForegroundImage").transform.localPosition = localPosition;
		}
		else if (SizeCategory.Instance.Category == "medium" || SizeCategory.Instance.Category == "large")
		{
			Vector3 localPosition2 = base.transform.Find("ForegroundImage").transform.localPosition;
			localPosition2.y = 4f;
			base.transform.Find("ForegroundImage").transform.localPosition = localPosition2;
		}
	}
}
