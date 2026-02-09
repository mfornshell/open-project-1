using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIInventoryTab : MonoBehaviour
{
	[SerializeField] private Image _tabImage = default;
	[SerializeField] private Button _actionButton = default;
	[SerializeField] private Color _selectedIconColor = default;
	[SerializeField] private Color _deselectedIconColor = default;

	[SerializeField] InventoryTabSO _tabData;

	[SerializeField] InventoryTabSOEventChannel _tabClickedChannel;
	[SerializeField] InventoryTabSOEventChannel _tabChangedListener;

	private void Awake() => Debug.Assert(_tabChangedListener != null);

	private void Start()
	{
		_tabImage.sprite = _tabData.TabIcon;
		
		UpdateState(false);
	}

	private void OnEnable() => _tabChangedListener.OnRaiseEvent += UpdateState;
	private void OnDisable() => _tabChangedListener.OnRaiseEvent -= UpdateState;

	void UpdateState(bool isSelected)
	{
		_actionButton.interactable = !isSelected;

		_tabImage.color = isSelected ? _selectedIconColor : _deselectedIconColor;
	}

	void UpdateState(InventoryTabSO selectedTab) => UpdateState(selectedTab == _tabData);

	public void ClickButton() => _tabClickedChannel?.RaiseEvent(_tabData);
}
