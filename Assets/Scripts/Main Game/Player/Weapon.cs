using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Players {
	public abstract class Weapon : NetworkBehaviour {

		public bool fire0, fire1; //These are input bools to be controlled by the WeaponManager

		public abstract void Fire0 ();
		public abstract void Fire1 ();
	}
}