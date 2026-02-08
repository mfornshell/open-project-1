using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIInventoryTab : MonoBehaviour
{
	public UnityAction<InventoryTabSO> TabClicked;
	
	[SerializeField] private Image _tabImage = default;
	[SerializeField] private Button _actionButton = default;
	[SerializeField] private Color _selectedIconColor = default;
	[SerializeField] private Color _deselectedIconColor = default;

	public InventoryTabSO TabData = default;

	private void Start()
	{
		_tabImage.sprite = TabData.TabIcon;
		UpdateState(false);
	}


	public void SetTab(InventoryTabSO tabData, bool isSelected)
	{
		TabData = tabData;
		_tabImage.sprite = tabData.TabIcon;

		UpdateState(isSelected);
	}

	public void UpdateState(bool isSelected)
	{
		_actionButton.interactable = !isSelected;

		_tabImage.color = isSelected ? _selectedIconColor : _deselectedIconColor;
	}

	public void ClickButton()
	{
		TabClicked?.Invoke(TabData);
	}
}
