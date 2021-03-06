﻿/*
 * Author(s): Isaiah Mann
 * Description: Used for debugging and editing tools
 * Usage: [no notes]
 */

#if UNITY_EDITOR

using UnityEngine;

using k = PPGlobal;

public class ToolController : SingletonController<ToolController>
{
    [Header("Fast Time Cheat")]
    [SerializeField]
    KeyCode toggleFastSpeedKey = KeyCode.C;

    [SerializeField]
    float increaseTimeScale = 100;
    bool fastSpeed = false;

    [Header("Coin Cheat")]
	[SerializeField]
	KeyCode coinKey = KeyCode.Z;

	[SerializeField]
	int defaultCoinIncrease = 1000;

    [Header("Food Cheat")]
	[SerializeField]
	KeyCode foodKey = KeyCode.X;

	[SerializeField]
	int defaultFoodIncrease = 10;

	[Header("Adopt All Cheat")]
	[SerializeField]
	KeyCode adoptAllKey = KeyCode.V;

	[Header("Max Affection Cheat")]
	[SerializeField]
	KeyCode maxAffectionKey = KeyCode.B;

	void Update()
	{
		if(Input.GetKeyDown(coinKey))		
		{
			increaseCoins(defaultCoinIncrease);
		}
		// Unrelated if statement so the developer can choose set both keys to the same (overloaded functionality)
		if(Input.GetKeyDown(foodKey))
		{
			increaseFood(defaultFoodIncrease);
		}
        if(Input.GetKeyDown(toggleFastSpeedKey))
        {
            toggleFastSpeed();
        }
		if(Input.GetKeyDown(adoptAllKey))
		{
			adoptAll();
		}
		if(Input.GetKeyDown(maxAffectionKey))
		{
			maxAffection();
		}
	}

	void increaseCoins(int amount)
	{
		gameController.ChangeCoins(amount);
	}

	void increaseFood(int amount)
	{
		gameController.ChangeFood(amount);
	}

    void toggleFastSpeed()
    {
        fastSpeed = !fastSpeed;
        gameController.ChangeTimeScale(this, fastSpeed ? increaseTimeScale : k.DEFAULT_TIME_SCALE);
    }

	void adoptAll()
	{
		DogDescriptor[] allDogs = DogDatabase.GetInstance.Dogs;
		for(int i = 0; i < allDogs.Length; i++)
		{
			if(!dataController.CheckIsAdopted(allDogs[i]))
			{
				dataController.Adopt(allDogs[i]);
			}
		}
	}

	void maxAffection()
	{
		foreach(DogDescriptor dog in dataController.AdoptedDogs)
		{
			dog.MaxAffection();
		}
	}

}

#endif
