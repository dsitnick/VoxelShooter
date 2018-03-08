using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// This is the base class for a controller instance, with functions to be overridden for different 
/// </summary>
public abstract class Controller {

	public abstract float MoveX ();
	public abstract float MoveY ();
	public abstract float AimX ();
	public abstract float AimY ();
	public abstract bool Fire ();
	public abstract bool FireDown ();
	public abstract bool SecondaryFire ();
	public abstract bool SecondaryFireDown ();
	public abstract bool Jump ();
	public abstract bool Reload ();

	public abstract void Update ();
}

public class PCController : Controller {

	public override float MoveX(){ return Input.GetAxisRaw ("Horizontal"); }
	public override float MoveY (){ return Input.GetAxisRaw ("Vertical"); }
	public override float AimX (){ return Input.GetAxisRaw ("Mouse X"); }
	public override float AimY (){ return Input.GetAxisRaw ("Mouse Y"); }
	public override bool Fire (){ return Input.GetMouseButton(0); }
	public override bool FireDown (){ return Input.GetMouseButtonDown(0); }
	public override bool SecondaryFire (){ return Input.GetMouseButton(1); }
	public override bool SecondaryFireDown (){ return Input.GetMouseButtonDown(1); }
	public override bool Jump (){ return Input.GetKeyDown(KeyCode.Space); }
	public override bool Reload (){ return Input.GetKey(KeyCode.R); }

	public override void Update () { }
}
