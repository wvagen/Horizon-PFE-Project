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


    SetupLocalPlayer localPlayer;
    static string[] Roles = new string[] { "DS", "MS", "CMS" };

    void Start()
    {

        if (isLocalPlayer)
        {
            localPlayer = GetComponent<SetupLocalPlayer>();

            switch (localPlayer.pRoleName)
            {
                case "MS": Instantiate(MapScene, Vector2.zero, Quaternion.identity); break;
                case "CMS": Instantiate(CakeMakerScene, Vector2.zero, Quaternion.identity); break;
                case "DS": Instantiate(DoughScene, Vector2.zero, Quaternion.identity); break;
                default: Debug.Log("Scene not registred"); break;
            }

        }
    }

}
