﻿/*
 * Authors: Timothy Ng, Isaiah Mann, Grace Barrett-Snyder
 * Description: Game controller for Pickup Pup
 */

using System.IO;
using UnityEngine;
using System.Collections.Generic;
using k = PPGlobal;

public class PPGameController : GameController, ICurrencySystem 
{
	#region Static Accessors

	// Returns the Instance cast to the sublcass
	public static PPGameController GetInstance 
	{
		get 
		{
			return Instance as PPGameController;
		}
	}

	#endregion

	static string TUNING_FILE_PATH
	{
		get
		{
			return Path.Combine(k.JSON_DIR, k.TUNING);
		}
	}

	static string GAME_DATA_FILE_PATH 
	{
		get 
		{
			return Path.Combine(k.JSON_DIR, k.GAME_DATA);
		}
	}

    static string SHOP_FILE_PATH
    {
        get
        {
			return Path.Combine(k.JSON_DIR, k.SHOP_ITEMS);
        }
    }

	static string GIFT_FILE_PATH
    {
        get
        {
			return Path.Combine(k.JSON_DIR, k.GIFT_ITEMS);
        }
    }

    static string SAVE_FILE_PATH 
	{
		get 
		{
			return Path.Combine(Application.persistentDataPath, k.SAVE_FILE);
		}
	}

    public PPTuning Tuning
    {
        get
        {
            return tuning;
        }
    }

	#region Instance Accessors

	public DogDatabase DogData
	{
		get 
		{
			return dogDatabase; 
		}
	}

    public ShopDatabase Shop
    {
        get
        {
            return shop;
        }
    }

	public bool DogsScoutingAtCapacity
    {
        get
        {
			return dataController.ScoutingDogs.Count >= tuning.MaxDogsScouting;
        }
    }
		
	public GiftDatabase Gifts
    {
        get
        {
            return gifts;
        }
    }

    public LanguageDatabase Languages
    {
        get
        {
            return languages;
        }
    }
	
    #region ICurrencySystem Interface

    public CoinsData Coins
    {
        get
        {
            return dataController.Coins;
        }
    }

    public DogFoodData DogFood
    {
        get
        {
            return dataController.DogFood;
        }
    }

	public bool HasTargetSlot
	{
		get
		{
			return targetSlot != null;
		}
	}

	#endregion

    public bool MainMenuIsOpen
    {
        get
        {
            return mainMenuIsOpen;
        }
    }

    public float TimeScale
    {
        get
        {
            return this._timeScale;
        }
        set
        {
            this._timeScale = value;
        }
    }

    #endregion

    #region Controller Overrides

    protected override bool shouldReSetRefsOnReset 
    {
        get 
        {
            return false;
        }
    }

    #endregion

    // The dog the player currently has selected
	PPTuning tuning;
	DogDatabase dogDatabase;
    ShopDatabase shop;
	GiftDatabase gifts;
	LanguageDatabase languages;
	PPGiftController giftController;
	DogSlot targetSlot;
    bool mainMenuIsOpen = false;
    float _timeScale = k.DEFAULT_TIME_SCALE;

	#region MonoBehaviourExtended Overrides

	protected override void setReferences() 
	{
		base.setReferences();
        if(isSingleton)
        {
    		dogDatabase = parseDogDatabase();
            shop = parseShopDatabase();
    		gifts = parseGiftDatabase();
    		tuning = parseTuning();
            languages = initLanguages();
            shop.Initialize();
    		gifts.Initialize();
        }
	}

	protected override void fetchReferences() 
	{
        base.fetchReferences();
        if(isSingleton)
        {
            dogDatabase.Initialize(dataController);
            dataController.SetFilePath(SAVE_FILE_PATH);
    		dataController.LoadGame();
    		giftController = PPGiftController.Instance;
    		giftController.Init(tuning);
    		handleLoadGame(dataController);
        }
	}

    protected override void handleSceneLoaded(int sceneIndex)
    {
        if(isSingleton)
        {
            base.handleSceneLoaded(sceneIndex);
            if(dataController)
            {
                setupScoutingOnLoad(dataController);
            }
        }
    }

