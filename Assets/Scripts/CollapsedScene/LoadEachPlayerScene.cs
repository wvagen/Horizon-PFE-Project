using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using Prototype.NetworkLobby;

public class LoadEachPlayerScene : NetworkBehaviour
{
    public GameObject MapScene;
    public GameObject CakeMakerScene;
    public GameObject DoughScene;
    public GameObject cashierScene;

    SetupLocalPlayer localPlayer;
    static string[] Roles = new string[] { "DS", "MS", "CMS" , "CS"};

    GameObject loadedScene;
    void Start()
    {

        if (isLocalPlayer)
        {
            localPlayer = GetComponent<SetupLocalPlayer>();

            switch (localPlayer.pRoleName)
            {
                case "MS": loadedScene = Instantiate(MapScene, Vector2.zero, Quaternion.identity,transform); break;
                case "CMS": loadedScene = Instantiate(CakeMakerScene, Vector2.zero, Quaternion.identity,transform); break;
                case "DS": loadedScene = Instantiate(DoughScene, Vector2.zero, Quaternion.identity,transform); break;
                case "CS": loadedScene = Instantiate(cashierScene, Vector2.zero, Quaternion.identity,transform);
                    loadedScene.GetComponentInChildren<CashierManager>().network = GetComponent<GameNetworkManager>();
                    break;
                default: Debug.Log("Scene not registred"); break;
            }
            localPlayer.playerNameTxt.enabled = true;
            localPlayer.playerNameTxt.text = localPlayer.pRoleName;
            loadedScene.GetComponentInChildren<Canvas>().worldCamera = Camera.main;

            NetworkServer.Spawn(loadedScene);
            GetComponent<GameNetworkManager>().localPlayer = localPlayer;
        }
    }

}
