﻿/*
 * Author: Grace Barrett-Snyder 
 * Description: Controls the shop screen
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPShopUIController : PPUIController
{
    [SerializeField]
    CurrencyDisplay dogFoodDisplay;
    [SerializeField]
    CurrencyDisplay coinDisplay;

    ShopItem[] items;
    ShopItemSlot[] itemSlots;
    ShopDatabase shop;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        base.setReferences();
        itemSlots = GetComponentsInChildren<ShopItemSlot>();
        shop = ShopDatabase.Instance;
        shop.Initialize();
        items = shop.Items;
        populateShop(items);
    }

    protected override void fetchReferences()
    {
        base.fetchReferences();
        EventController.Event(PPEvent.LoadShop);

        // Set Currency Displays
		// Set Currency Displays
		dogFoodDisplay.Init(dataController, CurrencyType.DogFood);
		coinDisplay.Init(dataController, CurrencyType.Coins);
    }

    #endregion

    void populateShop(ShopItem[] items)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            ShopItemSlot itemSlot = itemSlots[i];
            itemSlot.Init(this, items[i]);
        }
    }

    public void OnMenuClick()
    {
        sceneController.LoadHome();
    }

    public void OnAdoptClick()
    {
        sceneController.LoadShelter();
    }

}
