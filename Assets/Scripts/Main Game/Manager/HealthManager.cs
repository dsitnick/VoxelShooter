using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Match{
	public class HealthManager : NetworkBehaviour {

		public static HealthManager SINGLETON;
		void Awake(){
			SINGLETON = this;
		}

		//TEMPORARY
		private static float HEALTH = 100;

		private SyncListFloat healthList = new SyncListFloat();
		private SyncListInt idList = new SyncListInt();

		private Dictionary<int, Player> playerMap;

		public static void Initialize(){
			SINGLETON.healthList.Clear ();
			SINGLETON.idList.Clear ();
			SINGLETON.playerMap = new Dictionary<int, Player> ();
		}

		public static void AddPlayer(int id, Player player){
			SINGLETON.idList.Add (id);
			SINGLETON.healthList.Add (0);

			SINGLETON.playerMap.Add (id, player);
		}

		public static void RemovePlayer(int id){
			int index = SINGLETON.indexOf (id);

			SINGLETON.idList.RemoveAt (index);
			SINGLETON.healthList.RemoveAt (index);

			SINGLETON.playerMap.Remove (id);
		}

		public static float GetHealth(int id){
			return SINGLETON.healthList [SINGLETON.indexOf(id)];
		}

		public static float GetSpawn(int id){
			return SINGLETON.healthList [SINGLETON.indexOf(id)];
		}

		public static void TakeDamage(int id, float amount){
			int index = SINGLETON.indexOf (id);
			float health = SINGLETON.healthList [index];
			if (health <= 0)
				return;
			
			health -= amount;
			SINGLETON.healthList [index] = health;
			if (health <= 0) {
				Die (id);
			}
		}

		public static void Die(int id){
			int index = SINGLETON.indexOf (id);
			SINGLETON.healthList [index] = 0;
			SINGLETON.playerMap [id].RpcDie ();
		}

		public static void Spawn(int id){
			int index = SINGLETON.indexOf (id);
			SINGLETON.healthList [index] = HEALTH;
			SINGLETON.playerMap [id].RpcSpawn (Vector3.zero, 0);
		}

		private int indexOf(int id){
			for (int i = 0; i < idList.Count; i++) {
				if (idList [i] == id) {
					return i;
				}
			}
			return -1;
		}
	}
}
