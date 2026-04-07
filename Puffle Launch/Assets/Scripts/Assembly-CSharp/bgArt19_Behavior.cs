using UnityEngine;

public class bgArt19_Behavior : MonoBehaviour
{
	private const float kReferenceScreenHeight = 640f;

	private void Start()
	{
		base.transform.Find("InstructionText").GetComponent<TextMesh>().text = LocalizationManager.Instance.GetString("TXT_Instructions6");
		float num = 640f / (float)Screen.height;
		base.transform.Find("ForegroundImage").transform.localPosition *= num;
		base.transform.Find("InstructionText").transform.localScale *= num;
	}
}
