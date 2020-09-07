using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public Voxel[,,] ChunkData;

    public Material material;

    public VoxelType voxelType;

    int chunkHeight = 16;
    int chunkArea = 8;

    List<Vector3> vertices = new List<Vector3>();
    List<Vector3> normals = new List<Vector3>();
    List<Vector2> uvs = new List<Vector2>();
    List<int> triangles = new List<int>();

    private void Start()
    {
        Mesh mesh = new Mesh();

        mesh.name =
            "Chunk_"
            + transform.position.x + "_"
            + transform.position.y + "_"
            + transform.position.z;

        ChunkData = new Voxel[chunkArea, chunkHeight, chunkArea];

        // calculate the chunk data
        for (int z = 0; z < chunkArea; z++)
            for (int y = 0; y < chunkHeight; y++)
                for (int x = 0; x < chunkArea; x++)
                {
                    Vector3 pos = new Vector3(x, y, z);
                    ChunkData[x, y, z] = new Voxel(voxelType, pos);
                }

        // draw the chunk based on its data
        for (int z = 0; z < chunkArea; z++)
            for (int y = 0; y < chunkHeight; y++)
                for (int x = 0; x < chunkArea; x++)
                {
                    ChunkData[x, y, z].DrawVoxel(vertices, normals, uvs, triangles);
                }


        mesh.vertices = vertices.ToArray();

        mesh.normals = normals.ToArray();

        mesh.uv = uvs.ToArray();

        mesh.triangles = triangles.ToArray();

        mesh.RecalculateBounds();

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = material;

    }
}
