﻿/*
 * Author(s): Isaiah Mann
 * Description: Determines what kind of gift the player receives from dogs scouting
 * Usage: [no notes]
 */

using UnityEngine;
using k = PPGlobal;

public class PPGiftController : SingletonController<PPGiftController>
{	
	const float DEFAULT_DISCOUNT = k.DEFAULT_DISCOUNT_DECIMAL;

	PPTuning tuning;

	WeightedRandomBuffer<CurrencyType> defaultReturnChances;
	WeightedRandomBuffer<CurrencyData> giftChances;

    CurrencyFactory giftFactory;

    #if UNITY_EDITOR

    [Header("Cheats")]
    [SerializeField]
    bool alwaysReturnSouvenirFirst = false;
	[SerializeField]
	bool alwaysReturnSpecialGift = false;

    #endif

	public void Init(PPTuning tuning)
	{
		this.tuning = tuning;
		defaultReturnChances = new WeightedRandomBuffer<CurrencyType>(
			new CurrencyType[]
			{
				CurrencyType.Coins, 
				CurrencyType.DogFood,
			},
			new float[]
			{
				tuning.DefaultChanceOfCollectingMoney,
				tuning.DefaultChanceOfCollectingDogFood,
			}
        );
		giftChances = populateGifts(
			tuning.DailyGiftOptions,
			tuning.DailyGiftAmounts,
			tuning.DailyGiftWeights,
			tuning.DailyGiftDiscountAmount);
        giftFactory = new CurrencyFactory();
	}

	public CurrencyData GetGiftFromDog(DogDescriptor dog)
	{
        CurrencyData gift;
        if(tryGetExistingGift(dog, out gift))
        {
            return gift;
        }
        else
        {
            return generateGift(dog);
        }
	}

    bool tryGetExistingGift(DogDescriptor info, out CurrencyData gift)
    {
        if(info.IsLinkedToDog)
        {
            Dog dog = info.PeekDogLink;
            if(dog.HasRedeemableGift)
            {
                gift = dog.PeekAtGift;
                return true;
            }
            else 
            {
                gift = null;
                return false;
            }
        }
        else
        {
            gift = null;
            return false;
        }
    }

    CurrencyData generateGift(DogDescriptor dog)
    {
		
    #if UNITY_EDITOR
        
		// CHEAT: Returns the souvenir first for testing purposes:
        if(alwaysReturnSouvenirFirst && !dog.SouvenirCollected)
        {
			return dog.Souvenir;
        }
		else if(alwaysReturnSpecialGift)
		{
			return getSpecialGift(dog);
		}

    #endif

		DogFoodData food = dog.DigestFood();
		if(food == null)
		{
			food = dataController.DogFood;
		}
        CurrencyType specialization = dog.Breed.ISpecialization;
        int amount = randomAmount();
        CurrencyType type;
        if(specialization == CurrencyType.None)
        {
			type = defaultRandomType(dog, food);
        }
        else
        {
            type = getRandomizerBySpecialization(dog, food, specialization).GetRandom();
        }
        if(type == CurrencyType.SpecialGift)
        {
            return getSpecialGift(dog);
		}
        else
        {
			CurrencyData gift = giftFactory.Create(type, amount);
			if(food.AmountMod != k.NONE_VALUE)
			{
				gift.MultiplyBy(food.AmountMod);
			}
			return gift;
        }
    }

    bool eligibleForSouvenir(DogDescriptor dog)
    {
        return !(dog.SouvenirCollected || dog.FirstTimeScouting);
    }

    CurrencyData getSpecialGift(DogDescriptor dog)
    {
		CurrencyData specialGift = getDogSpecialGiftChances(dog).GetRandom(); 
		if(specialGift is SouvenirData)
		{
			// Need to return the specific souvenir the dog owns
			return dog.Souvenir;
		}
		else
		{
			SpecialGiftData gift = specialGift as SpecialGiftData;
			(gift as SpecialGiftData).SetFinder(dog);
			if(!checkSpecialGiftCanBeUsed(gift))
			{
				// Set to a default currency if the SpecialGift cannot be used
			 	specialGift = giftFactory.Create(defaultReturnChances.GetRandom(), randomAmount());
			}
			return specialGift;
		}
	}

	bool checkSpecialGiftCanBeUsed(SpecialGiftData specialGift)
	{
		bool invalidUse = specialGift is GiftEventData && !GiftDatabase.GetInstance.TryUseEvent(specialGift as GiftEventData);
		return !invalidUse;
	}

