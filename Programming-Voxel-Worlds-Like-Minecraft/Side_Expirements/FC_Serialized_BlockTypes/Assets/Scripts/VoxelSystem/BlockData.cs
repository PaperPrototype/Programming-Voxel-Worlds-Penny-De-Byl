using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newBlock", menuName = "VoxelWorld/NewBlock")]
public class BlockData : ScriptableObject
{
	[SerializeField]
    public enum TextureType { DIRT, STONE, SAND, AIR }

	//TextureType

	private Vector2[,] blockUVs = { 
		/*GRASS TOP*/		{new Vector2( 0.125f, 0.375f ), new Vector2( 0.1875f, 0.375f),
								new Vector2( 0.125f, 0.4375f ), new Vector2( 0.1875f, 0.4375f )},
		/*GRASS SIDE*/		{new Vector2( 0.1875f, 0.9375f ), new Vector2( 0.25f, 0.9375f),
								new Vector2( 0.1875f, 1.0f ), new Vector2( 0.25f, 1.0f )},
		/*DIRT*/			{new Vector2( 0.125f, 0.9375f ), new Vector2( 0.1875f, 0.9375f),
								new Vector2( 0.125f, 1.0f ), new Vector2( 0.1875f, 1.0f )},
		/*STONE*/			{new Vector2( 0f, 0.875f ), new Vector2( 0.0625f, 0.875f),
								new Vector2( 0, 0.9375f ) ,new Vector2( 0.0625f, 0.9375f )},
		/*SAND*/            {new Vector2( 0f, 0.25f), new Vector2(0.0625f, 0.25f),
								new Vector2(0.0625f, 0.3125f), new Vector2(0f, 0.3125f)}
						};
}
