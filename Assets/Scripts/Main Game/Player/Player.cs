﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Match {

    //The main monobehaviour for each player.
    //This is responsible for all calls to and from the networkmanager, as well as communicating with all components
    public class Player : NetworkBehaviour {

        private Movement movement;
        private WeaponManager weaponManager;

		public Camera mainCamera;
        public Transform modelRoot; //Change to model manager in future
        public BoxCollider box;

        void Awake () {
            movement = gameObject.AddComponent<Movement> ();
            weaponManager = gameObject.AddComponent<WeaponManager> ();
        }

        #region Primary Methods

        //Called OnStartClient
        public void Initialize () {
            movement.Initialize (InputManager.DEFAULT_CONTROLLER, mainCamera, modelRoot, box);
            weaponManager.Initialize ();
        }

        //Called RpcSetup
        public void Setup (int index) {
			movement.Setup (GameData.Character.Default);
            weaponManager.Setup (index);
        }
			
		private void Spawn (Vector3 position, float rotation) {
            movement.Spawn (position, rotation);
            weaponManager.Spawn ();
        }

        private void Die () {
            movement.Die ();
            weaponManager.Die ();
        }

        #endregion

        #region Network Events

        public override void OnStartClient () {
            base.OnStartClient ();
            Initialize ();

			//TEMPORARY
			Setup (0);
			Spawn (Vector3.up * 5, 0);
        }

		[ClientRpc]
		public void RpcSpawn (Vector3 position, float rotation) {
			Spawn (position, rotation);
		}

		[ClientRpc]
		public void RpcDie () {
			Die ();
		}

        [ClientRpc]
        public void RpcSetup(int index) {
            Setup (index);
        }

        //Invoke to set character
        [Command]
        public void CmdSetup(int index) {
            RpcSetup (index);
        }

		[Command]
		public void CmdDamage(float amount){
			HealthManager.TakeDamage (playerControllerId, amount);
		}

        #endregion
    }
}

