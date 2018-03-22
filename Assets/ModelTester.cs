using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelTester : MonoBehaviour {
	public MeshFilter filter;

	void Start(){
		VoxelMesh m = new VoxelMesh (3, 3, 3, 1);
		m.Set (0, 0, 0, 0);
		m.Set (1, 0, 0, 1);
		m.Set (2, 0, 0, 2);
		m.Set (0, 1, 0, 3);
		m.Set (2, 1, 0, 4);
		m.Set (0, 0, 1, 5);
		m.Set (1, 1, 1, 6);
		m.Set (2, 2, 2, 7);

		filter = GetComponent<MeshFilter> ();
		filter.mesh = m.ToMesh ();
	}
}
