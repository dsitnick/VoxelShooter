using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Players;
using GameData;

namespace Match {
	public class PlayerManager : NetworkBehaviour {

		private static PlayerManager singleton;
		public static PlayerManager SINGLETON { get { return singleton; } }

		private Dictionary<short,Player> playerMap;

		public override void OnStartServer () {
			base.OnStartServer ();
			singleton = this;
			playerMap = new Dictionary<short, Player> ();
		}

		/// <summary>
		/// Called from Manager
		/// Adds a Player to the map
		/// </summary>
		public static void Add(short id, Player player){
			singleton.playerMap.Add (id, player);
			if (singleton.playerMap.Count != NetworkServer.connections.Count) {
				Debug.LogError ("Player Map: " + singleton.playerMap.Count +
					", Connections: " + NetworkServer.connections.Count);
				singleton.playerMap.Clear ();
				foreach (PlayerController c in ClientScene.localPlayers) {
					singleton.playerMap.Add (c.playerControllerId, c.gameObject.GetComponent<Player> ());
				}
			}
		}

		/// <summary>
		/// Called from Manager
		/// Removes the player from the map
		/// </summary>
		public static void Remove(short id){
			singleton.playerMap.Remove (id);
		}

		/// <summary>
		/// Called from Player with authority
		/// Deals damage to the target player
		/// </summary>
		public static void DealDamage(short id, short src, Damage damage, Vector3 hitPosition){
			if (!singleton.playerMap.ContainsKey (id)) {
				return;
			}
			singleton.playerMap [id].TakeDamage (src, damage, hitPosition);
		}

		/// <summary>
		/// Called from Die on the server
		/// Starts the respawn timer on the server
		/// </summary>
		public static void Respawn(short id, float seconds){
			singleton.StartCoroutine (singleton.respawnRoutine (id, seconds));
		}

		/// <summary>
		/// Informs clients of respawn time, waits, then spawns the player
		/// </summary>
		private IEnumerator respawnRoutine(short id, float seconds){
			singleton.playerMap [id].RpcRespawn (seconds);
			yield return new WaitForSeconds (seconds);
			singleton.playerMap [id].Spawn ();
		}
	}
}