	protected override void handleGameTogglePause(bool isPaused)
	{
		base.handleGameTogglePause(isPaused);
		if(!isPaused && dataController)
		{
			dataController.LoadGame();
		}
	}

    #endregion

	void handleLoadGame(PPDataController dataController)
	{
		checkToAddStartingDogs(dataController);
		setupScoutingOnLoad(dataController);
	}

	bool checkToAddStartingDogs(PPDataController dataController)
	{
		if(shouldAddStartingDogs(dataController))
		{
			for(int i = 0; i < tuning.StartingDogCount; i++)
			{
                dataController.Adopt(dogDatabase.RandomDog(mustBeUnadopted:true));
			}
			return true;
		}
		else
		{
			return false;
		}
	}

	bool shouldAddStartingDogs(PPDataController dataController)
	{
        return dataController.ShouldGiveFreeDogs;
	}

	void setupScoutingOnLoad(PPDataController dataController)
	{
        List<DogDescriptor> dogs = dataController.ScoutingDogs;
		if(dogs != null && dogs.Count > 0)
		{
			Dog[] dogObjs = new DogFactory(hideGameObjects:true).CreateGroup(dogs.ToArray());
			callScoutingDogsLoaded(dogObjs);
		}
	}

	void callScoutingDogsLoaded(Dog[] dogs)
	{
		foreach(Dog dog in dogs)
		{
			dog.SetGame(this);
			dog.SetTimer(dog.Info.TimeRemainingScouting);
			dog.Info.HandleScoutingBegan(dog.Info.ScoutingSlotIndex);
			EventController.Event(PPEvent.ScoutingDogLoaded, dog);
		}
	}

	public int GetCurrentSlotIndex()
	{
		if(HasTargetSlot)
		{
			return targetSlot.GetIndex();	
		}
		else
		{
			return INVALID_VALUE;
		}
	}

    #region ICurrencySystem Interface

    public void ChangeTimeScale(object source, float newTimeScale)
    {
        if(source is Controller)
        {
            this.TimeScale = newTimeScale;
        }
    }

    public void ChangeCoins(int deltaCoins) 
	{
		dataController.ChangeCoins(deltaCoins);
	}

	public void ChangeFood(int deltaFood) 
	{
		dataController.ChangeFood(deltaFood);
	}

	public void ChangeCurrencyAmount(CurrencyData currency)
    {
		dataController.ChangeCurrencyAmount(currency);
    }

	public void ConvertCurrency(CurrencyData taken, CurrencyData given)
    {
		dataController.ConvertCurrency(taken, given);
    }

	public void SetTargetSlot(DogSlot slot)
	{
		this.targetSlot = slot;
	}
		
	public void GiveCurrency(CurrencyData currency)
	{
		dataController.GiveCurrency(currency);
	}

	public void SubscribeToCurrencyChange(CurrencyType type, MonoActionInt callback)
	{
		dataController.SubscribeToCurrencyChange(type, callback);
	}

	public void UnsubscribeFromCurrencyChange(CurrencyType type, MonoActionInt callback)
	{
		dataController.UnsubscribeFromCurrencyChange(type, callback);
	}

	bool ICurrencySystem.TryUnsubscribeAll()
	{
		(dataController as ICurrencySystem).TryUnsubscribeAll();
		return true;
	}

    public bool CanAfford(CurrencyType type, int amount)
    {
        return dataController.CanAfford(type, amount);
    }

    public bool HasCurrency(CurrencyType type)
    {
        return dataController.HasCurrency(type);
    }

	public bool TryTakeCurrency(CurrencyData currency)
	{
		return dataController.TryTakeCurrency(currency);
	}

	public bool HasFood(string foodType, int amount)
	{
		return dataController.HasFood(foodType, amount);
	}

    #endregion

    public void ToggleMainMenuOpen(bool menuIsOpen)
    {
        this.mainMenuIsOpen = menuIsOpen;
    }

	public CurrencyData GetGift(DogDescriptor dog)
	{
		CurrencyData data = giftController.GetGiftFromDog(dog);
		return data;
	}

