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
        private float speed, jumpHeight;
		private Controller controller;
		private Camera mainCamera;
        private Transform modelRoot;
        private BoxCollider box;

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
        

		float y = 0;


        private long lastMilli;
        float grav;
        private Vector3 velocity, direction;
        private const float GRAVITY = 8f;
        public bool ground;
        void Update(){
			if (ground && controller.Jump ()) {
				Jump ();
			}

            //Camera movement
			y = Mathf.Clamp (y - controller.AimY() * sensitivity, -90, 90);
			mainCamera.transform.localEulerAngles = Vector3.right * y;
			modelRoot.eulerAngles += Vector3.up * controller.AimX () * sensitivity;

            //Model movement
            long millis = Millis () - lastMilli;
            direction = (modelRoot.right * controller.MoveX () + modelRoot.forward * controller.MoveY ());
            float y2 = grav - GRAVITY * (millis / 1000f);
            modelRoot.position = rb.position + (velocity + Vector3.down * y2 * (millis / 1000f)) * (millis / 1000f);
        }

        void FixedUpdate () {
            Vector3 v = direction * speed;
            v.y = grav;
            rb.position += v * Time.fixedDeltaTime;
            rb.velocity = Vector3.zero;

            float y;
            if (v.y <= 0 && grounded (rb.transform, out y)) {
                ground = true;
                rb.transform.position = new Vector3(rb.transform.position.x, y, rb.transform.position.z);
                grav = 0;
            } else {
                ground = false;
                grav -= GRAVITY * Time.fixedDeltaTime;
            }
            lastMilli = Millis ();
            velocity = v;
            velocity.y = grav;
        }

		void Jump(){
            grav = jumpHeight;
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

        private static long Millis () { return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond; }
    }
}



