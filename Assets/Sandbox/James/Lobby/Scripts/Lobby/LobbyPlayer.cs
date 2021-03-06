using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Prototype.NetworkLobby
{
    //Player entry in the lobby. Handle selecting color/setting name & getting ready for the game
    //Any LobbyHook can then grab it and pass those value to the game player prefab (see the Pong Example in the Samples Scenes)
    public class LobbyPlayer : NetworkLobbyPlayer
    {
        static Vector4[] Colors = new Vector4[] { SplatColor.MAGENTA, SplatColor.INDIGO, SplatColor.CYAN, SplatColor.NEONYELLOW, SplatColor.NEONORANGE, SplatColor.NEONGREEN };
        //used on server to avoid assigning the same color to two player
        static List<int> _colorInUse = new List<int>();        
        public int connectionId;
		public Sprite dogTeamImage;
		public Sprite manTeamImage;
		public Button teamButton;
        public Button colorButton;
        public InputField nameInput;
        public Button readyButton;
        public Button waitingPlayerButton;
        public Button removePlayerButton;

        public GameObject localIcone;
        public GameObject remoteIcone;

        [SyncVar(hook = "OnMyName")]
        public string playerName = "";
        [SyncVar(hook = "OnMyColor")]
        public Vector4 playerColor = SplatColor.MAGENTA;
		[SyncVar(hook = "OnMyTeam")]
		public Teams.Team playerTeam = Teams.Team.DOGS;

        public Color OddRowColor = new Color(250.0f / 255.0f, 250.0f / 255.0f, 250.0f / 255.0f, 1.0f);
        public Color EvenRowColor = new Color(180.0f / 255.0f, 180.0f / 255.0f, 180.0f / 255.0f, 1.0f);

        static Color JoinColor = new Color(255.0f/255.0f, 0.0f, 101.0f/255.0f,1.0f);
        static Color NotReadyColor = new Color(34.0f / 255.0f, 44 / 255.0f, 55.0f / 255.0f, 1.0f);
        static Color ReadyColor = new Color(0.0f, 204.0f / 255.0f, 204.0f / 255.0f, 1.0f);
        static Color TransparentColor = new Color(0, 0, 0, 0);        

        public override void OnClientEnterLobby()
        {
            base.OnClientEnterLobby();

            if (LobbyManager.s_Singleton != null) LobbyManager.s_Singleton.OnPlayersNumberModified(1);

            LobbyPlayerList._instance.AddPlayer(this);
            LobbyPlayerList._instance.DisplayDirectServerWarning(isServer && LobbyManager.s_Singleton.matchMaker == null);

            if (isLocalPlayer)
            {
                SetupLocalPlayer();
            }
            else
            {
                SetupOtherPlayer();
            }

            //setup the player data on UI. The value are SyncVar so the player
            //will be created with the right value currently on server
            OnMyName(playerName);
            OnMyColor(playerColor);
			OnMyTeam (playerTeam);
        }

        public override void OnStartAuthority()
        {
            base.OnStartAuthority();

            //if we return from a game, color of text can still be the one for "Ready"
            readyButton.transform.GetChild(0).GetComponent<Text>().color = Color.white;

           SetupLocalPlayer();
        }

        void ChangeReadyButtonColor(Color c)
        {
            ColorBlock b = readyButton.colors;
            b.normalColor = c;
            b.pressedColor = c;
            b.highlightedColor = c;
            b.disabledColor = c;
            readyButton.colors = b;
        }

        void SetupOtherPlayer()
        {
            nameInput.interactable = false;
            removePlayerButton.interactable = NetworkServer.active;

            ChangeReadyButtonColor(NotReadyColor);

            readyButton.transform.GetChild(0).GetComponent<Text>().text = "...";
            readyButton.interactable = false;

            OnClientReady(false);
        }

        void SetupLocalPlayer()
        {
            nameInput.interactable = true;
            remoteIcone.gameObject.SetActive(false);
            localIcone.gameObject.SetActive(true);

            CheckRemoveButton();

            if (playerColor == SplatColor.MAGENTA)
                CmdColorChange();

            ChangeReadyButtonColor(JoinColor);

            readyButton.transform.GetChild(0).GetComponent<Text>().text = "JOIN";
            readyButton.interactable = true;

            //have to use child count of player prefab already setup as "this.slot" is not set yet
            if (playerName == "")
                CmdNameChanged("Player" + (LobbyPlayerList._instance.playerListContentTransform.childCount-1));

            //we switch from simple name display to name input
            colorButton.interactable = true;
            nameInput.interactable = true;
			teamButton.interactable = true;

            nameInput.onEndEdit.RemoveAllListeners();
            nameInput.onEndEdit.AddListener(OnNameChanged);

            colorButton.onClick.RemoveAllListeners();
            colorButton.onClick.AddListener(OnColorClicked);

			teamButton.onClick.RemoveAllListeners();
			teamButton.onClick.AddListener(OnTeamClicked);

            readyButton.onClick.RemoveAllListeners();
            readyButton.onClick.AddListener(OnReadyClicked);

            //when OnClientEnterLobby is called, the loval PlayerController is not yet created, so we need to redo that here to disable
            //the add button if we reach maxLocalPlayer. We pass 0, as it was already counted on OnClientEnterLobby
            if (LobbyManager.s_Singleton != null) LobbyManager.s_Singleton.OnPlayersNumberModified(0);
        }

        //This enable/disable the remove button depending on if that is the only local player or not
        public void CheckRemoveButton()
        {
            if (!isLocalPlayer)
                return;

            int localPlayerCount = 0;
            foreach (PlayerController p in ClientScene.localPlayers)
                localPlayerCount += (p == null || p.playerControllerId == -1) ? 0 : 1;

            removePlayerButton.interactable = localPlayerCount > 1;
        }

        public override void OnClientReady(bool readyState)
        {
            if (readyState)
            {
                ChangeReadyButtonColor(TransparentColor);

                Text textComponent = readyButton.transform.GetChild(0).GetComponent<Text>();
                textComponent.text = "READY";
                textComponent.color = ReadyColor;
                readyButton.interactable = false;
                colorButton.interactable = false;
                nameInput.interactable = false;
                teamButton.interactable = false;
            }
            else
            {
                ChangeReadyButtonColor(isLocalPlayer ? JoinColor : NotReadyColor);

                Text textComponent = readyButton.transform.GetChild(0).GetComponent<Text>();
                textComponent.text = isLocalPlayer ? "JOIN" : "...";
                textComponent.color = Color.white;
                readyButton.interactable = isLocalPlayer;
                colorButton.interactable = isLocalPlayer;
                nameInput.interactable = isLocalPlayer;
                teamButton.interactable = isLocalPlayer;
            }
        }

        public void OnPlayerListChanged(int idx)
        { 
            GetComponent<Image>().color = (idx % 2 == 0) ? EvenRowColor : OddRowColor;
        }

        ///===== callback from sync var

        public void OnMyName(string newName)
        {
            playerName = newName;
            nameInput.text = playerName;
        }

        public void OnMyColor(Vector4 newColor)
        {
            playerColor = newColor;
            colorButton.GetComponent<Image>().color = newColor;
			teamButton.GetComponent<Image> ().color = newColor;
        }

		public void OnMyTeam(Teams.Team newTeam)
		{            
            if (newTeam == Teams.Team.DOGS)
            {
                teamButton.GetComponent<Image>().sprite = dogTeamImage;
            }
            else
            {
                teamButton.GetComponent<Image>().sprite = manTeamImage;
            }
		}

        public void TeamChange()
        {
            if (playerTeam == Teams.Team.DOGS)
            {
                CmdTeamChange(Teams.Team.MAN);
            }
            else
            {
                CmdTeamChange(Teams.Team.DOGS);
            }
        }

        //===== UI Handler

        //Note that those handler use Command function, as we need to change the value on the server not locally
        //so that all client get the new value throught syncvar
        public void OnColorClicked()
        {
            CmdColorChange();
        }

		public void OnTeamClicked()
		{
			TeamChange();
            CmdColorChange();
		}

        public void OnReadyClicked()
        {
            SendReadyToBeginMessage();
        }

        public void OnNameChanged(string str)
        {
            CmdNameChanged(str);
        }

        public void OnRemovePlayerClick()
        {
            if (isLocalPlayer)
            {
                RemovePlayer();
            }
            else if (isServer)
                LobbyManager.s_Singleton.KickPlayer(connectionToClient);
                
        }

        public void ToggleJoinButton(bool enabled)
        {
            readyButton.gameObject.SetActive(enabled);
            waitingPlayerButton.gameObject.SetActive(!enabled);
        }

        [ClientRpc]
        public void RpcUpdateCountdown(int countdown)
        {
            LobbyManager.s_Singleton.countdownPanel.UIText.text = "Match Starting in " + countdown;
            LobbyManager.s_Singleton.countdownPanel.gameObject.SetActive(countdown != 0);
        }

        [ClientRpc]
        public void RpcUpdateRemoveButton()
        {
            CheckRemoveButton();
        }

        [ClientRpc]
        public void RpcUpdateTeam(Teams.Team newTeam)
        {
            playerTeam = newTeam;
            if (newTeam == Teams.Team.DOGS)
            {
                teamButton.GetComponent<Image>().sprite = dogTeamImage;
            }
            else
            {
                teamButton.GetComponent<Image>().sprite = manTeamImage;
            }
        }

        //====== Server Command

        [Command]
        public void CmdTeamChange(Teams.Team team)
        {
            string h;
            if (team == Teams.Team.DOGS) { h = "Dogs"; }
            else { h = "Man"; }
            Debug.Log("Selected Team is " + h);
            Debug.Log("Entering ManCount is " + LobbyManager.ManTeamCount);
            if (team == Teams.Team.MAN && LobbyManager.ManTeamCount < LobbyManager.ManTeamLimit)
            {
                LobbyManager.ManTeamCount++;
            }
            else if (team == Teams.Team.DOGS)
            {
                LobbyManager.ManTeamCount--;              
            }
            else
            {
                Debug.Log("Too many men.");
                return;
            } 
            playerTeam = team;
            LobbyManager.s_Singleton.SetTeamLobby(GetComponent<NetworkIdentity>().connectionToClient, team);
            RpcUpdateTeam(team);
        }

        [Command]
        public void CmdColorChange()
        {
            int idx = 0;
            if (playerTeam == Teams.Team.DOGS)
            {
                if(playerColor != SplatColor.WHITE){
                    idx = System.Array.IndexOf(Colors, playerColor);
                    _colorInUse.Remove(idx);
                }

                if (idx < 0) idx = 0;
                idx = (idx + 1) % Colors.Length;
                bool alreadyInUse = false;

                do
                {
                    alreadyInUse = false;
                    for (int i = 0; i < _colorInUse.Count; ++i)
                    {
                        if (_colorInUse[i] == idx)
                        {//that color is already in use
                            alreadyInUse = true;
                            idx = (idx + 1) % Colors.Length;
                        }
                    }
                }
                while (alreadyInUse);

                _colorInUse.Add(idx);
                playerColor = Colors[idx];
            }
            else if(playerTeam == Teams.Team.MAN)
            {
                if (playerColor != SplatColor.WHITE)
                {
                    idx = System.Array.IndexOf(Colors, playerColor);
                    _colorInUse.Remove(idx);
                }
                playerColor = SplatColor.WHITE;
            }
        }

        [Command]
        public void CmdNameChanged(string name)
        {
            playerName = name;
        }

        //Cleanup thing when get destroy (which happen when client kick or disconnect)
        public void OnDestroy()
        {
            LobbyPlayerList._instance.RemovePlayer(this);
            if (LobbyManager.s_Singleton != null) LobbyManager.s_Singleton.OnPlayersNumberModified(-1);

            int idx = System.Array.IndexOf(Colors, playerColor);

            if (idx < 0)
                return;

            for (int i = 0; i < _colorInUse.Count; ++i)
            {
                if (_colorInUse[i] == idx)
                {//that color is already in use
                    _colorInUse.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
