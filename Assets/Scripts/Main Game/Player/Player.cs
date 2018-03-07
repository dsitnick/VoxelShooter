using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Game {

    //The main monobehaviour for each player.
    //This is responsible for all calls to and from the networkmanager, as well as communicating with all components
    public class Player : NetworkBehaviour {

        private PlayerMovement movement;

        void Awake () {
            movement = GetComponent<PlayerMovement> ();
        }

        #region Primary Methods

        //Called OnStartClient
        public void Initialize () {
            movement.Initialize ();
        }

        //Called RpcSetup
        public void Setup (int index) {
            movement.Setup (index);
        }

        public void Spawn () {
            movement.Spawn ();
        }

        public void Die () {
            movement.Die ();
        }

        #endregion

        #region Network Events

        public override void OnStartClient () {
            base.OnStartClient ();
            Initialize ();
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

        #endregion
    }
}

