using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryItemSlots : MonoBehaviour
{
	[SerializeField, ReadOnly] List<UIInventoryItem> _slots = new List<UIInventoryItem>();

    void Awake()
    {
		GetComponentsInChildren(_slots);
    }

	//needs data binding

	//the Draw method should go here, but only for updating when a tab is switched
	//should probably have OnSelect calls as well, try and get as much ItemSlot logic stuff out
	// of UIInventory and keep that just general calls
	internal void Draw(IEnumerable<ItemStack> items)
	{
		var index = 0;
		foreach (var item in items)
		{
			_slots[index].SetItem(item, false);
			index++;
		}

		for (int i = index; i < _slots.Count; i++)
		{
			_slots[i].ClearItem();
		}

		//HideItemInformation();

		//if (_selectedItemId >= 0)
		//{
		//	UnselectItem(_selectedItemId);
		//	_selectedItemId = -1;
		//}

		_slots[0].SelectFirstElement();
	}

	//some sort of binding that maybe keeps track of the current slots and what is in them
	//along with the actual ItemStacks and updates that way
}
