﻿/*
 * Author: Isaiah Mann
 * Desc: Used to store tuning variables
 */

using UnityEngine;

[System.Serializable]
public class PPTuning : PPData 
{
	#region Instance Acessors

	public int MaxDogsScouting
	{
		get
		{
			return maxDogsScouting;
		}
	}

	public int MaxDogsInHouse
	{
		get
		{
			return maxDogsInHouse;
		}
	}

	public int DailyCoinBonus
	{
		get
		{
			return dailyCoinBonus;
		}
	}

	public int DailyNumberOfNewDogsAtShelter
	{
		get
		{
			return dailyNumberOfNewDogsAtShelter;
		}
	}

	public int MinDogsToUnlockCollar
	{
		get
		{
			return minDogsToUnlockCollar;
		}
	}

	public int MinDogFoodToUnlockCollar
	{
		get
		{
			return minDogFoodToUnlockCollar;
		}
	}

	public int MissedFeedingsBeforeSeizure
	{
		get
		{
			return missedFeedingsBeforeSeizure;
		}
	}

	public int SpecialDogCount
	{
		get
		{
			return specialDogCount;
		}
	}

	public int MaxNumberAdsPerDay
	{
		get
		{
			return maxNumberAdsPerDay;
		}
	}

	public int VideoAdCoinBonus
	{
		get
		{
			return videoAdCoinBonus;
		}
	}

	public int CostOfOneDogFoodCan
	{
		get
		{
			return costOfOneDogFoodCan;
		}
	}

	public int CostOfFiveDogFoodCans
	{
		get
		{
			return costOfFiveDogFoodCans;
		}
	}

	public int CostOfTenDogFoodCans
	{
		get
		{
			return costOfTenDogFoodCans;
		}
	}

	public int CostOfTwentyDogFoodCans
	{
		get
		{
			return costOfTwentyDogFoodCans;
		}
	}

	public int MaxPuppyAge
	{
		get
		{
			return maxPuppyAge;
		}
	}

	public int MinSeniorDogAge
	{
		get
		{
			return minSeniorDogAge;
		}
	}

	public float TimeScoutingToTimeRestingRatio
	{
		get
		{
			return timeScoutingToTimeRestingRatio;
		}
	}

	public float ChanceOfDogsEatingPoop
	{
		get
		{
			return chanceOfDogsEatingPoop;
		}
	}

	public float MinCouponDiscount
	{
		get
		{
			return minCouponDiscount;
		}
	}

	public float MaxCouponDiscount
	{
		get
		{
			return maxCouponDiscount;
		}
	}

	public float ShelterResellCostScale
	{
		get
		{
			return shelterResellCostScale;
		}
	}

	public float DefaultChanceOfCollectingDogFood
	{
		get
		{
			return defaultChanceOfCollectingDogFood;
		}
	}

	public float DefaultChanceOfCollectingMoney
	{
		get
		{
			return defaultChanceOfCollectingMoney;
		}
	}

	public float PuppyChanceOfRunningAwayDuringScouting
	{
		get
		{
			return puppyChanceOfRunningAwayDuringScouting;
		}
	}

	public float PuppyScoutingTimeModifier
	{
		get
		{
			return puppyScoutingTimeModifier;
		}
	}

	public float SeniorDogScoutingTimeModifier
	{
		get
		{
			return seniorDogScoutingTimeModifier;
		}
	}

	public int MaxAmountPerTypeFromScouting
	{
		get
		{
			return maxAmountPerTypeFromScouting;
		}
	}

	public float ChanceOfRareObject
	{
		get
		{
			return chanceOfRareObject;
		}
	}

	public float ChanceSpecialItemIsPresent
	{
		get
		{
			return chanceSpecialItemIsPresent;
		}
	}

	public float ChanceSpecialItemIsDog
	{
		get
		{
			return chanceSpecialItemIsDog;
		}
	}

	public float ChanceSpecialItemIsTubTub
	{
		get
		{
			return chanceSpecialItemIsTubTub;
		}
	}

	#endregion

	#region JSON Fields

	[SerializeField]
	int maxDogsScouting;
	[SerializeField]
	int maxDogsInHouse;
	[SerializeField]
	int dailyCoinBonus;
	[SerializeField]
	int dailyNumberOfNewDogsAtShelter;
	[SerializeField]
	int minDogsToUnlockCollar;
	[SerializeField]
	int minDogFoodToUnlockCollar;
	[SerializeField]
	int missedFeedingsBeforeSeizure;
	[SerializeField]
	int specialDogCount;
	[SerializeField]
	int maxNumberAdsPerDay;
	[SerializeField]
	int videoAdCoinBonus;
	[SerializeField]
	int costOfOneDogFoodCan;
	[SerializeField]
	int costOfFiveDogFoodCans;
	[SerializeField]
	int costOfTenDogFoodCans;
	[SerializeField]
	int costOfTwentyDogFoodCans;
	[SerializeField]
	int maxPuppyAge;
	[SerializeField]
	int minSeniorDogAge;
	[SerializeField]
	float timeScoutingToTimeRestingRatio;
	[SerializeField]
	float chanceOfDogsEatingPoop;
	[SerializeField]
	float minCouponDiscount;
	[SerializeField]
	float maxCouponDiscount;
	[SerializeField]
	float shelterResellCostScale;
	[SerializeField]
	float defaultChanceOfCollectingDogFood;
	[SerializeField]
	float defaultChanceOfCollectingMoney;
	[SerializeField]
	float puppyChanceOfRunningAwayDuringScouting;
	[SerializeField]
	float puppyScoutingTimeModifier;
	[SerializeField]
	float seniorDogScoutingTimeModifier;
	[SerializeField]
	int maxAmountPerTypeFromScouting;
	[SerializeField]
	float chanceOfRareObject;
	[SerializeField]
	float chanceSpecialItemIsPresent;
	[SerializeField]
	float chanceSpecialItemIsDog;
	[SerializeField]
	float chanceSpecialItemIsTubTub;

	#endregion

}