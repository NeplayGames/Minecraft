using System;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    public static string GenerateGenericKey(int x, int y, int z)
    {
        return " " + x + " " + y + " " + z;
    }
    public static string GenerateGenericKey(Vector3 keyVector)
    {
        return " " + keyVector.x + " " + keyVector.y + " " + keyVector.z;
    }
    [Serializable]
    public class SaveableVector3
    {
        public SaveableVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public float x;
        public float y;
        public float z;
    }
    [Serializable]
    public class SaveableVector2
    {
        public SaveableVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
           
        }
        public float x;
        public float y;

    }

    public static List<Vector3> ReturnVector3(List<SaveableVector3> vector3s)
    {
        List<Vector3> returnList = new List<Vector3>();
        foreach (SaveableVector3 vector3 in vector3s)
            returnList.Add(new Vector3(vector3.x, vector3.y, vector3.z));
        return returnList;
    }
    public static List<Vector2> ReturnVector2(List<SaveableVector2> vector2s)
    {
        List<Vector2> returnList = new List<Vector2>();
        foreach (SaveableVector2 vector2 in vector2s)
            returnList.Add(new Vector3(vector2.x, vector2.y));
        return returnList;
    }
    public static List<SaveableVector3> ReturnSaveableVector3(List<Vector3> vector3s)
    {
        List<SaveableVector3> returnList = new List<SaveableVector3>();
        foreach (Vector3 vector3 in vector3s)
            returnList.Add(new SaveableVector3(vector3.x, vector3.y, vector3.z));
        return returnList;
    }
    public static List<SaveableVector2> ReturnSaveableVector2(List<Vector2> vector3s)
    {
        List<SaveableVector2> returnList = new List<SaveableVector2>();
        foreach (Vector2 vector3 in vector3s)
            returnList.Add(new SaveableVector2(vector3.x, vector3.y));
        return returnList;
    }
}
