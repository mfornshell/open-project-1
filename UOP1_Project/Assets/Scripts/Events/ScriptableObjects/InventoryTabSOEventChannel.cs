using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/InventoryTabSO Event Channel")]
public class InventoryTabSOEventChannel : DescriptionBaseSO
{
	public event Action<InventoryTabSO> OnRaiseEvent;

	public void RaiseEvent(InventoryTabSO inventoryTab) => OnRaiseEvent?.Invoke(inventoryTab);
}
