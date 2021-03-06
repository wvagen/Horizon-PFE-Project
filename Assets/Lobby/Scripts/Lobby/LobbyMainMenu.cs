using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Prototype.NetworkLobby
{
    //Main menu, mainly only a bunch of callback called by the UI (setup throught the Inspector)
    public class LobbyMainMenu : MonoBehaviour 
    {
        public LobbyManager lobbyManager;

        public Animator otherCanvasAnimator;

        public RectTransform lobbyServerList;
        public RectTransform lobbyPanel;

        public InputField ipInput;


        public void OnEnable()
        {
            lobbyManager.topPanel.ToggleVisibility(true);

            ipInput.onEndEdit.RemoveAllListeners();
            ipInput.onEndEdit.AddListener(onEndEditIP);
        }


        public void OnClickHost()
        {
            if (!MainMenuManager.isMultiplayerButtonClicked) return;
            MainMenuManager.isPlayerConnected = true;
            otherCanvasAnimator.Play("Host_Join");
            lobbyManager.StartHost();
        }

        public void OnClickJoin()
        {
            if (!MainMenuManager.isMultiplayerButtonClicked) return;
            MainMenuManager.isPlayerConnected = true;
            otherCanvasAnimator.Play("Host_Join");

            lobbyManager.ChangeTo(lobbyPanel);

            if (ipInput.Equals(string.Empty)) ipInput.text = "localhost";
            lobbyManager.networkAddress = ipInput.text;
            lobbyManager.StartClient();

            lobbyManager.backDelegate = lobbyManager.StopClientClbk;
            lobbyManager.DisplayIsConnecting();

            lobbyManager.SetServerInfo("Connecting...", lobbyManager.networkAddress);
        }

        public void OnClickOpenServerList()
        {
            lobbyManager.StartMatchMaker();
            lobbyManager.backDelegate = lobbyManager.SimpleBackClbk;
            lobbyManager.ChangeTo(lobbyServerList);
        }

        void onEndEditIP(string text)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                OnClickJoin();
            }
        }

    }
}
