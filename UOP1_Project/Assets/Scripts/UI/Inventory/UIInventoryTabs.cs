using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIInventoryTabs : MonoBehaviour
{
	[SerializeField] private List<UIInventoryTab> _tabs = new List<UIInventoryTab>();
	[SerializeField] List<InventoryTabType> _tabTypes = new List<InventoryTabType>();

	public event UnityAction<InventoryTabSO> TabChanged;

	private bool _canDisableLayout = false; //unused?

	private void OnDisable()
	{
		for (int i = 0; i < _tabs.Count; i++)
		{

			_tabs[i].TabClicked -= ChangeTab;
		}
	}

	private void OnEnable()
	{
		//if ((gameObject.GetComponent<VerticalLayoutGroup>() != null) && _canDisableLayout)
		//{
		//	gameObject.GetComponent<VerticalLayoutGroup>().enabled = false;
		//	_canDisableLayout = false;
		//}

		foreach (var tab in _tabs)
		{
			tab.gameObject.SetActive(true);
			tab.TabClicked += ChangeTab;
		}

		StartCoroutine(WaitForCanDisable());
	}


	public void SetTabs(List<InventoryTabSO> typesList, InventoryTabSO selectedType)
	{
		//if (gameObject.GetComponent<VerticalLayoutGroup>() != null)
		//	gameObject.GetComponent<VerticalLayoutGroup>().enabled = true;

		int maxCount = Mathf.Max(typesList.Count, _tabs.Count);

		for (int i = 0; i < maxCount; i++)
		{
			if (i < typesList.Count)
			{
				if (i >= _tabs.Count)
				{
					Debug.LogError("Maximum tabs reached");
				}
				bool isSelected = typesList[i] == selectedType;
				//fill
				_tabs[i].SetTab(typesList[i], isSelected);
				_tabs[i].gameObject.SetActive(true);
				_tabs[i].TabClicked += ChangeTab;

			}
			else if (i < _tabs.Count)
			{
				//Desactive
				_tabs[i].gameObject.SetActive(false);
			}
		}
		if (isActiveAndEnabled) // check if the game object is active and enabled so that we could start the coroutine. 
		{
			StartCoroutine(WaitForCanDisable());
		}
		else // if the game object is inactive, disabling the layout will happen on onEnable 
		{
			_canDisableLayout = true;
		}
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

	public void ChangeTabSelection(InventoryTabSO selectedType)
	{
		foreach (UIInventoryTab t in _tabs)
		{
			bool isSelected = t.TabData == selectedType;
			t.UpdateState(isSelected);
		}
	}

	void ChangeTab(InventoryTabSO newTabType)
	{
		ChangeTabSelection(newTabType);
		TabChanged?.Invoke(newTabType);
	}
}
