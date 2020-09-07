using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {

    #region Variables
    public Material textureAtlasMaterial;
	public static int chunkSize = 32;
	public static int chunkHeight = 64;

    private static int chunkColumnHeight = 16;

	public static Vector3[,,] allVertices = new Vector3[chunkSize + 1,chunkHeight + 1,chunkSize + 1];
	public static Vector3[] allNormals = new Vector3[6];
	public enum NDIR {UP, DOWN, LEFT, RIGHT, FRONT, BACK}

    public static Dictionary<string, Chunk> chunks = new Dictionary<string, Chunk>();
    #endregion

    #region Custom Methods
    public static string BuildChunkName(Vector3 v)
    {
		return
			(int)v.x + "_" +
			(int)v.y + "_" +
			(int)v.z;
	}

	private void BuildChunkAt(Vector3 pos)
    {
		Chunk c = new Chunk(chunkSize, chunkHeight, chunkSize, pos, gameObject, textureAtlasMaterial);
		chunks.Add(c.chunk.name, c);
	}

    IEnumerator BuildChunkColumn()
    {
        for(int i = 0; i < chunkColumnHeight; i++)
        {
            Vector3 chunkPos = new Vector3
                (transform.position.x, i * chunkHeight, transform.position.z);

            BuildChunkAt(chunkPos);
            yield return null;
        }
    }

    private void SetUp()
    {
        // make sure the World is centered
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        // if the chunk size is set lower by user then
        // optimize for smaller mesh data
        if (World.chunkSize * World.chunkHeight * World.chunkSize < 16384)
        {
            Debug.Log("Lower IndexFormat for chunk meshes");
        }
        // else set max mesh data higher
        else
        {
            Debug.Log("Higher IndexFormat for chunk meshes");
        }

        //generate all vertices
        for (int z = 0; z <= chunkSize; z++)
            for (int y = 0; y <= chunkHeight; y++)
                for (int x = 0; x <= chunkSize; x++)
                {
                    allVertices[x, y, z] = new Vector3(x, y, z);
                }

        allNormals[(int)NDIR.UP] = Vector3.up;
        allNormals[(int)NDIR.DOWN] = Vector3.down;
        allNormals[(int)NDIR.LEFT] = Vector3.left;
        allNormals[(int)NDIR.RIGHT] = Vector3.right;
        allNormals[(int)NDIR.FRONT] = Vector3.forward;
        allNormals[(int)NDIR.BACK] = Vector3.back;
    }
    #endregion

    #region Builtin Methods
    // Use this for initialization
    void Start ()
    {
        SetUp();

        //build chunk here
        //GameObject c = Instantiate(chunkPrefab, this.transform.position, this.transform.rotation);
        //c.GetComponent<Chunk>().CreateChunk(cSize, cHeight, cSize);

        //BuildChunkAt(transform.position);

        StartCoroutine(BuildChunkColumn());
    }

    // Update is called once per frame
    void Update ()
	{

	}
    #endregion
}
