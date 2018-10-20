using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

namespace Prototype.NetworkLobby
{
    public class LobbyNetworkDiscovery : NetworkDiscovery
    {
        public override void OnReceivedBroadcast(string fromAddress, string data)
        {
            gameObject.GetComponentInChildren < LobbyMainMenu>().ipInput.text = fromAddress;
        }
    }
}