#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[InitializeOnLoad]
public class IRPUtils
{

    static UnityEvent<InstantRoadPath, bool> thisEvent = null;
    class IRPEvent : UnityEvent<InstantRoadPath, bool>
    {
    }
    public static void IRPlistener(UnityAction<InstantRoadPath, bool> listener)
    {
        if (thisEvent == null)
            thisEvent = new IRPEvent();
        thisEvent.AddListener(listener);
    }
    static IRPUtils()
    {
        InstantRoadPath.regenerateTerrain += reSplat;
        InstantRoadPath.requestSmoothing += giveSmoothing;
    }
    static void giveSmoothing(InstantRoadPath IRP, float res = 7)
    {

        IRP.PMiddle = null;
        IRP.PSide1 = null;
        IRP.PSide2 = null;
        if (IRP.irpPath == null) return;
        Vector3[] Points = new Vector3[IRP.irpPath.Length];
        Terrain ter = IRP.GetComponent<Terrain>();
        float scale = Mathf.Max(ter.terrainData.heightmapScale.x, ter.terrainData.heightmapScale.z);
        for (int ip = 0; ip < IRP.irpPath.Length; ip++)
        {
            Points[ip] = IRP.irpPath[ip].Point;
        }
        TagussanBSpline bspline = new TagussanBSpline(Points, 4);
        float iter = (IRP.irpPath.Length * IRP.roadWidth * res / scale);
        //IRP.irpPath = null;
        IRP.path2 = new Vector3[Mathf.CeilToInt(iter)];
        int step = (int)(8 / scale);
        List<Vector3> PMiddle = new List<Vector3>();
        int lasti = 0;
        for (int ip = 0; ip < iter; ip++)
        {
            float[] xyz = bspline.calcAt(ip / iter);
            IRP.path2[ip] = new Vector3(xyz[0], xyz[1], xyz[2]);
            if (lasti < ip)
            {
                PMiddle.Add(IRP.path2[ip]);//untuk visualisasi
                lasti = ip + step;
            }
        }
        IRP.PMiddle = PMiddle.ToArray();
        //dari sini kebawah untuk visualisasi
        int visIdx = 0;
        float curWidth = IRP.roadWidth * .6f;
        IRP.PSide1 = new Vector3[Mathf.CeilToInt(iter / step)];
        IRP.PSide2 = new Vector3[Mathf.CeilToInt(iter / step)]; visIdx = 0;
        for (int i = 1; i < IRP.path2.Length; i += step)
        {
            Vector2 dir = (Vector3toXZ(IRP.path2[i]) - IRPUtils.Vector3toXZ(IRP.path2[i - 1]));
            float angle = Mathf.Atan2(dir.y, dir.x) + (Mathf.Deg2Rad * 90);
            for (float iSide = 0; iSide <= curWidth; iSide += curWidth)
            {
                Vector3 vector = new Vector3(IRP.path2[i - 1].x + (iSide - curWidth / 2f) * Mathf.Cos(angle),
                          IRP.path2[i - 1].y, IRP.path2[i - 1].z + (iSide - curWidth / 2f) * Mathf.Sin(angle));
                if (iSide == 0) IRP.PSide1[visIdx] = vector;
                if (iSide == curWidth) IRP.PSide2[visIdx] = vector;
            }
            visIdx++;
        }
        //Uncoment this to visualize prepared markers for Easy Road 3d
        //ERConstructor.GenerateERMarkers(IRP,true);
    }

