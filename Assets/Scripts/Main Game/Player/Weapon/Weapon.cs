using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Match;
using GameData;

namespace Players {
	public abstract class Weapon : NetworkBehaviour {

		protected WeaponManager manager;

		public virtual void Setup (WeaponData data){

		}
	}

	public abstract class HitscanWeapon : Weapon {
		public int mask;
		private const float MAX_DISTANCE = 256;

		void Awake(){
			mask = 1 << LayerMask.NameToLayer ("Default");
		}


		public void Hitscan(Vector3 position, Vector3 direction){
			RaycastHit h;
			if (Physics.Raycast(position, direction, out h, MAX_DISTANCE, mask)){
				Player p = h.transform.GetComponent<Player> ();
				if (p != null) {
					bool headshot = h.transform.tag == "Head";
					CmdHitscan (p.playerControllerId, position, h.point, headshot);
				}
			}
		}

		[Command]
		private void CmdHitscan(short id, Vector3 source, Vector3 point, bool headshot){
			Damage d = new Damage (CalcDamage (Vector3.Distance (source, point), headshot));
			PlayerManager.DealDamage (id, playerControllerId, d, point);
		}

		protected abstract float CalcDamage (float distance, bool headshot);
	}

	public abstract class ProjectileWeapon : Weapon {
		public void Projectile(Vector3 position, Vector3 direction){
			CmdProjectile (position, direction);
		}

		[Command]
		private void CmdProjectile(Vector3 position, Vector3 direction){

		}
	}
}