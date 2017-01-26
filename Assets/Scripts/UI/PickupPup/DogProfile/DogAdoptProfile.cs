﻿/*
 * Author: Grace Barrett-Snyder 
 * Description: UI for displaying an individual dog's profile in the Shelter scene
 */

using UnityEngine;
using UnityEngine.UI;

public class DogAdoptProfile : DogProfile
{
    [SerializeField]
    Text priceText;
    [SerializeField]
    Text adoptButtonText;
    [SerializeField]
    Button adoptButton;
    [SerializeField]
    UIElement costField;

    PPTuning tuning;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        base.setReferences();
        iconsObject.SetActive(false);
    }

    protected override void fetchReferences()
    {
        base.fetchReferences();
        tuning = gameController.Tuning;
    }

    #endregion

    #region DogProfile Overrides

    public override void SetProfile(Dog dog)
    {
        base.SetProfile(dog);
        checkReferences();
        if(checkAdopted(dogInfo))
        {
            showAdopted();
        }
        else
        {
            showDefault();
            setPriceText();
        }
    }

    #endregion

    bool checkAdopted(DogDescriptor dogInfo)
    {
        return PPDataController.GetInstance.CheckAdopted(dogInfo);
    }

    bool setPriceText()
    {
        priceText.text = dogInfo.CostToAdoptStr;

        if(!gameController.CanAfford(CurrencyType.Coins, dogInfo.CostToAdopt))
        {
            priceText.color = tuning.UnaffordableColorRGBA;
            return false;
        }
        priceText.color = tuning.AdoptedTextColorRGBA;
        return true;
    }

    void showAdopted()
    {
        setComponents(false, tuning.AdoptedBackgroundColorRGBA, tuning.AdoptedText, 
            tuning.AdoptedTextColorRGBA, false);
    }

    void showDefault()
    {
        setComponents(setPriceText(), tuning.AdoptedBackgroundColorRGBA, tuning.AdoptText,
            tuning.AdoptedTextColorRGBA, true);
    }

    void setComponents(bool adoptButtonInteractable, Color adoptButtonColor, 
        string adoptButtonTextString, Color adoptButtonTextColor, bool showCostField)
    {
        adoptButton.interactable = adoptButtonInteractable;
        adoptButton.image.color = adoptButtonColor;
        adoptButtonText.text = adoptButtonTextString;
        adoptButtonText.color = adoptButtonTextColor;
        if(showCostField)
        {
            costField.Show();
        }
        else
        {
            costField.Hide();
        }
    }

}