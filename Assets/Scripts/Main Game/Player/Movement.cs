﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using GameData;

namespace Match {

    //Handles all motion, physics, knockback, etc
    public class Movement : MonoBehaviour {

		private Rigidbody rb;
        private float speed, jumpHeight;
		private Controller controller;
		private Camera mainCamera;

		//TEMPORARY
		public float sensitivity;

		#region Primary Methods

		void Awake(){
			rb = GetComponent<Rigidbody> ();
			Setup (Character.Default); //TEMPORARY
		}

		public void Initialize (Controller controller, Camera mainCamera) {
			this.controller = controller;
			this.mainCamera = mainCamera;
        }

        public void Setup (Character character) {
			speed = character.speed;
			jumpHeight = character.jumpHeight;
        }

        public void Spawn () {

        }

        public void Die () {

        }

		#endregion

		private const float SPEED_MULTIPLIER = 0.2f;

		float y = 0;

		void Update(){
			if (controller.Jump ()) {
				Jump ();
			}
			y = Mathf.Clamp (y - controller.AimY() * sensitivity, -90, 90);
			mainCamera.transform.localEulerAngles = Vector3.right * y;
			rb.transform.eulerAngles += Vector3.up * controller.AimX () * sensitivity;
		}

		void FixedUpdate(){
			Vector3 v = (transform.right * controller.MoveX () + transform.forward * controller.MoveY ()) * speed * SPEED_MULTIPLIER * Time.fixedDeltaTime;
			rb.position += v;
		}

		void Jump(){
			
		}
    }
}


