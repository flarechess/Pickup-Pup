﻿/*
 * Author(s): Isaiah Mann
 * Description: Displays a single dog's portrait and world sprites
 * Usage: [no notes]
 */

using UnityEngine;
using UnityEngine.UI;

using k = PPGlobal;

public class DogSpriteDisplayTool : MonoBehaviourExtended 
{	
    #region Instance Accessors

    public DogDescriptor Dog
    {
        get;
        private set;
    }

    #endregion

    [SerializeField]
    Text nameDisplay;

    [SerializeField]
    Text breedDisplay;

    [SerializeField]
    Text colorDisplay;

    [SerializeField]
    Image dogProfilePortrait;

    [SerializeField]
    Image dogWorldSprite;

    public void Display(DogDescriptor dog)
    {
        this.Dog = dog;
        this.nameDisplay.text = string.Format("{0}: {1}", k.NAME, dog.Name);
        this.breedDisplay.text = string.Format("{0}: {1}", k.BREED, dog.Breed.Breed);
        this.colorDisplay.text = string.Format("{0}: {1}", k.Color, dog.Color);
        this.dogProfilePortrait.sprite = dog.Portrait;
        this.dogWorldSprite.sprite = dog.WorldSprite;
    }

    public bool CheckPortrait()
    {
        return checkSprite(dogProfilePortrait.sprite);
    }

    public bool CheckWorld()
    {
        return checkSprite(dogWorldSprite.sprite);
    }

    private bool checkSprite(Sprite sprite)
    {
        return !(sprite == null || sprite == DogDatabase.DefaultSprite);
    }

}
