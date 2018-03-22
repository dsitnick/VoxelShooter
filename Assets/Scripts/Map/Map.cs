using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Map  {
    public const int AIR = -1;
    public int[] voxels;
    public int x, y, z;

    public Map(int x, int y, int z) {
        voxels = new int[x * y * z];
        for (int i = 0; i < voxels.Length; i++) { voxels[i] = AIR; }
        this.x = x;
        this.y = y;
        this.z = z;
    }

    private int Coordinate(int x, int y, int z) { return x + y * this.x + z * this.x * this.y; }
    private bool OutOfBounds(int x, int y, int z) { return x < 0 || y < 0 || z < 0 || x >= this.x || y >= this.y || z >= this.z; }

    public void Set(int x, int y, int z, int value) {
        if (OutOfBounds(x, y, z)) {
            return;
        }
        voxels[Coordinate (x, y, z)] = value;
    }

    public int Get(int x, int y, int z) {
        if (OutOfBounds (x, y, z)) {
            return AIR;
        }
        return voxels[Coordinate (x, y, z)];
    }

    public bool Air(int x, int y, int z) {
        if (OutOfBounds (x, y, z)) {
            return true;
        }
        return voxels[Coordinate (x, y, z)] == AIR;
    }
}
