using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Players {
    //Handles the usage of weapons
    public class WeaponManager : NetworkBehaviour {

		private HashSet<Weapon> weapons;

		private bool active;

        public void Initialize () {
			weapons = new HashSet<Weapon> ();
			active = false;
        }

        public void Setup (int index) {
			foreach (Weapon w in weapons) {
				//CLEAR WEAPONS
				//Destroy(w);
			}
			weapons.Clear ();
			//ADD WEAPONS TO SET
        }

        public void Spawn () {
			active = true;
        }

        public void Die () {
			active = false;
        }
    }
}



