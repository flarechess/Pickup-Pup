﻿/*
 * Author: Grace Barrett-Snyder 
 * Description: Stores data for the Player's dog food currency.
 */

using System.IO;
using UnityEngine;
using k = PPGlobal;

[System.Serializable]
public class DogFoodData : CurrencyData
{
	#region Instance Accessors 

	public string FoodType
	{
		get
		{
			return foodType;
		}
	}

	public float SpecialGiftMod
	{
		get
		{
			return specialGiftMod;
		}
	}

	public float AmountMod
	{
		get
		{
			return amountMod;
		}
	}

	public Color Color
	{
		get
		{
			if(!colorIsSet)
			{
				ColorUtility.TryParseHtmlString(colorHex, out _color);
			}
			return _color;
		}
	}

	public string ColorStr
	{
		get
		{
			return color;
		}
	}

	public string ColorHex
	{
		get
		{
			return colorHex;
		}
	}

	#region CurrencyData Overrides

	public override Sprite Icon
	{
		get
		{
			return FoodDatabase.DefaultSprite;
		}
	}

	#endregion

	#endregion

	[SerializeField]
	string foodType;
	[SerializeField]
	float specialGiftMod;
	[SerializeField]
	float amountMod;
	[SerializeField]
	string color;
	[SerializeField]
	string colorHex;

	[System.NonSerialized]
	Color _color = Color.white;
	bool colorIsSet = false;

    public DogFoodData(int initialAmount) : base(initialAmount)
    {
        type = CurrencyType.DogFood;
        amount = initialAmount;
    }

	public static DogFoodData Default()
	{
		DogFoodData defaultFood = new DogFoodData(k.NONE_VALUE);
		defaultFood.foodType = k.DEFAULT_FOOD_TYPE;
		defaultFood.amountMod = k.NONE_VALUE;
		defaultFood.specialGiftMod = k.NONE_VALUE;
		defaultFood.colorHex = string.Format("#{0}", ColorUtility.ToHtmlStringRGB(Color.white));
		defaultFood.color = Color.white.ToString();
		return defaultFood;
	}

	#region CurrencyData Overrides

	public override void Give()
	{
		dataController.ChangeFood(this.Amount);
	}

    #endregion

	public DogFoodData Copy()
	{
		return Copy<DogFoodData>();
	}

}
