using System;
using UnityEngine;

namespace Task.Helper
{
    [Serializable]
    public class Item
    {
        int verticesIndex;
        ObjectType objectType;
        public Item( int verticesIndex, ObjectType objectType)
        {
            this.objectType = objectType;
            this.verticesIndex = verticesIndex;
        }

        public int VerticesIndex { get => verticesIndex; set => verticesIndex = value; }
        public ObjectType ObjectType { get => objectType;}

       
    }
}

