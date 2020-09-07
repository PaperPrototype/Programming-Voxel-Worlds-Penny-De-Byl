using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CreateAssetMenu(fileName = "NewVoxelType", menuName = "Voxel/NewVoxelType")]
[Serializable]
public class VoxelType : ScriptableObject
{
    // use the name of the Object to
    // identify in code
    // unless... a serializable enum is possible

    public enum CurSide { TOP, SIDE, BOTTOM };

    private enum UVOptions { AUTO, MANUAL };

    [HideInInspector]
    public CurSide curSide;

    [SerializeField]
    private UVOptions uvOptions = UVOptions.AUTO;

    [Header("Base UV's (or Bottom UV's)")]

    [SerializeField]
    public Sprite b_atlasSprite;
    private Rect b_spriteRect;

    [SerializeField]
    private Vector2 b_uv1;
    [SerializeField]
    private Vector2 b_uv2;
    [SerializeField]
    private Vector2 b_uv3;
    [SerializeField]
    private Vector2 b_uv4;

    [Header("Optional Side UV's")]
    [SerializeField]
    public Sprite s_atlasSprite;
    private Rect s_spriteRect;

    [SerializeField]
    private Vector2 s_uv1;
    [SerializeField]
    private Vector2 s_uv2;
    [SerializeField]
    private Vector2 s_uv3;
    [SerializeField]
    private Vector2 s_uv4;

    [Header("Optional Top UV's")]
    [SerializeField]
    public Sprite t_atlasSprite;
    private Rect t_spriteRect;

    [SerializeField]
    private Vector2 t_uv1;
    [SerializeField]
    private Vector2 t_uv2;
    [SerializeField]
    private Vector2 t_uv3;
    [SerializeField]
    private Vector2 t_uv4;

    private void OnValidate()
    {
        if(uvOptions == UVOptions.AUTO)
        {
            if(b_atlasSprite)
            {
                // if the sprite is packed it will refernce the atlas
                // otherwise it will refernce the source sprite
                Texture texture = b_atlasSprite.texture;

                // Base uv Assignment
                b_spriteRect = b_atlasSprite.rect;

                float b_rectX = b_spriteRect.position.x;
                float b_rectY = b_spriteRect.position.y;

                float b_uv1x = b_rectX / (float)texture.width;
                float b_uv1y = b_rectY / (float)texture.height;
                float b_uv2x = (b_rectX + b_spriteRect.width) / (float)texture.width;
                float b_uv2y = b_rectY / (float)texture.height;
                float b_uv3x = b_rectX / (float)texture.width;
                float b_uv3y = (b_rectY + b_spriteRect.height) / (float)texture.height;
                float b_uv4x = (b_rectX + b_spriteRect.width) / (float)texture.width;
                float b_uv4y = (b_rectY + b_spriteRect.height) / (float)texture.height;

                b_uv1 = new Vector2(b_uv1x, b_uv1y);
                b_uv2 = new Vector2(b_uv2x, b_uv2y);
                b_uv3 = new Vector2(b_uv3x, b_uv3y);
                b_uv4 = new Vector2(b_uv4x, b_uv4y);

            }
            if (s_atlasSprite)
            {
                // if the sprite is packed it will refernce the atlas
                // otherwise it will refernce the source sprite
                Texture texture = b_atlasSprite.texture;

                // Side uv Assignments
                s_spriteRect = s_atlasSprite.rect;

                float rectX = s_spriteRect.position.x;
                float rectY = s_spriteRect.position.y;

                float uv1x = rectX / (float)texture.width;
                float uv1y = rectY / (float)texture.height;
                float uv2x = (rectX + s_spriteRect.width) / (float)texture.width;
                float uv2y = rectY / (float)texture.height;
                float uv3x = rectX / (float)texture.width;
                float uv3y = (rectY + s_spriteRect.height) / (float)texture.height;
                float uv4x = (rectX + s_spriteRect.width) / (float)texture.width;
                float uv4y = (rectY + s_spriteRect.height) / (float)texture.height;

                s_uv1 = new Vector2(uv1x, uv1y);
                s_uv2 = new Vector2(uv2x, uv2y);
                s_uv3 = new Vector2(uv3x, uv3y);
                s_uv4 = new Vector2(uv4x, uv4y);
            }
            if (t_atlasSprite)
            {
                // if the sprite is packed it will refernce the atlas
                // otherwise it will refernce the source sprite
                Texture texture = b_atlasSprite.texture;

                // Side uv Assignments
                t_spriteRect = t_atlasSprite.rect;

                float rectX = t_spriteRect.position.x;
                float rectY = t_spriteRect.position.y;

                float uv1x = rectX / (float)texture.width;
                float uv1y = rectY / (float)texture.height;
                float uv2x = (rectX + t_spriteRect.width) / (float)texture.width;
                float uv2y = rectY / (float)texture.height;
                float uv3x = rectX / (float)texture.width;
                float uv3y = (rectY + t_spriteRect.height) / (float)texture.height;
                float uv4x = (rectX + t_spriteRect.width) / (float)texture.width;
                float uv4y = (rectY + t_spriteRect.height) / (float)texture.height;

                t_uv1 = new Vector2(uv1x, uv1y);
                t_uv2 = new Vector2(uv2x, uv2y);
                t_uv3 = new Vector2(uv3x, uv3y);
                t_uv4 = new Vector2(uv4x, uv4y);
            }

        }
    }

