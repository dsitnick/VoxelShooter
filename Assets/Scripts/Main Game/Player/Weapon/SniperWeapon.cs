using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using GameData;

namespace Players {
	public class SniperWeapon : HitscanWeapon {

		[SyncVar]
		private Damage damage;

		[SyncVar]
		private float headshotDamage;

		protected override float CalcDamage (float distance, bool headshot) {
			return damage.amount * (headshot ? headshotDamage : 1);
		}

	}
}