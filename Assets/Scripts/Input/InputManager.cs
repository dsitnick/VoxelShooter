using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

	public static Dictionary<int, Controller> ActiveControllers = new Dictionary<int, Controller> ();
	public static Controller DEFAULT_CONTROLLER = new PCController ();

	void Awake(){
		ActiveControllers.Add(0, DEFAULT_CONTROLLER);
	}
	
	void Update () {
		foreach (Controller c in ActiveControllers.Values) {
			c.Update ();
		}
	}
}
