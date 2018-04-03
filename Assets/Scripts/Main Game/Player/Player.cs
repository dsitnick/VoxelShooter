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

		/// <summary>
		/// Calls client initialization functions
		/// </summary>
		public override void OnStartClient () {
			base.OnStartClient ();
			movement.Initialize (InputManager.DEFAULT_CONTROLLER, mainCamera, modelRoot, box);
			weaponManager.Initialize ();

			mainCamera.enabled = false;
		}

		/// <summary>
		/// Calls local-only initialization functions
		/// </summary>
		public override void OnStartLocalPlayer () {
			base.OnStartLocalPlayer ();
			mainCamera.enabled = true;
		}

		/// <summary>
		/// Called from Manager OnServerAddPlayer
		/// Initializes SyncVars
		/// </summary>
		[Server]
        public void ServerInitialize () {
			Character = -1;
			Health = 0;
        }

		#endregion
	
		#region Character

		/// <summary>
		/// Can be invoked locally from UI
		/// Sets the Character value
		/// </summary>
		[Command]
		public void CmdCharacter(int character){
			Character = character;
			//Set to health of player
		}

		/// <summary>
		/// Character hook
		/// Calls local Setup functions
		/// </summary>
		private void SetCharacter(int character){
			Character = character;
			movement.Setup (GameData.Character.Default);
			weaponManager.Setup (character);
		}

		#endregion

		#region Spawn

		/// <summary>
		/// Called from PlayerManager on respawn
		/// </summary>
		[Server]
		public void Spawn (){
			//Set health to max
			RpcSpawn(Vector3.zero, 0);
		}

		/// <summary>
		/// Called from Spawn
		/// Positions the player and calls local spawn functions
		/// </summary>
		[ClientRpc]
		private void RpcSpawn (Vector3 position, float rotation) {
            movement.Spawn (position, rotation);
            weaponManager.Spawn ();
        }

		/// <summary>
		/// Called from PlayerManager at the beginning of respawn
		/// </summary>
		[ClientRpc]
		public void RpcRespawn(float seconds){
			//SET THE DISPLAYED RESPAWN TIME TO X SECONDS
		}

		#endregion

		#region Health

		/// <summary>
		/// Called from local weapons
		/// Deals damage through the player manager
		/// </summary>
		[Command]
		public void CmdDamage(short dest, float amount, Vector3 hitPosition){
			PlayerManager.DealDamage (dest, playerControllerId, amount, hitPosition);
		}

		/// <summary>
		/// Called from PlayerManager
		/// Sets health and potentially calls Die
		/// </summary>
		[Server]
		public void TakeDamage(short srcId, float amount, Vector3 hitPosition){
			Health -= amount;
			if (Health <= 0) {
				Die ();
			}
		}

		//TODO REMOVE
		private const float SPAWNTIME = 3;

		/// <summary>
		/// Called from TakeDamage
		/// Server-side Die functionality
		/// </summary>
		[Server]
		public void Die(){
			PlayerManager.Respawn (playerControllerId, SPAWNTIME);
			RpcDie ();
		}

		/// <summary>
		/// Calls local Die functions
		/// </summary>
		[ClientRpc]
		private void RpcDie () {
			movement.Die ();
			weaponManager.Die ();
		}

		/// <summary>
		/// Health hook
		/// Calls local damage/heal functions
		/// </summary>
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

