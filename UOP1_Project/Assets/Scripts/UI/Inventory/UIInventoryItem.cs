using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization.Components;
using UnityEngine.Events;
using System;

public class UIInventoryItem : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _itemCount = default;
	[SerializeField] private Image _itemPreviewImage = default;
	[SerializeField] private Image _bgImage = default;
	[SerializeField] private Image _imgHover = default;
	[SerializeField] private Image _imgSelected = default;
	[SerializeField] private Image _bgInactiveImage = default;
	[SerializeField] private Button _itemButton = default;
	[SerializeField] private LocalizeSpriteEvent _bgLocalizedImage = default;

	public UnityAction<ItemSO> ItemSelected;
	
	[HideInInspector] public ItemStack currentItem;
	
	bool _isSelected = false;

	public void SetItem(ItemStack itemStack, bool isSelected)
	{
		Debug.Assert(itemStack != null);

		currentItem = itemStack;
		SetState(true);
		_imgSelected.gameObject.SetActive(isSelected);
		
		if (currentItem.Item.IsLocalized)
		{
			_bgLocalizedImage.enabled = true;
			_bgLocalizedImage.AssetReference = itemStack.Item.LocalizePreviewImage;
		}
		else
		{
			_bgLocalizedImage.enabled = false;
			_itemPreviewImage.sprite = itemStack.Item.PreviewImage;
		}
		_itemCount.text = itemStack.Amount.ToString();
		_bgImage.color = itemStack.Item.ItemType.TypeColor;
	}

	public void ClearItem()
	{
		currentItem = null;
		SetState(false);
		_imgSelected.gameObject.SetActive(false);
	}

	private void SetState(bool isActive)
	{
		_itemPreviewImage.gameObject.SetActive(isActive);
		_itemCount.gameObject.SetActive(isActive);
		_bgImage.gameObject.SetActive(isActive);
		_imgHover.gameObject.SetActive(false);
		_itemButton.gameObject.SetActive(isActive);
		_bgInactiveImage.gameObject.SetActive(!isActive);
	}

	public void SelectFirstElement()
	{
		_isSelected = true;
		_itemButton.Select();
		SelectItem();
	}

	private void OnEnable()
	{
		if (_isSelected)
		{ SelectItem(); }
	}

	public void OnHoverChanged(bool isHovering) =>
		_imgHover.gameObject.SetActive(isHovering);

	public void SelectItem()
	{
		_isSelected = true;
		if (ItemSelected != null && currentItem != null && currentItem.Item != null)

		{
			_imgSelected.gameObject.SetActive(true);
			ItemSelected.Invoke(currentItem.Item);
		}
		else
		{
			_imgSelected.gameObject.SetActive(false);
		}
	}

	public void UnselectItem()
	{
		_isSelected = false;
		_imgSelected.gameObject.SetActive(false);
	}

	void OnSelect()
	{
		if (currentItem == null && currentItem.Item == null)
			return;

		_imgSelected.gameObject.SetActive(true);
		ItemSelected?.Invoke(currentItem.Item);
	}

	void OnDeselect()
	{
		_imgSelected.gameObject.SetActive(false);
	}
	
}
