using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIInventoryTabs : MonoBehaviour
{
	[SerializeField] private List<UIInventoryTab> _tabs = new List<UIInventoryTab>();
	[SerializeField] List<InventoryTabType> _tabTypes = new List<InventoryTabType>();
	[SerializeField] InventoryTabSOEventChannel _tabClickedChannel;
	[SerializeField] InventoryTabSOEventChannel _tabChangedChannel;

	private bool _canDisableLayout = false; //unused?

	private void Awake() => Debug.Assert(_tabChangedChannel != null);

	private void OnDisable()
	{
		//foreach (UIInventoryTab tab in _tabs)
		//{
		//	tab.TabClicked -= ChangeTab;
		//}
		_tabClickedChannel.OnRaiseEvent -= ChangeTab;
	}

	private void OnEnable()
	{
		//if ((gameObject.GetComponent<VerticalLayoutGroup>() != null) && _canDisableLayout)
		//{
		//	gameObject.GetComponent<VerticalLayoutGroup>().enabled = false;
		//	_canDisableLayout = false;
		//}

		//foreach (var tab in _tabs)
		//{
		//	tab.gameObject.SetActive(true);
		//	tab.TabClicked += ChangeTab;
		//}

		_tabClickedChannel.OnRaiseEvent += ChangeTab;
		//_tabs[0].ClickButton(); //just sets the first tab to be selected

		StartCoroutine(WaitForCanDisable());
	}

	IEnumerator WaitForCanDisable()
	{
		yield return new WaitForSeconds(1);
		//disable layout group after layout calculation
		if (gameObject.GetComponent<VerticalLayoutGroup>() is VerticalLayoutGroup v)
		{
			v.enabled = false;
			_canDisableLayout = false;
		}
	}

	void ChangeTab(InventoryTabSO newTabType) => _tabChangedChannel?.RaiseEvent(newTabType);
}
