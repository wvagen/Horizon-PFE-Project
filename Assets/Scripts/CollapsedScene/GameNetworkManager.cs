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

    public SetupLocalPlayer localPlayer;

    DoughManager doughMan;
    CashierManager cashMan;
    

    void Start()
    {
        if (isLocalPlayer){
            switch (localPlayer.pRoleName)
            {
                case "DS": doughMan = GetComponentInChildren<DoughManager>(); break;
                case "CS": cashMan = GetComponentInChildren<CashierManager>();Debug.Log ("NIGGER YES") ; break;
            }
        }
        
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
        Debug.Log(isLocalPlayer);
        if (localPlayer.pRoleName == "DS")
        {
            doughMan.GenerateNewRequirmentMenu();
            DoughManager.clientTimer = timeValue;
        }
    }


}
