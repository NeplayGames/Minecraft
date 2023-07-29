using System.Collections.Generic;
using Task.Helper;
using UnityEngine;


public class WorldManager : MonoBehaviour
{
    [SerializeField] private int length;
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private SavingSystem savingSystem;
    private Dictionary<string, Item> blockInfos = new Dictionary<string, Item>();
    private List<string> keyValues = new List<string>();
    private List<int> triangles = new List<int>();
    private List<Vector3> vertices = new List<Vector3>();
    private List<Vector2> uvVertices = new List<Vector2>();
    private MeshFilter meshFilter;
    private MeshCollider meshCollider;

    private int[,] faces = new int[6, 4]{
                        {0, 1, 2, 3 },     //top
                        {7, 6, 5, 4 },   //bottom
                        {2, 1, 5, 6},     //right
                        {0, 3, 7, 4},   //left
                        {3, 2, 6, 7},    //front
                        {1, 0, 4, 5}    //back
                    };
    private Vector3[] vertPos = new Vector3[8]{
                        new Vector3(-1, 1, -1), new Vector3(-1, 1, 1),
                        new Vector3(1, 1, 1), new Vector3(1, 1, -1),
                        new Vector3(-1, -1, -1), new Vector3(-1, -1, 1),
                        new Vector3(1, -1, 1), new Vector3(1, -1, -1),
                    };

    private void Awake()
    {
        InitialValue();
        SaveData data = savingSystem.LoadFile();
        if (data == null)      
            GenerateMesh();       
        else       
            RestoreState(data);              
    }

    private void InitialValue()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        meshFilter.mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        meshCollider.sharedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;     
    }

    private void GenerateMesh()
    {
        for (int x = 0; x < length; x++)
            for (int y = 0; y < height; y++)
                for (int z = 0; z < width; z++)
                {
                    int a = Random.Range(0, 3);
                    AddBlock(x, y, z, a);
                }
        SetMeshInitially();
    }

    private void SetMeshInitially()
    {
        Mesh mesh = new Mesh()
        {
            vertices = vertices.ToArray(),
            triangles = triangles.ToArray(),
            uv = uvVertices.ToArray()

        };
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
    }

    public void CaptureState()
    {
        SaveData data = new SaveData
        {
            blockInfos = this.blockInfos,
            keyValues = this.keyValues,
            triangles = this.triangles,
            vertices = Helpers.ReturnSaveableVector3( this.vertices),
            uvVertices = Helpers.ReturnSaveableVector2(this.uvVertices),
        };
        savingSystem.SaveFile(data);

    }

    public void RestoreState(SaveData saveData)
    {
        this.blockInfos = saveData.blockInfos;
        this.keyValues = saveData.keyValues;
        this.triangles = saveData.triangles;
        this.vertices = Helpers.ReturnVector3(saveData.vertices);
        this.uvVertices = Helpers.ReturnVector2(saveData.uvVertices);
        SetMeshInitially();
    }


    public bool AddBlockOnPoint(int x, int y, int z, int a)
    {
        bool isAdded =  AddBlock(x, y, z,a);
        if(isAdded)
            RecreateMesh();
        return isAdded;
    }

    /// <summary>
    /// This function recreates the mesh using modified vertices and the indices
    /// </summary>
    private void RecreateMesh()
    {
        meshFilter.mesh.Clear();
        Mesh mesh = new Mesh()
        {
            vertices = vertices.ToArray(),
            triangles = triangles.ToArray(),
            uv = uvVertices.ToArray()

        };
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh.Clear();
        meshCollider.sharedMesh = mesh;
    }

    public Item RemoveBlock( string key)
    {    
        if (blockInfos.ContainsKey(key))
        {
           Item item = RemoveEachBlock(key);         
            RecreateMesh();
            return item;
        }
        return null;
    }

    private bool AddBlock(int x, int y, int z,int intValueOfObjectType)
    {
        string key = Helpers.GenerateGenericKey(x, y, z);
        if (blockInfos.ContainsKey(key))
            return false;
        keyValues.Add(key);
        Vector2 textureVector;
        ObjectType objectType;
        DetermineTextVectorAndObjectType(intValueOfObjectType, out textureVector, out objectType);
        Item blockInfo = new Item(vertices.Count, objectType);
        blockInfos.Add(key, blockInfo);
        for (int facenum = 0; facenum < 6; facenum++)
        {
            int v = vertices.Count;
            for (int i = 0; i < 4; i++) 
                vertices.Add(new Vector3(x, y, z) + vertPos[faces[facenum, i]] / 2);
            triangles.AddRange(new List<int>() { v, v + 1, v + 2, v, v + 2, v + 3 });
            uvVertices.AddRange(new List<Vector2>() { textureVector + new Vector2(0, 0.5f), textureVector + new Vector2(0.5f, 0.5f), textureVector + new Vector2(0.5f, 0), textureVector });
        }
        return true;
    }

    /// <summary>
    /// This function is determines the texture to use from the sprite as well as the object type.
    /// </summary>
    /// <param name="intValueOfObjectIndex"></param>
    /// <param name="textureVector"></param>
    /// <param name="objectType"></param>
    private void DetermineTextVectorAndObjectType(int intValueOfObjectIndex, out Vector2 textureVector, out ObjectType objectType)
    {
        textureVector = new Vector2(0, 0) / 2f;
        objectType = ObjectType.Red;
        switch (intValueOfObjectIndex)
        {
            case 0:
                objectType = ObjectType.Pink;
                textureVector = new Vector2(0, 0) / 2f;
                break;
            case 1:
                objectType = ObjectType.Green;
                textureVector = new Vector2(0, 1) / 2f;
                break;
            case 2:
                objectType = ObjectType.Red;
                textureVector = new Vector2(1, 1) / 2f;
                break;
        }
    }

    public Item RemoveEachBlock(string key)
    {
        Item item = blockInfos[key];
        int verticesIndex = item.VerticesIndex;
        int index = keyValues.IndexOf(key);
        for (int i = index; i < keyValues.Count; i++)        
            blockInfos[keyValues[i]].VerticesIndex -= 24;
        int triangleIndex = triangles.Count - 36;
        triangles.RemoveRange(triangleIndex, 36);        
        vertices.RemoveRange(verticesIndex, 24);
        uvVertices.RemoveRange(verticesIndex, 24);
        blockInfos.Remove(key);
        keyValues.Remove(key);
        return item;
    }
}
