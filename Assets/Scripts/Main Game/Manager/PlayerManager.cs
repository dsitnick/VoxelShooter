using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Players;

namespace Match {
	public class PlayerManager : NetworkBehaviour {

		private static PlayerManager singleton;
		public static PlayerManager SINGLETON { get { return singleton; } }

		private Dictionary<short,Player> playerMap;

		public override void OnStartClient () {
			base.OnStartClient ();
			singleton = this;
			playerMap = new Dictionary<short, Player> ();

			foreach (PlayerController c in ClientScene.localPlayers) {
				playerMap.Add (c.playerControllerId, c.gameObject.GetComponent<Player> ());
			}
		}

		public static void Add(short id, Player player){
			singleton.playerMap.Add (id, player);
			if (singleton.playerMap.Count != ClientScene.localPlayers.Count) {
				Debug.LogError ("Player Map: " + singleton.playerMap.Count +
				", Local Players: " + ClientScene.localPlayers);
				singleton.playerMap.Clear ();
				foreach (PlayerController c in ClientScene.localPlayers) {
					singleton.playerMap.Add (c.playerControllerId, c.gameObject.GetComponent<Player> ());
				}
			}
		}

		public static void Remove(short id){
			singleton.playerMap.Remove (id);
		}

		public static void DealDamage(short id, short src, float amount, Vector3 hitPosition){
			singleton.CmdDamage (id, src, amount, hitPosition);
		}

		[Command]
		private void CmdDamage(short id, short src, float amount, Vector3 hitPosition){
			if (!singleton.playerMap.ContainsKey (id)) {
				return;
			}
			singleton.playerMap [id].TakeDamage (src, amount, hitPosition);
		}

		public static void Respawn(short id, float seconds){
			singleton.StartCoroutine (singleton.respawnRoutine (id, seconds));
		}

		private IEnumerator respawnRoutine(short id, float seconds){
			singleton.playerMap [id].RpcRespawn (seconds);
			yield return new WaitForSeconds (seconds);
			singleton.playerMap [id].Spawn ();
		}
	}
}
