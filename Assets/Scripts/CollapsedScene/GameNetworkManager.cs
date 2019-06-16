using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameNetworkManager : NetworkBehaviour
{
    
    //Shared variables

    [SyncVar]
    public float timeValue ;

    //End of shared variables

    public SetupLocalPlayer localPlayer;
    DoughManager doughMan;

    

    void Start()
    {
        if (isLocalPlayer){
            switch (localPlayer.pRoleName)
            {
                case "DS": doughMan = GetComponentInChildren<DoughManager>(); break;
                
            }
        }
        if (isServer)
        {
            RpcGenerateNewOrder();
             InvokeRepeating("InvokeFunctions", 0, 3);
        }
    }

    void InvokeFunctions()
    {
        CmdGenerateNewOrder(10);
    }

    [Command]
    public void CmdGenerateNewOrder(float time)
    {
            timeValue = time;
            RpcGenerateNewOrder();
    }

    [ClientRpc]
    void RpcGenerateNewOrder()
    {
        Debug.Log(isLocalPlayer);
        if (localPlayer.pRoleName == "DS")
        {
            doughMan.GenerateNewRequirmentMenu();
            DoughManager.clientTimer = timeValue;
        }
    }


}
