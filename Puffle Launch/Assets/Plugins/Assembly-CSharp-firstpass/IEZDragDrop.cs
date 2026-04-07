using UnityEngine;

public interface IEZDragDrop
{
	object Data { get; set; }

	bool IsDraggable { get; set; }

	LayerMask DropMask { get; set; }

	bool IsDragging { get; set; }

	GameObject DropTarget { get; set; }

	bool DropHandled { get; set; }

	float DragOffset { get; set; }

	EZAnimation.EASING_TYPE CancelDragEasing { get; set; }

	float CancelDragDuration { get; set; }

	void DragUpdatePosition(POINTER_INFO ptr);

	void CancelDrag();

	void OnEZDragDrop_Internal(EZDragDropParams parms);

	void AddDragDropDelegate(EZDragDropDelegate del);

	void RemoveDragDropDelegate(EZDragDropDelegate del);

	void SetDragDropDelegate(EZDragDropDelegate del);
}
