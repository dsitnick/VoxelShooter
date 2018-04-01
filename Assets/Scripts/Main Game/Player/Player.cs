using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Match;

namespace Players {

    //The main monobehaviour for each player.
    //This is responsible for all calls to and from the networkmanager, as well as communicating with all components
    public class Player : NetworkBehaviour {

        private Movement movement;
        private WeaponManager weaponManager;

		[SyncVar(hook = "SetCharacter")]
		public int Character;
		[SyncVar(hook = "SetHealth")]
		public float Health;

		public Camera mainCamera;
        public Transform modelRoot; //Change to model manager in future
        public BoxCollider box;

        void Awake () {
            movement = gameObject.GetComponent<Movement> ();
            weaponManager = gameObject.GetComponent<WeaponManager> ();
        }

		#region Initialization

		public override void OnStartClient () {
			base.OnStartClient ();
			movement.Initialize (InputManager.DEFAULT_CONTROLLER, mainCamera, modelRoot, box);
			weaponManager.Initialize ();

			mainCamera.enabled = false;
		}

		public override void OnStartLocalPlayer ()
		{
			base.OnStartLocalPlayer ();
			mainCamera.enabled = true;
		}

		[Server]
        public void ServerInitialize () {
			Character = -1;
			Health = 0;
        }

		#endregion
	
		#region Character

		[Command]
		public void CmdCharacter(int character){
			Character = character;
			//Set to health of player
		}

		//Character hook
		private void SetCharacter(int character){
			Debug.Log ("Setting " + playerControllerId + " to " + character);
			Character = character;
			movement.Setup (GameData.Character.Default);
			weaponManager.Setup (character);
		}

		#endregion

		#region Spawn

		[Server]
		public void Spawn (){
			//Set health to max
			RpcSpawn(Vector3.zero, 0);
		}
			
		[ClientRpc]
		private void RpcSpawn (Vector3 position, float rotation) {
            movement.Spawn (position, rotation);
            weaponManager.Spawn ();
        }

		[ClientRpc]
		public void RpcRespawn(float seconds){
			//SET THE DISPLAYED RESPAWN TIME TO X SECONDS
		}

		#endregion

		#region Health

		//Called 
		[Server]
		public void TakeDamage(int srcId, float amount, Vector3 hitPosition){
			Health -= amount;
			if (Health <= 0) {
				Die ();
			}
		}

		//TODO REMOVE
		private const float SPAWNTIME = 3;

		[Server]
		public void Die(){
			PlayerManager.Respawn (playerControllerId, SPAWNTIME);
			RpcDie ();
		}

		[ClientRpc]
		private void RpcDie () {
			movement.Die ();
			weaponManager.Die ();
		}

		//Health hook
		private void SetHealth(float health){
			Health = health;
			if (health < 0){
				//Local Damage thing
			}
			if (health > 0){
				//Local Heal thing
			}
		}

		#endregion
    }
}

