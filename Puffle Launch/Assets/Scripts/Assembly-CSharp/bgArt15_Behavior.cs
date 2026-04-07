using UnityEngine;

public class bgArt15_Behavior : MonoBehaviour
{
	private const float kReferenceScreenHeight = 640f;

	private void Start()
	{
		UpdateString();
		float num = 640f / (float)Screen.height;
		base.transform.localPosition *= num;
		base.transform.Find("Device").transform.localPosition *= num;
		base.transform.Find("InstructionText").transform.localScale *= num;
	}

	private void UpdateString()
	{
		TextMesh component = base.transform.Find("InstructionText").GetComponent<TextMesh>();
		component.text = LocalizationManager.Instance.GetString(component.text);
	}
}
