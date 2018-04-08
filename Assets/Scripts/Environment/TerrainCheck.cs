using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainCheck : MonoBehaviour
{
   // private Texture texture;
    [SerializeField] private List<Texture> rockTextures;
    [SerializeField] private List<Texture> sandTextures;
    [SerializeField] private List<Texture> iceTextures;

    public List <Texture> RockTexture { get { return rockTextures; } }
    public List<Texture> SandTexture { get { return sandTextures; } }
    public List<Texture> IceTexture { get { return iceTextures; } }
    private Vector3 TS, terrainPosition; // terrain size
    private Vector2 AS; // control texture size
    private int AX, AY;
    private Terrain previousTerrain;

    public Texture GetTerrainTextureAt(Vector3 position, Terrain currentTerrain)
    {
        Texture retval = new Texture();
        TerrainData TD = currentTerrain.terrainData;

        TS = TD.size;
        terrainPosition = currentTerrain.transform.position;
        AS.x = TD.alphamapWidth;
        AS.y = TD.alphamapHeight;

        if (position.x > 0)
        {
            AX = (int)(((position.x % TS.x) / TS.x) * AS.x + 0.5f);
        }
        else
        {
            AX = (int)(((position.x % TS.x) / TS.x) * AS.x + 0.5f) + (int)(TS.x / AS.x);
        }

        if (position.z > 0)
        {
            AY = (int)(((position.z % TS.z) / TS.z) * AS.y + 0.5f);
        }
        else
        {
            AY = (int)(((position.z % TS.z) / TS.z) * AS.y + 0.5f) + (int)(TS.z / AS.y);
        }

        float[,,] TerrCntrl = TD.GetAlphamaps(AX, AY, 1, 1);


        for (int i = 0; i < TD.splatPrototypes.Length; i++)
        {
            if (TerrCntrl[0, 0, i] > .5f)
            {
                retval = TD.splatPrototypes[i].texture;
            }
        }

        return retval;
    }
}