	public CurrencyData GetDailyGift()
	{
		return giftChances.GetRandom();
	}

	WeightedRandomBuffer<CurrencyType> getRandomizerBySpecialization(DogDescriptor dog, DogFoodData food, CurrencyType specialization)
	{
		CurrencyType[] currencies = new CurrencyType[]
        {
            specialization,
            getSecondary(specialization),
            CurrencyType.SpecialGift,
        };
		float[] weights = getWeights(food, hasSpecialization:true);
		return new WeightedRandomBuffer<CurrencyType>(currencies, weights);
	}

	float[] getWeights(DogFoodData dogFood, bool hasSpecialization)
	{
		float difference;
		float halfDifference;
		float specialGiftChance;
		if(dogFood.SpecialGiftMod != k.NONE_VALUE)
		{
			difference = dogFood.SpecialGiftMod - tuning.ChanceOfSpecialGift;
			specialGiftChance = dogFood.SpecialGiftMod;
		}
		else
		{
			difference = k.NONE_VALUE;
			specialGiftChance = tuning.ChanceOfSpecialGift;
		}
		halfDifference = difference / 2f;
		float[] weights;
		if(hasSpecialization)
		{
			weights = new float[]
			{
				tuning.ChanceOfSpecialization - halfDifference,
				tuning.ChanceOfSecondary - halfDifference,
				specialGiftChance,
			};
		}
		else
		{
			weights = new float[]
			{
				tuning.DefaultChanceOfCollectingDogFood - halfDifference,
				tuning.DefaultChanceOfCollectingMoney - halfDifference,
				specialGiftChance,
			};
		}
		return weights;
	}

	WeightedRandomBuffer<CurrencyData> populateGifts(
		string[] giftTypes, 
		int[] giftAmounts, 
		float[] giftChances,
		float discountPercent)
	{
		ParallelArray<string, int> giftData = new ParallelArray<string, int>(giftTypes, giftAmounts);
		CurrencyFactory giftFactory = new CurrencyFactory();
		CurrencyData[] gifts = giftFactory.CreateGroup(giftData, discountPercent);
		return new WeightedRandomBuffer<CurrencyData>(gifts, giftChances);
	}

	WeightedRandomBuffer<CurrencyData> getDogSpecialGiftChances(DogDescriptor dog)
	{
		float affectionFraction;
		/*
		 * Affection determines whether dog can return souvenir
		 * IF dog already hsa souvenir, assume affection is 0 
		 * (no chance of returning souvenir)
		 */
		if(eligibleForSouvenir(dog))
		{
			affectionFraction = dog.FractionOfMaxAffection;
		}
		else
		{
			affectionFraction = k.NONE_VALUE;
		}
		CurrencyData[] specialGiftOptions = giftFactory.CreateGroup(tuning.SpecialGiftTypes);
		float[] specialGiftOdds = new float[specialGiftOptions.Length];
		for(int i = 0; i < specialGiftOdds.Length; i++)
		{
			specialGiftOdds[i] = Mathf.Lerp(
				tuning.SpecialGiftMinAffectionChances[i],
				tuning.SpecialGiftMaxAffectionChances[i],
				affectionFraction);
			// Check to prevent a DogVoucher from being given out if all dogs have already been adopted:
			if(specialGiftOptions[i] is DogVoucherData && dataController.AllDogsAdopted(DogDatabase.GetInstance))
			{
				specialGiftOdds[i] = k.NONE_VALUE;
			}
		}
		return new WeightedRandomBuffer<CurrencyData>(specialGiftOptions, specialGiftOdds);
	}

	CurrencyType defaultRandomType(DogDescriptor dog, DogFoodData food)
	{
		return new WeightedRandomBuffer<CurrencyType>(
			new CurrencyType[]
			{
				CurrencyType.Coins, 
				CurrencyType.DogFood,
				CurrencyType.SpecialGift
			},
			getWeights(food, hasSpecialization:false)).GetRandom();
	}

	int randomAmount()
	{
		int zeroOffset = 1;
		return Random.Range(zeroOffset, tuning.MaxAmountPerTypeFromScouting + zeroOffset);
	}

	CurrencyType getSecondary(CurrencyType specialization)
	{
		switch(specialization)
		{
			case CurrencyType.Coins:
				return CurrencyType.DogFood;
			case CurrencyType.DogFood:
				return CurrencyType.Coins;
			default:
				return CurrencyType.None;
		}
	}

}
