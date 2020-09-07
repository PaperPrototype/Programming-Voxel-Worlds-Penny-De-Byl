using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
	public Material cubeMateral;
	public Block[,,] chunkData;

    IEnumerator BuildChunk(int sizeX, int sizeY, int sizeZ)
    {
		chunkData = new Block[sizeX, sizeY, sizeZ];

        //create blocks
		for(int z = 0; z < sizeZ; z++)
        {
			for (int y = 0; y < sizeY; y++)
			{
				for (int x = 0; x < sizeX; x++)
				{
                    if(Random.Range(0,100) < 50)
                    {
						Vector3 pos = new Vector3(x, y, z);
						chunkData[x, y, z] = new Block(Block.BlockType.DIRT, pos, this.gameObject, cubeMateral);
					}
                    else
                    {
						Vector3 pos = new Vector3(x, y, z);
						chunkData[x, y, z] = new Block(Block.BlockType.AIR, pos, this.gameObject, cubeMateral);
					}
				}
			}
		}

        //draw blocks
		for (int z = 0; z < sizeZ; z++)
        {
			for (int y = 0; y < sizeY; y++)
			{
				for (int x = 0; x < sizeX; x++)
				{
					chunkData[x,y,z].Draw();
					
				}
			}
		}
		CombineQuads();
		yield return null;
	}

    private void Start()
    {
		StartCoroutine(BuildChunk(16, 32, 16));
    }

    void CombineQuads()
	{

		//1. Combine all children meshes
		MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
		CombineInstance[] combine = new CombineInstance[meshFilters.Length];
		int i = 0;
		while (i < meshFilters.Length)
		{
			combine[i].mesh = meshFilters[i].sharedMesh;
			combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
			i++;
		}

		//2. Create a new mesh on the parent object
		MeshFilter mf = (MeshFilter)this.gameObject.AddComponent(typeof(MeshFilter));
		mf.mesh = new Mesh();

		//3. Add combined meshes on children as the parent's mesh
		mf.mesh.CombineMeshes(combine);

		//4. Create a renderer for the parent
		MeshRenderer renderer = this.gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
		renderer.material = cubeMateral;

		//5. Delete all uncombined children
		foreach (Transform quad in this.transform)
		{
			Destroy(quad.gameObject);
		}

	}
}
