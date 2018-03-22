using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Match {

    public class Manager : NetworkManager {

		void Start(){
			StartHost ();
		}

		public override void OnStartServer ()
		{
			base.OnStartServer ();
			HealthManager.Initialize ();
		}

		public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId)
		{
			GameObject g = Instantiate (playerPrefab, Vector3.zero, Quaternion.identity);
			NetworkServer.AddPlayerForConnection (conn, g, playerControllerId);
			HealthManager.AddPlayer(playerControllerId, g.GetComponent<Player>());
			//base.OnServerAddPlayer (conn, playerControllerId);
		}

		public override void OnServerRemovePlayer (NetworkConnection conn, PlayerController player)
		{
			base.OnServerRemovePlayer (conn, player);
			HealthManager.RemovePlayer (player.playerControllerId);
		}
    }
}