    public Vector2 uv00(CurSide curSide)
    {
        if(curSide == CurSide.TOP && t_atlasSprite != null)
        {
            return t_uv1;
        }
        else if (curSide == CurSide.SIDE && s_atlasSprite != null)
        {
            return s_uv1;
        }
        else
        {
            return b_uv1;
        }
    }

    public Vector2 uv10(CurSide curSide)
    {
        if (curSide == CurSide.TOP && t_atlasSprite != null)
        {
            return t_uv2;
        }
        else if (curSide == CurSide.SIDE && s_atlasSprite != null)
        {
            return s_uv2;
        }
        else
        {
            return b_uv2;
        }
    }

    public Vector2 uv01(CurSide curSide)
    {
        if (curSide == CurSide.TOP && t_atlasSprite != null)
        {
            return t_uv3;
        }
        else if (curSide == CurSide.SIDE && s_atlasSprite != null)
        {
            return s_uv3;
        }
        else
        {
            return b_uv3;
        }
    }

    public Vector2 uv11(CurSide curSide)
    {
        if (curSide == CurSide.TOP && t_atlasSprite != null)
        {
            return t_uv4;
        }
        else if (curSide == CurSide.SIDE && s_atlasSprite != null)
        {
            return s_uv4;
        }
        else
        {
            return b_uv4;
        }
    }

    /*
    public void OnPostprocessTexture(Texture2D texture)
    {
        int colCount = 16;
        int rowCount = 16;
        int sw = texture.width / colCount;
        int sh = texture.height / rowCount;

        List<SpriteMetaData> metas = new List<SpriteMetaData>();

        for (int r = 0; r < rowCount; r++)
        {
            for (int c = 0; c < colCount; c++)
            {
                SpriteMetaData meta = new SpriteMetaData();
                meta.rect = new Rect(c * sw, r * sh, sw, sh);
                float uv1x = (c * sw) / (float)texture.width;
                float uv1y = (r * sh) / (float)texture.height;
                float uv2x = (c * sw + sw) / (float)texture.width;
                float uv2y = (r * sh) / (float)texture.height;
                float uv3x = (c * sw) / (float)texture.width;
                float uv3y = (r * sh + sh) / (float)texture.height;
                float uv4x = (c * sw + sw) / (float)texture.width;
                float uv4y = (r * sh + sh) / (float)texture.height;
                meta.name = uv1x + "," + uv1y + "|" +
                            uv2x + "," + uv2y + "|" +
                            uv3x + "," + uv3y + "|" +
                            uv4x + "," + uv4y;
                metas.Add(meta);
            }
        }

        TextureImporter textureImporter = (TextureImporter)assetImporter;
        textureImporter.spritesheet = metas.ToArray();
    }
    */
}