	public bool TryBuyItem(ShopItem item, int amount)
    {
		if(CanAfford(item.CostCurrencyType, item.GetTotalCost(amount)))
        {   
			buyItem(item, amount);
            return true;
        }
        else 
        {
            EventController.Event(k.GetPlayEvent(k.EMPTY));
            return false;
        }
    }

	void buyItem(ShopItem item, int amount)
    {
        EventController.Event(k.GetPlayEvent(k.PURCHASE));
		ConvertCurrency(item.Cost, item.GetPurchase(amount));
    }

    public bool TryAdoptDog(DogDescriptor dog)
    {
		if(dataController.CheckIsAdopted(dog))
		{
			// Trying to adopt a dog that is already adopted
			return false;
		}
		else
		{
	        if(CanAfford(CurrencyType.Coins, dog.CostToAdopt))
	        {
	            AdoptDog(dog);
	            return true;
	        }        
	        return false;       
		}
    }

    void AdoptDog(DogDescriptor dog)
    {
        dataController.ChangeCoins(-dog.CostToAdopt);
        dataController.Adopt(dog);
    }

	public bool TrySendDogToScout(Dog dog, out int slotIndex)
	{
		// Can only send a certain number of dogs out to scout
		if(DogsScoutingAtCapacity)
		{
			slotIndex = INVALID_VALUE;
			return false;
		} 
		else 
		{
            if(targetSlot)
            {
                slotIndex = targetSlot.GetIndex();
            }
            else if(dog.Info.ScoutingSlotIndex != INVALID_VALUE)
            {
                slotIndex = dog.Info.ScoutingSlotIndex;
            }
            else if(ScoutingDisplay.MostRecentInstance.TryFindOpenSlot(out targetSlot))
            {
                slotIndex = targetSlot.GetIndex();
            }
            else 
            {
                slotIndex = INVALID_VALUE;
                return false;
            }
			sendDogToScout(dog);
			dataController.SendDogToScout(dog);
			return true;
		}
	}
        
	public void SendToTargetSlot(Dog dog)
	{
		if(HasTargetSlot)
		{
            targetSlot.Init(dog, inScoutingSelectMode:false);
			ClearTargetSlot();
		}
	}

	public void ClearTargetSlot()
	{
		this.targetSlot = null;
	}

	void sendDogToScout(Dog dog) 
	{
        EventController.Event(k.GetPlayEvent(k.DOG_SENDOUT));
		dog.SubscribeToScoutingTimerEnd(handleDogDoneScouting);
	}

	void handleDogDoneScouting(Dog dog) 
	{
		// Need to unsubscribe to prevent stacking even subscriptions if dog is sent to scout again:
		dog.UnsubscribeFromScoutingTimerEnd(handleDogDoneScouting);
	}

    public bool TryRedeemGift(int value, CurrencyType valueCurrencyType)
    {
        RedeemGift(value, valueCurrencyType);
        return true;
    }

	public bool TryRedeemGift(GiftItem gift)
    {
        return TryRedeemGift(gift.Value, gift.ValueType);
    }

	public void RedeemGift(int value, CurrencyType valueCurrencyType)
    {
		dataController.ChangeCurrencyAmount(new CurrencyFactory().Create(valueCurrencyType, value));
    }

	DogDatabase parseDogDatabase() 
	{
        return parseFromJSONInResources<DogDatabase>(GAME_DATA_FILE_PATH);
	}

    ShopDatabase parseShopDatabase()
    {
        return parseFromJSONInResources<ShopDatabase>(SHOP_FILE_PATH);
    }

	GiftDatabase parseGiftDatabase()
    {
        TextAsset json = loadTextAssetInResources(GIFT_FILE_PATH);
        return JsonUtility.FromJson<GiftDatabase>(json.text);
    }

	PPTuning parseTuning() 
	{
        return parseFromJSONInResources<PPTuning>(TUNING_FILE_PATH);
	}

    LanguageDatabase initLanguages()
    {
        LanguageDatabase languages = LanguageDatabase.Instance;
        languages.Initialize();
        return languages;
    }

}
