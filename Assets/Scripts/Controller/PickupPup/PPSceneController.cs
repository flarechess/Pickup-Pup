﻿/*
 * Author: Isaiah Mann, Grace Barrett-Snyder
 * Description: Handles scene loading and management
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class PPSceneController : SingletonController<PPSceneController> 
{
	#region Instance Accessors

	public PPScene CurrentScene 
	{
		get 
		{
			return (PPScene) SceneManager.GetActiveScene().buildIndex;
		}
	}

	#endregion
    bool readyToSwitchScenes
    {
        get
        {
            return sceneLoadingBlockers == NONE_VALUE;        
        }
    }
        
    // Whether blocks should be cleared ever time the controller loads a new scene
    [SerializeField]
    bool shouldZeroOutSceneLoadingBlockersOnLoadScene = true;

    PPDataController dataController;
    // Set this to invalid until the script is fully loaded
    int sceneLoadingBlockers = INVALID_VALUE;

	#region MonoBehaviourExtended

	protected override void fetchReferences()
	{
		base.fetchReferences();
		dataController = PPDataController.GetInstance;
        // Now that refs are fetched, scenes can be loaded (need to be able to save before loading)
        zeroOutSceneLoadingBlockers();
	}

	#endregion

    public void LoadShelter()
    {
        LoadScene(PPScene.Shelter);
    }

    public void LoadShop()
    {
        LoadScene(PPScene.Shop);
    }

    public void LoadLivingRoom()
    {
        LoadScene(PPScene.LivingRoom);
    }

    public void LoadYard()
    {
        LoadScene(PPScene.Yard);
    }

    public void LoadScene(PPScene scene) 
	{
		dataController.SaveGame();
		SceneManager.LoadScene((int) scene);
        if(shouldZeroOutSceneLoadingBlockersOnLoadScene)
        {
            zeroOutSceneLoadingBlockers();
        }
	}

    // TODO: Implement an async version of this that stores a queue of scene loading requests 
    // TODO: Implement an async version to loan scenes additively
    public bool RequestLoadScene(PPScene scene)
    {
        if(canLoadScene(scene)) 
        {
            LoadScene(scene);
            return true;
        }
        else 
        {
            return false;
        }
    }
        
    public bool RequestReloadCurrentScene()
    {
        return RequestLoadScene(CurrentScene);
    }

    public void BlockFromLoadingScenes()
    {
        sceneLoadingBlockers++;
    }

    public void UnblockFromLoadingScenes()
    {
        sceneLoadingBlockers--;
    }

    // Currently does not care about which scene, but may need more advanced logic in future
    bool canLoadScene(PPScene scene)
    {
        return readyToSwitchScenes;
    }

    void zeroOutSceneLoadingBlockers()
    {
        sceneLoadingBlockers = NONE_VALUE;
    }

}

public enum PPScene 
{
    Shelter,
    Shop,
    LivingRoom,
    Yard,

}
