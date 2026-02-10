using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class UIInventory : MonoBehaviour
{
	public UnityAction Closed;

	[SerializeField] private InputReader _inputReader = default;
	[SerializeField] private InventorySO _currentInventory = default;
	[SerializeField] private UIInventoryItem _itemPrefab = default;
	[SerializeField] private GameObject _contentParent = default;
	[SerializeField] private GameObject _errorPotMessage = default;
	[SerializeField] private UIInventoryInspector _inspectorPanel = default;
	[SerializeField] private List<InventoryTabSO> _inventoryTabs = new List<InventoryTabSO>();
	[SerializeField] private List<UIInventoryItem> _availableItemSlots = default;

	[Header("Listening to")]
	[SerializeField] private UIInventoryTabs _tabsPanel = default;
	[SerializeField] private UIActionButton _actionButton = default;
	[SerializeField] private VoidEventChannelSO _onInteractionEndedEvent = default;
	[SerializeField] InventoryTabSOEventChannel _tabChangedEvent;

	[Header("Broadcasting on")]
	[SerializeField] private ItemEventChannelSO _useItemEvent = default;
	[SerializeField] private IntEventChannelSO _restoreHealth = default;
	[SerializeField] private ItemEventChannelSO _equipItemEvent = default;
	[SerializeField] private ItemEventChannelSO _cookRecipeEvent = default;
	[SerializeField] private InventoryTabSOEventChannel _tabSwitchedEvent;

	private InventoryTabSO _selectedTab = default;
	private bool _isNearPot = false;
	private int _selectedItemId = -1;

	private void OnEnable()
	{
		_actionButton.Clicked += OnActionButtonClicked;
		_onInteractionEndedEvent.OnEventRaised += InteractionEnded;
		_tabChangedEvent.OnRaiseEvent += OnChangeTab;

		for (int i = 0; i < _availableItemSlots.Count; i++)
		{
			_availableItemSlots[i].ItemSelected += InspectItem;
		}

		_selectedTab = _inventoryTabs[0];
		_selectedItemId = -1;

		_inputReader.TabSwitched += OnSwitchTab;
	}

	private void OnDisable()
	{
		_actionButton.Clicked -= OnActionButtonClicked;
		_onInteractionEndedEvent.OnEventRaised -= InteractionEnded;
		_tabChangedEvent.OnRaiseEvent -= OnChangeTab;

		for (int i = 0; i < _availableItemSlots.Count; i++)
		{
			_availableItemSlots[i].ItemSelected -= InspectItem;
		}

		_inputReader.TabSwitched -= OnSwitchTab;
	}

	private void OnSwitchTab(float orientation)
	{
		if (Mathf.Approximately(orientation, 0))
			return;

		var index = _inventoryTabs.IndexOf(_selectedTab);

		index += (orientation < 0 ? -1 : 1);

		index = index < 0 ? _inventoryTabs.Count - 1
			: index >= _inventoryTabs.Count ? 0
			: index;

		_tabSwitchedEvent?.RaiseEvent(_inventoryTabs[index]);
	}

	public void DrawInventory(InventoryTabType _selectedTabType = InventoryTabType.CookingItem, bool isNearPot = false)
	{
		_isNearPot = isNearPot;

		var items = _currentInventory.Items.Where(x => x.Item.ItemType.TabType == _selectedTab);

		DrawInventoryItems(items);
	}


	// TODO should be a class called InventoryItemSlots that holds all the slots
	// and can perform draw calculations like this. Also being able to bind each slot to a value
	// ie the ItemStack and update itself if the item stack changes the amount value OR goes null
	// OR changes to a different ItemStack completely
	private void DrawInventoryItems(IEnumerable<ItemStack> items)
	{
		var index = 0;

		foreach (var item in items)
		{
			var isSelected = _selectedItemId == index;
			_availableItemSlots[index].SetItem(item, isSelected);
			index++;
		}

		for (int i = index; i < _availableItemSlots.Count; i++)
		{
			_availableItemSlots[i].ClearItem();
		}

		HideItemInformation();

		if (_selectedItemId >= 0)
		{
			UnselectItem(_selectedItemId);
			_selectedItemId = -1;
		}

		_availableItemSlots[0].SelectFirstElement();
	}

	void DrawInventoryItems(List<ItemStack> items)
	{
		var index = 0;

		while (index < _availableItemSlots.Count)
		{
			if (index < items.Count)
			{
				bool isSelected = _selectedItemId == index;
				_availableItemSlots[index].SetItem(items[index], isSelected);
			}
			else
			{
				_availableItemSlots[index].ClearItem();
			}
			++index;
		}

		HideItemInformation();

		if (_selectedItemId >= 0)
		{
			UnselectItem(_selectedItemId);
			_selectedItemId = -1;
		}

		_availableItemSlots[0].SelectFirstElement();
	}

	void HideItemInformation()
	{
		_actionButton.gameObject.SetActive(false);
		_inspectorPanel.gameObject.SetActive(false);
	}

	void InteractionEnded()
	{
		_isNearPot = false;
	}

	void UpdateItemInInventory(ItemStack itemToUpdate, bool removeItem)
	{
		if (_availableItemSlots == null)
			_availableItemSlots = new List<UIInventoryItem>();

		if (removeItem)
		{
			if (_availableItemSlots.Exists(o => o.currentItem == itemToUpdate))
			{

				int index = _availableItemSlots.FindIndex(o => o.currentItem == itemToUpdate);
				_availableItemSlots[index].ClearItem();
			}
		}
		else
		{
			int index = 0;

			//if the item has already been created
			if (_availableItemSlots.Exists(o => o.currentItem == itemToUpdate))
			{
				index = _availableItemSlots.FindIndex(o => o.currentItem == itemToUpdate);
			}
			//if the item needs to be created
			else
			{
				//if the new item needs to be instantiated
				if (_currentInventory.Items.Count > _availableItemSlots.Count)
				{
					UIInventoryItem instantiatedPrefab = Instantiate(_itemPrefab, _contentParent.transform) as UIInventoryItem;
					_availableItemSlots.Add(instantiatedPrefab);
				}

				//find the last instantiated game object not used
				index = _currentInventory.Items.Count;
			}

			bool isSelected = _selectedItemId == index;
			_availableItemSlots[index].SetItem(itemToUpdate, isSelected);
		}
	}

	public void InspectItem(ItemSO itemToInspect)
	{
		if (_availableItemSlots.Exists(o => o.currentItem.Item == itemToInspect))
		{
			int itemIndex = _availableItemSlots.FindIndex(o => o.currentItem.Item == itemToInspect);

			//unselect selected Item
			if (_selectedItemId >= 0 && _selectedItemId != itemIndex)
				UnselectItem(_selectedItemId);

			//change Selected ID 
			_selectedItemId = itemIndex;

			//show Information
			ShowItemInformation(itemToInspect);

			//check if interactable
			bool isInteractable = true;
			_actionButton.gameObject.SetActive(true);
			_errorPotMessage.SetActive(false);
			if (itemToInspect.ItemType.ActionType == ItemInventoryActionType.Cook)
			{
				isInteractable = _currentInventory.hasIngredients(itemToInspect.IngredientsList) && _isNearPot;
				_errorPotMessage.SetActive(!_isNearPot);
			}
			else if (itemToInspect.ItemType.ActionType == ItemInventoryActionType.DoNothing)
			{
				isInteractable = false;
				_actionButton.gameObject.SetActive(false);
			}

			//set button
			_actionButton.FillInventoryButton(itemToInspect.ItemType, isInteractable);
		}
	}

	void ShowItemInformation(ItemSO item)
	{
		bool[] availabilityArray = _currentInventory.IngredientsAvailability(item.IngredientsList);

		_inspectorPanel.FillInspector(item, availabilityArray);
		_inspectorPanel.gameObject.SetActive(true);
	}

	void UnselectItem(int itemIndex) => _availableItemSlots[itemIndex].UnselectItem();

	void UpdateInventory()
	{
		DrawInventory(_selectedTab.TabType, _isNearPot);
	}

	void OnActionButtonClicked()
	{
		//find the selected Item
		if (_availableItemSlots.Count > _selectedItemId
			&& _selectedItemId > -1)
		{
			ItemSO itemToActOn = ScriptableObject.CreateInstance<ItemSO>();
			itemToActOn = _availableItemSlots[_selectedItemId].currentItem.Item;

			//check the selected Item type
			//call action function depending on the itemType
			switch (itemToActOn.ItemType.ActionType)
			{
				case ItemInventoryActionType.Cook:
					CookRecipe(itemToActOn);
					break;
				case ItemInventoryActionType.Use:
					UseItem(itemToActOn);
					break;
				case ItemInventoryActionType.Equip:
					EquipItem(itemToActOn);
					break;
				default:

					break;
			}
		}
	}

	void UseItem(ItemSO itemToUse)
	{
		if (itemToUse.HealthResorationValue > 0)
		{ _restoreHealth.RaiseEvent(itemToUse.HealthResorationValue); }
		_useItemEvent.RaiseEvent(itemToUse);
		UpdateInventory();
	}

	void EquipItem(ItemSO itemToUse)
	{
		Debug.Log("Equip ITEM " + itemToUse.name);
		_equipItemEvent.RaiseEvent(itemToUse);
	}

	void CookRecipe(ItemSO recipeToCook)
	{
		_cookRecipeEvent.RaiseEvent(recipeToCook);

		//update inspector
		InspectItem(recipeToCook);

		//update inventory
		UpdateInventory();
	}

	void OnChangeTab(InventoryTabSO tabType)
	{
		_selectedTab = tabType;
		DrawInventory(_selectedTab.TabType, _isNearPot);
	}

	public void CloseInventory()
	{
		Closed.Invoke();
	}
}
