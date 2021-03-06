﻿/*
 * Author: Grace Barrett-Snyder 
 * Description: Controls navigation between Dog Profiles
 */

using System.Collections.Generic;
using UnityEngine;

public class DogProfileButtonController : PPUIButtonController
{
    #region Instance Accessors

    public bool IsInitialized
    {
        get;
        private set;
    }

	public DogDescriptor SelectedDogInfo
    {
		get
        {
			return SelectedDog.Info;
		}
	}

    public Dog SelectedDog
    {
        get
        {
            return dogsList[currentProfileIndex];
        }
    }

	public int SelectedIndex
    {
		get
        {
			return currentProfileIndex;
		}
	}

    #endregion

    [SerializeField]
    UIButton pageBackwardButton;
    [SerializeField]
    UIButton pageForwardButton;

    DogProfile parentWindow;
    ToggleableColorUIButton[] pageButtons;
    ToggleableColorUIButton selectedPageButton;

    List<Dog> dogsList;
    int currentProfileIndex;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        base.setReferences();
        // GetComponentInParent also checks the current GameObject (as per the current prefab)
        parentWindow = GetComponentInParent<DogProfile>();
    }

    #endregion

    public void Init(DogProfile profile, DogDescriptor[] dogs)
    {
        this.parentWindow = profile;
        IsInitialized = true;

        updateDogList(dogs);

        pageBackwardButton.SubscribeToClick(previousProfile);
        pageForwardButton.SubscribeToClick(nextProfile);
        if (dogsList.Count == 1)
        {
            pageBackwardButton.Hide();
            pageForwardButton.Hide();
        }
    }

    void updateDogList(DogDescriptor[] dogInfos) {
		DogFactory dogFactory = new DogFactory(hideGameObjects: true);
		dogsList = dogFactory.CreateGroupList(new List<DogDescriptor>(dogInfos));
	}

    void switchToProfile(int index)
    {
        checkReferences();
        currentProfileIndex = index;
        checkCurrentIndex();
        parentWindow.SetProfile(dogsList[currentProfileIndex]);
    }

    void nextProfile()
    {
        switchToProfile(currentProfileIndex + 1);
    }

    void previousProfile()
    {
        switchToProfile(currentProfileIndex - 1);
    }

    bool checkCurrentIndex()
    {
        if(currentProfileIndex < 0 || currentProfileIndex >= dogsList.Count)
        {
            fixCurrentIndex();
            return false;
        }
        return true;
    }

    void fixCurrentIndex()
    {
        if(currentProfileIndex < 0)
        {
            currentProfileIndex = dogsList.Count - 1;
        }
        else if(currentProfileIndex >= dogsList.Count)
        {
            currentProfileIndex = 0;
        }
    }

	public void CalibrateIndex(Dog dog) {
		for(int i = 0; i < dogsList.Count; i++) {
			var _dog = dogsList [i];
			if (_dog.Info == dog.Info) {
				currentProfileIndex = i;
				break;
			}
		}
	}
}
