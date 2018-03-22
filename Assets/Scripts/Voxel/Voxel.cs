using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct VoxelMesh {

	public const int AIR = -1;

	public Vector3Int size;
	public float scale;
	private int[] data;

	/// <summary>
	/// Initializes a new instance of the <see cref="VoxelMesh"/> struct.
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	/// <param name="z">The z coordinate.</param>
	public VoxelMesh(int x, int y, int z, float scale){
		size = new Vector3Int (x, y, z);
		this.scale = scale;
		data = new int[x * y * z];
		for (int i = 0; i < data.Length; i++) {
			data [i] = AIR;
		}
	}

	/// <summary>
	/// Copy constructor
	/// Initializes a new instance of the <see cref="VoxelMesh"/> struct.
	/// </summary>
	/// <param name="mesh">Mesh.</param>
	public VoxelMesh(VoxelMesh mesh){
		size = new Vector3Int (mesh.size.x, mesh.size.y, mesh.size.z);
		scale = mesh.scale;
		data = new int[size.x * size.y * size.z];
		for (int i = 0; i < data.Length; i++) {
			this.data [i] = data [i];
		}
	}

	/// <summary>
	/// Set the value specified x, y, z with value.
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	/// <param name="z">The z coordinate.</param>
	/// <param name="value">Value.</param>
	public void Set(int x, int y, int z, int value){
		if (!InBounds (x, y, z))
			return;
		
		data [index (x, y, z)] = value;
	}

	/// <summary>
	/// Get the value at specified x, y and z.
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	/// <param name="z">The z coordinate.</param>
	public int Get(int x, int y, int z){
		if (!InBounds (x, y, z))
			return AIR;

		return data [index (x, y, z)];
	}

	/// <summary>
	/// Determines whether this coordinate is occupied
	/// </summary>
	/// <returns><c>true</c> if this coordinate is occupied; otherwise, <c>false</c>.</returns>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	/// <param name="z">The z coordinate.</param>
	public bool IsBlock(int x, int y, int z){
		return InBounds(x, y, z) && data[index(x, y, z)] != AIR;
	}

	//Helper function to determine whether the given coordinates are inside the bounds
	private bool InBounds(int x, int y, int z){
		return x >= 0 && y >= 0 && z >= 0 && x < size.x && y < size.y && z < size.z;
	}

	//Helper function for the index of the given coordinates based on size
	private int index(int x, int y, int z){
		return x + y * size.x + z * size.x * size.y;
	}

	//Helper function for the index at the given coordinate, or CLEAR if not
	private Color getColor(int x, int y, int z){
		return IsBlock (x, y, z) ? VoxelColors.COLORS [data [index (x, y, z)]] : Color.clear;
	}

	/// <summary>
	/// Creates a Mesh class representing the voxel model
	/// </summary>
	/// <returns>The mesh.</returns>
	public Mesh ToMesh(){
		Mesh mesh = new Mesh();
		List<Vector3> verts = new List<Vector3> ();
		List<int> tris = new List<int> ();
		List<Vector3> norms = new List<Vector3> ();
		List<Color32> colors = new List<Color32> ();

		Vector3 offset = (Vector3)size / 2f;

		for (int x = 0; x < size.x; x++) {
			for (int y = 0; y < size.y; y++) {
				for (int z = 0; z < size.z; z++) {
					
					if (data[index(x, y, z)] != AIR){
						Vector3 pos = new Vector3 (x, y, z) - offset;

						if (!IsBlock(x + 1, y, z)){ //RIGHT
							addSide (verts, tris, norms, colors, 0, pos, getColor(x,y,z));
						}
						if (!IsBlock(x - 1, y, z)){ //LEFT
							addSide (verts, tris, norms, colors, 1, pos, getColor(x,y,z));
						}
						if (!IsBlock(x, y + 1, z)){ //UP
							addSide (verts, tris, norms, colors, 2, pos, getColor(x,y,z));
						}
						if (!IsBlock(x, y - 1, z)){ //DOWN
							addSide (verts, tris, norms, colors, 3, pos, getColor(x,y,z));
						}
						if (!IsBlock(x, y, z + 1)){ //FORWARD
							addSide (verts, tris, norms, colors, 4, pos, getColor(x,y,z));
						}
						if (!IsBlock(x, y, z - 1)){ //BACK
							addSide (verts, tris, norms, colors, 5, pos, getColor(x,y,z));
						}
					}

				}
			}
		}

		mesh.SetVertices (verts);
		mesh.SetTriangles (tris, 0);
		mesh.SetNormals (norms);
		mesh.SetColors (colors);
		return mesh;
	}

	//12
	//03
	private static Vector3[] SIDE_POINTS = new Vector3[] {
		new Vector3(-0.5f, -0.5f, -0.5f),
		new Vector3(-0.5f, 0.5f, -0.5f),
		new Vector3(-0.5f, 0.5f, 0.5f),
		new Vector3(-0.5f, -0.5f, 0.5f)
	};
	private static Vector3[] DIRECTIONS = new Vector3[] {
		Vector3.right, Vector3.left,
		Vector3.up, Vector3.down,
		Vector3.forward, Vector3.back
	};
	private static Quaternion[] ROTATIONS = new Quaternion[]{
		Quaternion.Euler(Vector3.forward * 180), //RIGHT
		Quaternion.identity,                     //LEFT
		Quaternion.Euler(Vector3.forward * -90), //UP
		Quaternion.Euler(Vector3.forward * 90),  //DOWN
		Quaternion.Euler(Vector3.up * 90),       //FORWARD
		Quaternion.Euler(Vector3.up * -90)       //BACK
	};
	//Number of verts in a side
	private const int SIDE_COUNT = 4;
	//12
	//03
	private static int[] SIDE_TRIS = new int[]{
		0, 2, 1, 0, 3, 2
	};

	//Helper function to add the data for a cube face to the given lists
	//Determines the direction which should be up down left right forward or back
	private void addSide(List<Vector3> verts, List<int> tris, List<Vector3> norms, List<Color32> colors,
		int side, Vector3 pos, Color color){
		int c = verts.Count;

		for (int i = 0; i < SIDE_COUNT; i++) {
			verts.Add ((ROTATIONS[side] * SIDE_POINTS[i] + pos) * scale);
			colors.Add (color);
			norms.Add (DIRECTIONS[side]);
		}
		foreach (int t in SIDE_TRIS)
			tris.Add (t + c);
	}

	/// <summary>
	/// Creates the set of particle structs needed to assemble the voxel model in the particle system
	/// </summary>
	/// <returns>Array of particle structs</returns>
	public ParticleSystem.Particle[] ToParticle(){
		List<ParticleSystem.Particle> result = new List<ParticleSystem.Particle> ();
		ParticleSystem.Particle p = new ParticleSystem.Particle ();
		p.startLifetime = 5000;
		p.remainingLifetime = 50000;
		p.velocity = Vector3.zero;

		Vector3 offset = (Vector3)size / 2;
		for (int x = 0; x < size.x; x++) {
			for (int y = 0; y < size.y; y++) {
				for (int z = 0; z < size.z; z++) {
					if (data[index(x, y, z)] != AIR){
						p.position = new Vector3 (x, y, z) - offset;
						p.startColor = getColor (x, y, z);
						result.Add (p);
					}
				}
			}
		}
		return result.ToArray ();
	}
}
