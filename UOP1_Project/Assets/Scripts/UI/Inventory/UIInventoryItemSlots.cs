#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIInventoryItemSlots : MonoBehaviour
{
	[SerializeField, ReadOnly] List<UIInventoryItem> _slots = new List<UIInventoryItem>();
	ViewModelList _viewModelList = default!;
	private ItemSO? _selected;

	[SerializeField] 

	void Awake()
    {
		GetComponentsInChildren(_slots);
		_viewModelList = ViewModelList.Create(_slots);
		// TODO register the ItemSelected callback to run the Select callback
		// ItemSelected should change its paramater to UIInventoryItem
		//then the callback to UIInventory should just use the itemStack field
		// also that way the current slot can easily call any unselect things

    }

	
	internal void Draw(IEnumerable<ItemStack> items)
	{
		_viewModelList.SetItems(items);
	}

	internal void UpdateView()
	{
		_viewModelList.Update();
	}

	internal void SelectFirstElement()
	{
		_viewModelList[0].Select();
	}

	internal void Select(ItemSO item)
	{
		if (_selected == item)
			return;

		//_viewModelList.Unselect(item);
		_selected = item;
	}

	class ViewModel
	{
		readonly UIInventoryItem _slot;
		ItemStack? _item;
		int _amount = -1;

		private ViewModel(UIInventoryItem slot) => _slot = slot;

		private ViewModel(UIInventoryItem slot, ItemStack item)
		{
			(_slot, _item) = (slot, item);
			_amount = item.Amount;
		}

		internal static ViewModel CreateEmpty(UIInventoryItem slot) =>
			new ViewModel(slot);

		internal static ViewModel Create(UIInventoryItem slot, ItemStack item) =>
			new ViewModel(slot, item);


		internal void SetItem(ItemStack item)
		{
			(_item, _amount) = (item, item.Amount);
			_slot.SetItem(item, false);
		}

		internal void Clear()
		{
			_slot.ClearItem();
			(_item, _amount) = (null, -1);
		}

		internal void Update()
		{
			if (_item is null || _item.Amount == _amount)
				return; //no update needed, should maybe check for item going from a value to null?

			_amount = _item.Amount;
			//set the slot _itemCount field
		}

		internal void Select()
		{
			if (_item == null)
				return;
			_slot.SelectFirstElement();
		}
	}

	class ViewModelList
	{
		List<ViewModel> _items = new List<ViewModel>();

		internal ViewModel this[int index] => _items[index];

		internal void SetItems(IEnumerable<ItemStack> items)
		{
			var index = 0;
			foreach (var item in _items)
			{
				item.Clear();
			}

			foreach (var item in items)
			{
				this[index].SetItem(item);
				index++;
			}
		}

		internal static ViewModelList Create(List<UIInventoryItem> slots) =>
			new ViewModelList() { _items = slots.Select(ViewModel.CreateEmpty).ToList() };

		internal void Update()
		{
			foreach (var item in _items)
			{
				item.Update();
			}
		}
	}
}
