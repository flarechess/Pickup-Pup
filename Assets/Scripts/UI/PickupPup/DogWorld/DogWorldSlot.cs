﻿/*
 * Authors: Isaiah Mann, Ben Page, Grace Barrett-Snyder
 * Description: Dogs will travel / be displayed at this spot on the UI
 * Usage: [no notes]
 */

using UnityEngine.UI;
using UnityEngine;

public class DogWorldSlot : DogSlot
{
    NameTag nameTag;

    #region MonoBehaviourExtended Overrides 

    protected override void setReferences()
    {
        base.setReferences();
        dogImage = GetComponent<Image>();
        UISFXHandler sfxScript = GetComponent<UISFXHandler>();
        sfxScript.DisableSounds();
        GetComponent<Button>().transition = Selectable.Transition.None;
        nameTag = GetComponentInChildren<NameTag>();
        dogImage = GetComponentsInChildren<Image>()[0];
    }

    protected override void cleanupReferences()
    {
        base.cleanupReferences();
        if(dogInfo != null)
        {
            dogInfo.UnsubscribeFromBeginScouting(handleScoutingBegun);
            dogInfo.UnsubscribeFromDoneScouting(handleScoutingDone);
        }
    }

    #endregion

    #region DogSlot Overrides 

    public override void Init(DogDescriptor dog)
    {
        base.Init(dog);
        if(nameTag)
        {
            nameTag.Init(this, this.dog);
            nameTag.Hide();
        }
    }

    protected override void setSprite (DogDescriptor dog)
    {
        this.dogInfo = dog;
        dogImage.sprite = dog.WorldSprite;
        dog.SubscribeToBeginScouting(handleScoutingBegun);
        dog.SubscribeToDoneScouting(handleScoutingDone);
    }

    #endregion

    public void SubscribeToNameTagClick(PPData.DogAction clickAction)
    {
        if(nameTag)
        {
            nameTag.SubscribeToClick(clickAction);
        }
    }

    public void UnsubscribeFromNameTagClick(PPData.DogAction clickAction)
    {
        if(nameTag)
        {
            nameTag.UnsubscribeFromClick(clickAction);
        }
    }

    // Hide dog on scouting begun
    void handleScoutingBegun()
    {
        this.Hide();
    }

    void handleScoutingDone()
    {
        this.Show();
    }

}
