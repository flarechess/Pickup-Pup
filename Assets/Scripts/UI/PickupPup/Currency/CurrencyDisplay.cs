﻿/*
 * Authors: Grace Barrett-Snyder, Isaiah Mann
 * Description: Controls UI of Currency
 */

using UnityEngine;
using UnityEngine.UI;

public class CurrencyDisplay : PPUIElement 
{
    [SerializeField]
    Image iconImage;

	CurrencyType type;
    DogFoodType dogFoodType;
    CurrencyDisplay[] dogFoodDisplays;

    public void Init(CurrencyData currency, PPDataController dataController)
	{
        this.dataController = dataController;
		unsubscribeEvents();
		type = currency.Type;
        iconImage.sprite = currency.Icon;
        updateAmount(currency.Amount);
		this.dataController.SubscribeToCurrencyChange(type, updateAmount);
	}

    public void Init(CurrencyData currency, PPDataController dataController, DogFoodType _dogFoodType, CurrencyDisplay[] _dogFoodDisplays)
    {
        this.dataController = dataController;
        unsubscribeEvents();
        type = currency.Type;
        dogFoodType = _dogFoodType;
        dogFoodDisplays = _dogFoodDisplays;
        // TODO: Have to add sprite to FoodItem JSON (or just use color somehow?)
        // iconImage.sprite = FoodDatabase.Instance.Items[(int)dogFoodType].sprite;
        updateFoodAmount(FoodDatabase.Instance.Food[(int)dogFoodType].CurrentAmount);
        this.dataController.SubscribeToCurrencyChange(type, updateFoodAmount);
    }

    protected override void cleanupReferences()
	{
		base.cleanupReferences();
		if(dataController)
		{
			dataController.UnsubscribeFromCurrencyChange(type, updateAmount);
		}
	}

	protected override void subscribeEvents()
	{
		base.subscribeEvents();
	}

	protected override void unsubscribeEvents()
	{
		base.unsubscribeEvents();
	}

	public void updateAmount(int newAmount)
	{
		text.text = newAmount.ToString();
	}

    public void updateFoodAmount(int newAmount)
    {
        Debug.Log(FoodDatabase.Instance.Food[(int)dogFoodType].CurrentAmount);
        Debug.Log(dogFoodType + "!!! :)");
        // for each dog food there is, o
        text.text = newAmount.ToString();
    }
}
