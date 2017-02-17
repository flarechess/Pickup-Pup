﻿/*
 * Authors: Isaiah Mann, Grace Barrett-Snyder
 * Description: Controls a UI
 */

using UnityEngine;
using k = PPGlobal;

public class PPUIController : MonoBehaviourExtended 
{
    protected PPSceneController sceneController;
    protected PPGameController gameController;
	protected PPGiftController giftController;

    protected DogProfile dogProfile;
    protected Dog selectedDog;
    protected PromptID promptID;

    [SerializeField]
    GameObject dogProfileObject;
    [SerializeField]
    CurrencyPanel currencyPanel;
    [SerializeField]
    NavigationPanel navigationPanel;
    [SerializeField]
    PopupPrompt popupPrompt;

    #region MonoBehaviourExtended Overrides

    protected override void setReferences()
    {
        base.setReferences();
        if (dogProfileObject != null)
        {
            dogProfileObject.SetActive(false);
        }
        if(popupPrompt)
        {
            showPopupPrompt();
        }
    }

    protected override void fetchReferences() 
	{
		base.fetchReferences();
		sceneController = PPSceneController.Instance;
		gameController = PPGameController.GetInstance;
		dataController = PPDataController.GetInstance;
		giftController = PPGiftController.Instance;
        setCurrencyPanel();
    }

	protected override void subscribeEvents()
	{
		base.subscribeEvents();
		EventController.Subscribe(handlePPDogEvent);
	}

	protected override void unsubscribeEvents()
	{
		base.unsubscribeEvents();
		EventController.Unsubscribe(handlePPDogEvent);
	}

	#endregion

    public void LoadShelter()
    {
        setNavigationPanel(false);
        sceneController.LoadShelter();
    }

    public void LoadShop()
    {
        sceneController.LoadShop();
    }

	public void LoadLivingRoom()
	{
		sceneController.LoadLivingRoom();
	}

    public void LoadYard()
    {
        sceneController.LoadYard();
    }

	void handlePPDogEvent(PPEvent gameEvent, Dog dog)
	{
		if(gameEvent == PPEvent.ClickDogSlot)
		{
            selectedDog = dog;
			handleDogSlotClicked(selectedDog);
		}
	}

	void handleDogSlotClicked(Dog dog)
	{
        // TODO: Insert universal dog slot handle code here
        if (dogProfileObject)
        {
        	showDogProfile(dog);
        }
	}

    protected virtual void showDogProfile(Dog dog)
    {
        EventController.Event(k.GetPlayEvent(k.MENU_POPUP));
        dogProfileObject.SetActive(true);
        if(!dogProfile)
        {
            dogProfile = dogProfileObject.GetComponent<DogProfile>();
        }
        dogProfile.SetProfile(dog);
    }

    protected virtual void showPopupPrompt()
    {
        PopupPrompt prompt = (PopupPrompt) Instantiate(popupPrompt);
        prompt.Set(promptID);
    }

    void setCurrencyPanel()
    {
        if (currencyPanel)
        {
			currencyPanel.Init(gameController, dataController, giftController);
        }
    }

    void setNavigationPanel(bool showAdoptButton)
    {
        if (navigationPanel)
        {
            navigationPanel.showAdoptButton(showAdoptButton);
        }
    }

}
