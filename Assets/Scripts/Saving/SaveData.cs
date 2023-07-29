using System;
using System.Collections.Generic;
using UnityEngine;
using Task.Helper;

[Serializable]
public class SaveData
{
    public Dictionary<string, Item> blockInfos = new Dictionary<string, Item>();
    public List<string> keyValues = new List<string>();
    public List<int> triangles = new List<int>();
    public List<Helpers.SaveableVector3> vertices = new List<Helpers.SaveableVector3>();
    public List<Helpers.SaveableVector2> uvVertices = new List<Helpers.SaveableVector2>();
  
}
