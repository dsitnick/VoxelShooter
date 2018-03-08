using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Match {

    public class Manager : NetworkManager {

		void Start(){
			StartHost ();
		}
    }
}