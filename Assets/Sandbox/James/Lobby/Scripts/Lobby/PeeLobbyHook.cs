using UnityEngine;
using UnityEngine.Networking;
using System.Collections;



namespace Prototype.NetworkLobby
{
    public class PeeLobbyHook : LobbyHook
    {
        public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer) {
            Splatter s = gamePlayer.GetComponent<Splatter>();
            Debug.Log("Lobby Hooked");
            //s.SetColors(lobbyPlayer.GetComponent<LobbyPlayer>().DogColorList);
        }
    }

}
