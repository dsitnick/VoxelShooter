using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using GameData;
using System;

namespace Match {

    //Handles all motion, physics, knockback, etc
    public class Movement : MonoBehaviour {

		private Rigidbody rb;
		private Controller controller;
		private Camera mainCamera;
        private Transform modelRoot;
        private BoxCollider box;

		private float speed, jumpHeight;
		private bool active = false;

        int mask;

        //TEMPORARY
        public const float sensitivity = 5;

		#region Primary Methods

		void Awake(){
			rb = GetComponent<Rigidbody> ();
            mask = 1 << LayerMask.NameToLayer ("Map");
			Setup (Character.Default); //TEMPORARY
		}

		public void Initialize (Controller controller, Camera mainCamera, Transform modelRoot, BoxCollider box) {
			this.controller = controller;
			this.mainCamera = mainCamera;
            this.modelRoot = modelRoot;
            this.box = box;
			active = false;
        }

        public void Setup (Character character) {
			speed = character.speed;
			jumpHeight = character.jumpHeight;
        }

		public void Spawn (Vector3 position, float rotation) {
			SetPosition (position);
			modelRoot.eulerAngles = Vector3.up * rotation;
			active = true;
        }

        public void Die () {
			active = false;
        }

		#endregion
        
		private const float GRAVITY = 8f, AIR_DRIFT = 0.1f;

		private float aimY = 0; //y angle of players aim
		private Vector3 direction; //Worldspace direction of suggested character movement, set on Update
        void Update(){
			if (!active) 
				return;

			//Jump Polling
			if (ground && controller.Jump ()) {
				Jump ();
			}

            //Camera controlling, body rotating
			aimY = Mathf.Clamp (aimY - controller.AimY() * sensitivity, -90, 90);
			mainCamera.transform.localEulerAngles = Vector3.right * aimY;
			modelRoot.eulerAngles += Vector3.up * controller.AimX () * sensitivity;

            //Movement controlling
			direction = modelRoot.right * controller.MoveX () + modelRoot.forward * controller.MoveY ();

			//Smooth Model movement
			float delta = (Millis () - lastTime) / 1000f;
			float modelGrav = ground ? 0 : GRAVITY * delta;
            Vector3 pos = rb.position + (rb.velocity - Vector3.up * modelGrav) * delta;
			modelRoot.position = pos;
        }
			
		private float fallSpeed; //Authoritative over rigidbody's y velocity
		public bool ground; //Running boolean for grounded since last check, set on FixedUpdate
        void FixedUpdate () {
			if (!active)
				return;

			//Updates velocity
			Vector3 velocity;
			if (ground) {
				velocity = direction * speed;
			}else{
				velocity = Vector3.Lerp (rb.velocity, direction * speed, AIR_DRIFT);
			}

			//Performing ground check
            float hitY;
            if (fallSpeed <= 0 && grounded (rb.transform, out hitY)) {
				//If grounded, position player at ground hit position, null the y velocity
                ground = true;
                rb.transform.position = new Vector3(rb.transform.position.x, hitY, rb.transform.position.z);
                fallSpeed = 0;
            } else {
				//If airborne, decrease fall speed by gravity
                ground = false;
                fallSpeed -= GRAVITY * Time.fixedDeltaTime;
            }
			velocity.y = fallSpeed;
			rb.velocity = velocity;

			lastTime = Millis ();
        }

		void Jump(){
            fallSpeed = jumpHeight;
		}

        private const float groundDistance = 0.2f;
        private bool grounded (Transform t, out float y) {
            bool result = false;
            Vector3 center = t.position + Vector3.up * groundDistance / 2f;
            Ray[] rays = new Ray[] {
                new Ray(center, Vector3.down),
                new Ray(center + Vector3.right * box.size.x / 2f + Vector3.forward * box.size.z, Vector3.down),
                new Ray(center - Vector3.right * box.size.x / 2f - Vector3.forward * box.size.z, Vector3.down),
                new Ray(center - Vector3.right * box.size.x / 2f + Vector3.forward * box.size.z, Vector3.down),
                new Ray(center - Vector3.forward * box.size.z, Vector3.down)
            };
            RaycastHit h;
            foreach (Ray r in rays) {
                if (Physics.Raycast(r, out h, groundDistance, mask)) {
                    y = h.point.y;
                    result = true;
                }
            }
            y = 0;
            return result;
        }

		private void SetPosition(Vector3 position){
			modelRoot.position = rb.position = position;
		}

		private long lastTime;
        private static long Millis () { return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond; }
    }
}



