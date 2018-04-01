using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Players;

namespace Match {

    public class Manager : NetworkManager {

		public PlayerManager playerManager;

		void Start(){
			//StartHost ();
		}

		public override void OnClientConnect (NetworkConnection conn)
		{
			base.OnClientConnect (conn);
			int index = conn.connectionId * 4;
			ClientScene.AddPlayer (conn, (short)index);
		}

		public override void OnStartServer ()
		{
			base.OnStartServer ();
		}

		public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId)
		{
			GameObject g = Instantiate (playerPrefab, Vector3.zero, Quaternion.identity);
			NetworkServer.AddPlayerForConnection (conn, g, playerControllerId);

			Player p = g.GetComponent<Player> ();
			PlayerManager.Add (playerControllerId, p);
			p.ServerInitialize ();

			//TODO Remove
			p.Spawn ();
		}

		public override void OnServerRemovePlayer (NetworkConnection conn, PlayerController player)
		{
			base.OnServerRemovePlayer (conn, player);

			PlayerManager.Remove (player.playerControllerId);
		}
    }
}