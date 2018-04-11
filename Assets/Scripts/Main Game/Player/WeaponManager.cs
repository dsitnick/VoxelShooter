using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using GameData;

namespace Players {
    //Handles the usage of weapons
    public class WeaponManager : NetworkBehaviour {

		public GameObject SniperPrefab;

		private HashSet<Weapon> weapons;

		private bool active;

        public void Initialize () {
			weapons = new HashSet<Weapon> ();
			active = false;
        }

		[Server]
		public void Setup (List<WeaponData> data) {
			foreach (Weapon w in weapons) {
				//CLEAR WEAPONS
				Destroy(w.gameObject);
			}
			weapons.Clear ();
			GameObject g;
			foreach (WeaponData d in data) {
				switch (d.type) {
				case WeaponData.Type.Sniper:
					g = Instantiate (SniperPrefab, transform);
					break;
				default:
					g = null;
					Debug.LogError ("Weapon type " + d.type + " not supported yet!");
					return;
				}
				NetworkServer.SpawnWithClientAuthority (g, gameObject);
				weapons.Add (g.GetComponent<Weapon> ());
			}

			foreach (Weapon w in weapons) {
				w.Initialize (this);
			}
        }

        public void Spawn () {
			active = true;
        }

        public void Die () {
			active = false;
        }
    }
}



