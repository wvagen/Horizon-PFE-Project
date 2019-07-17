using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameNetworkManager : NetworkBehaviour
{
    
    //Shared variables

    [SyncVar]
    public float timeValue = 10 ;

    //End of shared variables


    public void GenerateNewCake()
    {
        CmdGenerateNewCake();
    }

    [Command]
    public void CmdGenerateNewCake()
    {
        if (!isServer) return;
        RpcGenerateNewCake();
    }

    [ClientRpc]
    void RpcGenerateNewCake()
    {
        GameObject localPlayer = FindLocalNetworkPlayer();
        if (localPlayer.GetComponentInChildren<SetupLocalPlayer>().pRoleName == "CMS")
        {
            localPlayer.GetComponentInChildren<CakeMakerManager>().GenerateRandomCakeCode();
            localPlayer.GetComponentInChildren<CakeMakerManager>().GenerateCakePreview();
        }
    }

   public void GenerateNewOrder()
   {
       CmdGenerateNewOrder();
   }

    [Command]
   public void CmdGenerateNewOrder()
    {
        if (!isServer) return;
        RpcGenerateNewOrder();
    }

    [ClientRpc]
    void RpcGenerateNewOrder()
    {
        GameObject localPlayer = FindLocalNetworkPlayer();
        if (localPlayer.GetComponentInChildren<SetupLocalPlayer>().pRoleName == "DS")
        {
            localPlayer.GetComponentInChildren<DoughManager>().GenerateNewRequirmentMenu();
            //DoughManager.clientTimer = timeValue;
        }
        else
        {
            if (localPlayer.GetComponentInChildren<SetupLocalPlayer>().pRoleName == "CMS")
            {
                localPlayer.GetComponentInChildren<CakeMakerManager>().GenerateRandomCakeCode();
                localPlayer.GetComponentInChildren<CakeMakerManager>().GenerateCakePreview();
            }
        }
    }

    GameObject FindLocalNetworkPlayer()
    {
        NetworkManager networkManager = NetworkManager.singleton;
        List<PlayerController> pc = networkManager.client.connection.playerControllers;

        for (int i = 0; i < pc.Count; i++)
        {
            GameObject obj = pc[i].gameObject;
            NetworkBehaviour netBev = obj.GetComponent<NetworkBehaviour>();

            if (pc[i].IsValid && netBev != null && netBev.isLocalPlayer)
            {
                return pc[i].gameObject;
            }
        }
        return null;
    }


}