    static void reSplat(InstantRoadPath IRP)
    {
        float curWidth = IRP.roadWidth * (IRP.CutMode == 0 ? 2 : 1.6f);
        Terrain ter = IRP.GetComponent<Terrain>();
        TerrainData terrainData = ter.terrainData;
        int xSize = ter.terrainData.heightmapWidth;
        int zSize = ter.terrainData.heightmapHeight;
        float hmScale = 1025 / xSize;
        float[,] heights = ter.terrainData.GetHeights(0, 0, xSize, zSize);
        if (IRP.path2 == null) giveSmoothing(IRP);
        if (IRP.path2 == null)
        {
            EditorUtility.DisplayDialog("", "Failed", "Ok");
            return;
        }
        float muply = 2f / Mathf.Max(ter.terrainData.heightmapScale.x, ter.terrainData.heightmapScale.z);
        int muply2 = 1;
        for (int i = IRP.path2.Length - 1; i > muply2 + 1; i -= muply2)
        {
            if (Vector3.Distance(IRP.path2[i - 1], IRP.path2[i]) < .03f * hmScale) continue;
            Vector2 dir = (Vector3toXZ(IRP.path2[i - 1]) - Vector3toXZ(IRP.path2[i]));
            float angle = Mathf.Atan2(dir.y, dir.x) + (Mathf.Deg2Rad * 90);
            float actHeight = 0;
            for (int iSide = 1; iSide <= curWidth * muply; iSide++)
            {
                int adapX = Mathf.RoundToInt((IRP.path2[i - 1].x - IRP.transform.position.x + (iSide / muply - curWidth / 2f) * Mathf.Cos(angle)) / ter.terrainData.heightmapScale.x);
                int adapZ = Mathf.RoundToInt((IRP.path2[i - 1].z - IRP.transform.position.z + (iSide / muply - curWidth / 2f) * Mathf.Sin(angle)) / ter.terrainData.heightmapScale.z);
                adapX = Mathf.Clamp(adapX, 0, heights.GetLength(0) - 1);
                adapZ = Mathf.Clamp(adapZ, 0, heights.GetLength(1) - 1);
                if (IRP.CutMode == 0)
                    actHeight = ter.terrainData.GetHeight(adapX, adapZ);
                else if (IRP.CutMode == 1)
                    actHeight = heights[adapZ, adapX] * ter.terrainData.heightmapScale.y;
                float pengurang = Mathf.Abs(iSide / muply - (curWidth / 2f)) / (curWidth / 2f);
                heights[adapZ, adapX] = (IRP.path2[i - 1].y - (IRP.path2[i - 1].y - actHeight) * (Mathf.Pow(pengurang, 2.5f))) / ter.terrainData.heightmapScale.y;
            }
            muply2 = UnityEngine.Random.Range(1, 2);
        }

        bool[,] TextureMarks = new bool[heights.GetLength(0), heights.GetLength(1)];
        bool[,] DetailMarks = new bool[heights.GetLength(0), heights.GetLength(1)];
        curWidth = IRP.roadWidth * .7f;
        muply *= 1f;
        for (int i = IRP.path2.Length - 1; i > 1; i--)
        {
            if (Vector3.Distance(IRP.path2[i - 1], IRP.path2[i]) < .03f * hmScale) continue;
            Vector2 dir = (Vector3toXZ(IRP.path2[i - 1]) - Vector3toXZ(IRP.path2[i]));
            float angle = Mathf.Atan2(dir.y, dir.x) + (Mathf.Deg2Rad * 90);
            for (int iSide = 1; iSide <= curWidth * muply; iSide++)
            {
                int xHeight = Mathf.RoundToInt((IRP.path2[i - 1].x - IRP.transform.position.x + (iSide / muply - curWidth / 2f) * Mathf.Cos(angle)) / ter.terrainData.heightmapScale.x);
                int yHeight = Mathf.RoundToInt((IRP.path2[i - 1].z - IRP.transform.position.z + (iSide / muply - curWidth / 2f) * Mathf.Sin(angle)) / ter.terrainData.heightmapScale.z);
                heights[yHeight, xHeight] = (IRP.path2[i - 1].y / ter.terrainData.heightmapScale.y + heights[yHeight, xHeight]) / 2;
                DetailMarks[xHeight, yHeight] = true;
                //texturing
                if (iSide > 2 && iSide <= curWidth * muply - 2)
                {
                    TextureMarks[xHeight, yHeight] = true;
                }
            }
        }

        int HeightMapX = terrainData.heightmapWidth;
        int HeightMapY = terrainData.heightmapHeight;
        int[,][] treePos = new int[HeightMapX, HeightMapY][];
        if (IRP.removeTree)
        {
            EditorUtility.DisplayProgressBar("Instant Road Path", "Collecting Trees", .25f);
            for (int i = 0; i < terrainData.treeInstances.Length; i++)
            {
                Vector3 tempPos = terrainData.GetTreeInstance(i).position;
                int x = (int)(tempPos.x * HeightMapX);
                int y = (int)(tempPos.z * HeightMapY);
                if (treePos[x, y] == null) treePos[x, y] = new int[5];
                if (treePos[x, y][treePos[x, y].Length - 1] != 0)
                    Array.Resize(ref treePos[x, y], treePos[x, y].Length + 1);
                treePos[x, y][treePos[x, y].Length - 1] = (i == 0 ? -1 : i);
            }
        }

        EditorUtility.DisplayProgressBar("Instant Road Path", "Recreating detail", .45f);
        float[,,] TexMaps = terrainData.GetAlphamaps(0, 0, terrainData.alphamapWidth, terrainData.alphamapHeight);
        bool[,] ClearGrassMap = new bool[terrainData.detailWidth, terrainData.detailHeight];
        bool[] ClearTreeMap = new bool[terrainData.treeInstances.Length];
        int scaleX = Mathf.RoundToInt(terrainData.detailWidth / HeightMapY);//xy Untuk Tekstur & Grass kebalikan dari HeightMap 
        int scaleY = Mathf.RoundToInt(terrainData.detailHeight / HeightMapX);//xy Untuk Tekstur & Grass kebalikan dari HeightMap 
        int TreePosScaleX = Mathf.RoundToInt(terrainData.heightmapScale.x);
        int TreePosScaleY = Mathf.RoundToInt(terrainData.heightmapScale.z);
        int textScaleX = Mathf.CeilToInt(terrainData.alphamapWidth / (float)HeightMapY);// xy Untuk Tekstur & Grass kebalikan dari HeightMap  
        int textScaleY = Mathf.CeilToInt(terrainData.alphamapHeight / (float)HeightMapX);// xy Untuk Tekstur & Grass kebalikan dari HeightMap 
        for (int xHeight = 0; xHeight < HeightMapX; xHeight++)
            for (int yHeight = 0; yHeight < HeightMapY; yHeight++)
            {
                int posy, posx;
                if (TextureMarks[xHeight, yHeight])
                    if (IRP.ReTexture)
                    {
                        posy = Mathf.RoundToInt(xHeight / (float)HeightMapX * terrainData.alphamapHeight);// xy Untuk Tekstur & Grass kebalikan dari HeightMap 
                        posx = Mathf.RoundToInt(yHeight / (float)HeightMapY * terrainData.alphamapWidth);// xy Untuk Tekstur & Grass kebalikan dari HeightMap  
                        for (int x = posx - textScaleX; x < posx + textScaleX; x++)
                            for (int y = posy - textScaleY; y < posy + textScaleY; y++)
                                for (int sPro = 0; sPro < terrainData.splatPrototypes.Length; sPro++)
                                {
                                    if (sPro == IRP.selTexture)
                                        TexMaps[x, y, sPro] = 1;
                                    else
                                        TexMaps[x, y, sPro] = 0;
                                }
                    }
                if (!DetailMarks[xHeight, yHeight]) continue;
                if (IRP.removeGrass)
                {
                    posy = Mathf.RoundToInt(xHeight / (float)HeightMapX * terrainData.detailHeight);// xy Untuk Tekstur & Grass kebalikan dari HeightMap 
                    posx = Mathf.RoundToInt(yHeight / (float)HeightMapY * terrainData.detailWidth);// xy Untuk Tekstur & Grass kebalikan dari HeightMap  
                    for (int x = posx - scaleX - 2; x < posx + scaleX + 2; x++)
                        for (int y = posy - scaleY - 2; y < posy + scaleY + 2; y++)
                        {
                            if (x < 0 || y < 0 || x >= terrainData.detailWidth || y >= terrainData.detailHeight) continue;
                            ClearGrassMap[x, y] = true;
                        }
                }
                if (IRP.removeTree)
                    for (posx = xHeight - TreePosScaleX - 1; posx <= xHeight + TreePosScaleX; posx++)
                    {
                        for (posy = yHeight - TreePosScaleY - 1; posy <= yHeight + TreePosScaleY; posy++)
                        {
                            if (posx < 0 || posy < 0 || posx >= HeightMapX || posy >= HeightMapY) continue;
                            if (treePos[posx, posy] != null)
                                foreach (int treeIdx in treePos[posx, posy])
                                {
                                    if (treeIdx != 0)
                                        ClearTreeMap[(treeIdx == -1 ? 0 : treeIdx)] = true;
                                }
                        }
                    }
            }
        if (IRP.removeTree)
        {
            List<TreeInstance> newTreeInstance = new List<TreeInstance>();
            for (int treeIdx = 0; treeIdx < terrainData.treeInstances.Length; treeIdx++)
            {
                if (!ClearTreeMap[treeIdx])
                    newTreeInstance.Add(terrainData.GetTreeInstance(treeIdx));
            }
            terrainData.treeInstances = newTreeInstance.ToArray();
        }
        int[] ly = terrainData.GetSupportedLayers(0, 0, terrainData.detailWidth, terrainData.detailHeight);
        if (IRP.removeGrass)
            foreach (int item in ly)
            {
                int[,] map = terrainData.GetDetailLayer(0, 0, terrainData.detailWidth, terrainData.detailHeight, item);
                for (int x = 0; x < terrainData.detailWidth; x++)
                    for (int y = 0; y < terrainData.detailHeight; y++)
                    {
                        if (ClearGrassMap[x, y])
                            map[x, y] = 0;
                    }
                terrainData.SetDetailLayer(0, 0, item, map);
            }
        if (IRP.ReTexture)
        {
            Undo.RegisterCompleteObjectUndo(terrainData.alphamapTextures, "Construct");
            EditorUtility.DisplayProgressBar("Instant Road Path", "Applying texture", .8f);
            terrainData.SetAlphamaps(0, 0, TexMaps);
        }

        EditorUtility.DisplayProgressBar("Instant Road Path", "Applying new Heightmap", .1f);
        terrainData.SetHeights(0, 0, heights);//Disimpan di ujung supaya letak pohon ter-update

        EditorUtility.ClearProgressBar();

        if (InstantRoadPath.ERisready && IRP.portToER)
        {
            IRP.ERIndex++;
            if (thisEvent != null)
                thisEvent.Invoke(IRP, false);
        }
    }
    public static Vector2 Vector3toXZ(Vector3 vector)
    {
        return new Vector2(vector.x, vector.z);
    }

}
class MyAllPostprocessor : AssetPostprocessor
{
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        string[] guids = AssetDatabase.FindAssets("NewEasyRoads3D");
        if (guids.Length <= 0
            && File.Exists(Application.dataPath + @"/Instant Road Path/Scripts/ERConstructor.old"))
        {
            string filetochange = Application.dataPath + @"/Instant Road Path/Scripts/ERConstructor.cs";
            string result;
            result = Path.ChangeExtension(filetochange, ".pre");
            FileUtil.MoveFileOrDirectory(filetochange, result);
            filetochange = Application.dataPath + @"/Instant Road Path/Scripts/ERConstructor.old";
            result = Path.ChangeExtension(filetochange, ".cs");
            FileUtil.MoveFileOrDirectory(filetochange, result);
            AssetDatabase.Refresh();
            Debug.Log("kosongkan");
        }
        if (guids.Length > 0 && !File.Exists(Application.dataPath + @"/Instant Road Path/Scripts/ERConstructor.old"))
        {
            string filetochange = Application.dataPath + @"/Instant Road Path/Scripts/ERConstructor.cs";
            string result = Path.ChangeExtension(filetochange, ".old");
            FileUtil.MoveFileOrDirectory(filetochange, result);
            filetochange = Application.dataPath + @"/Instant Road Path/Scripts/ERConstructor.pre";
            result = Path.ChangeExtension(filetochange, ".cs");
            FileUtil.MoveFileOrDirectory(filetochange, result);
            AssetDatabase.Refresh();
            Debug.Log("isi");
        }
    }
}
#endif