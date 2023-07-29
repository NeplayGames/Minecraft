using System;
using System.Collections.Generic;
using Task.Helper;
using UnityEngine;

public class LoadAndSaveSystem : MonoBehaviour
{
    public Dictionary<string, Item> blockInfos = new Dictionary<string, Item>();
    public List<string> keyValues = new List<string>();
    public List<int> triangles = new List<int>();
    public List<Helpers.SaveableVector3> vertices = new List<Helpers.SaveableVector3>();
    public List<Helpers.SaveableVector2> uv = new List<Helpers.SaveableVector2>();
    public object CaptureState()
    {
        return new SaveData
        {
            blockInfos = this.blockInfos,
            keyValues = this.keyValues,
            triangles = this.triangles,
            vertices = this.vertices,
            uvVertices = this.uv,
        };

    }

    public void RestoreState(SaveData state)
    {
        SaveData saveData = state;
        blockInfos = saveData.blockInfos;
        keyValues = saveData.keyValues;
        triangles = saveData.triangles;
        vertices = saveData.vertices;
        uv = saveData.uvVertices;
    }

    

}
