using System.Collections.Generic;

public interface IUIPanelExtension
{
	List<BHUIButton> ButtonList { get; set; }

	List<string> ButtonName { get; set; }

	List<BHUITexture> TextureList { get; set; }

	List<string> TextureName { get; set; }

	List<BHUILabel> LabelList { get; set; }

	List<string> LabelName { get; set; }

	List<BHUITextField> TextFieldList { get; set; }

	List<string> TextFieldName { get; set; }

	BHUIPanelManager PanelManager { get; set; }

	UIControlHandler<BHUIButton> ButtonHandler { get; }

	UIControlHandler<BHUITexture> TextureHandler { get; }

	UIControlHandler<BHUILabel> LabelHandler { get; }

	UIControlHandler<BHUITextField> TextFieldHandler { get; }

	void Activate(bool aActivate);
}
