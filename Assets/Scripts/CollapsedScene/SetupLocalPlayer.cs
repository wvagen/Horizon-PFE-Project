using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SetupLocalPlayer : NetworkBehaviour
{
    [SyncVar]
    public string pname = "Player";

    [SyncVar]
    public string pRoleName = "MS";

    public Text playerNameTxt;

    void OnGUI()
    {
        if (isLocalPlayer)
        {
            CmdChangeName(pname);
        }
    }

    [Command]
    public void CmdChangeName(string newName)
    {
        playerNameTxt.text = newName;
    }

}
