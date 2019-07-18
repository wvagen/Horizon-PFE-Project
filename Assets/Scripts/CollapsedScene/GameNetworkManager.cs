using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameNetworkManager : NetworkBehaviour
{

    public void CakeOnTable(string cakeCode)
    {
        CmdCakeOnTable(cakeCode);
    }

    [Command]
    public void CmdCakeOnTable(string cakeCode)
    {
        if (!isServer) return;
        RpcCakeOnTable(cakeCode);
    }

    [ClientRpc]
    void RpcCakeOnTable(string cakeCode)
    {
        GameObject localPlayer = FindLocalNetworkPlayer();
        if (localPlayer.GetComponentInChildren<SetupLocalPlayer>().pRoleName == "CS")
        {
            localPlayer.GetComponentInChildren<CashierManager>().FetchReadyRecipiesAndPutOnTable(cakeCode);
        }
    }

    public void GenerateNewCake(float remaningTime, string cakeCode)
    {
        CmdGenerateNewCake(remaningTime, cakeCode);
    }

    [Command]
    public void CmdGenerateNewCake(float remaningTime,string cakeCode)
    {
        if (!isServer) return;
        RpcGenerateNewCake(remaningTime , cakeCode);
    }

    [ClientRpc]
    void RpcGenerateNewCake(float remaningTime,string cakeCode)
    {
        GameObject localPlayer = FindLocalNetworkPlayer();
        if (localPlayer.GetComponentInChildren<SetupLocalPlayer>().pRoleName == "CMS")
        {
            localPlayer.GetComponentInChildren<CakeMakerManager>().generatedcakeCode = cakeCode;
            localPlayer.GetComponentInChildren<CakeMakerManager>().patienceTime = remaningTime;
            localPlayer.GetComponentInChildren<CakeMakerManager>().GenerateCakePreview();
          //  localPlayer.GetComponentInChildren<CakeMakerManager>().GenerateButtons();
            
        }
    }

    public void GenerateNewOrder(float remainingTime, string cakeCode)
   {
       CmdGenerateNewOrder(remainingTime, cakeCode);
   }

    [Command]
    public void CmdGenerateNewOrder(float remainingTime, string cakeCode)
    {
        if (!isServer) return;
        RpcGenerateNewOrder(remainingTime, cakeCode);
    }

    [ClientRpc]
    void RpcGenerateNewOrder(float remainingTime, string cakeCode)
    {
        GameObject localPlayer = FindLocalNetworkPlayer();
        if (localPlayer.GetComponentInChildren<SetupLocalPlayer>().pRoleName == "DS")
        {
            localPlayer.GetComponentInChildren<DoughManager>().clientTimer = remainingTime;
            localPlayer.GetComponentInChildren<DoughManager>().GenerateNewRequirmentMenu(cakeCode);
           
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
