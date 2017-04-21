﻿/*
 * Author(s): Isaiah Mann, Ben Page
 * Description: Used for debugging and editing tools
 * Usage: [no notes]
 */

#if UNITY_EDITOR

using UnityEngine;

public class ToolController : SingletonController<ToolController>
{
	[SerializeField]
	KeyCode coinKey = KeyCode.Z;

	[SerializeField]
	int defaultCoinIncrease = 1000;

	[SerializeField]
	KeyCode foodKey = KeyCode.X;

	[SerializeField]
	int defaultFoodIncrease = 10;

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
	}

	void increaseCoins(int amount)
	{
		gameController.ChangeCoins(amount);
	}

	void increaseFood(int amount)
	{
        gameController.ChangeFood(amount, DogFoodType.Regular);
    }
}

#endif
