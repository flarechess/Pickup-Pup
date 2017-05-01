﻿/*
 * Author: Grace Barrett-Snyder 
 * Description: Controls a slot for an item in the shop
 */

using UnityEngine;
using UnityEngine.UI;

public class ShopItemSlot : PPUIElement
{
    [SerializeField]
    Text nameText;
    [SerializeField]
    Image itemImage;
	[SerializeField]
	Text amountText;

    [SerializeField]
    PriceTag priceTag;
    Button button;

    ShopItem item;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        base.setReferences();
        button = GetComponent<Button>();
    }

    protected override void fetchReferences()
    {
        base.fetchReferences();
        dataController = PPDataController.GetInstance;
    }

    protected override void subscribeEvents()
    {
        base.subscribeEvents();
        if (dataController)
        {
            dataController.SubscribeToCurrencyChange(CurrencyType.Coins, tryToggle);
        }
    }

    protected override void unsubscribeEvents()
    {
        base.unsubscribeEvents();
        if (dataController)
        {
            dataController.UnsubscribeFromCurrencyChange(CurrencyType.Coins, tryToggle);
        }
    }

    #endregion

    public void Init(ShopItem item)
    {
        subscribeEvents();
        this.item = item;
        if (nameText)
        {
            nameText.text = item.ItemName;
        }
		if(amountText)
		{
			amountText.text = item.Value.ToString();
		}
        priceTag.Set(item.Cost);
        priceTag.ShowPurchasable();
        itemImage.sprite = item.Icon;
    }

    public void Buy()
    {
        // If can afford to, reduce coins by cost.
        gameController.TryBuyItem(item);
             
        // If buying dogfood then update the amount of the appropriate DogFoodType
        if (item.ValueCurrencyType == CurrencyType.DogFood)
        {
            gameController.TryBuyFood(item);
        }
    }
    
    void tryToggle(int amount)
    {
        if(button)
        {
            button.interactable = gameController.CanAfford(item.CostCurrencyType, item.Cost);
        }
    }

}
