using UnityEngine;

namespace Game
{
    [System.Serializable]
    public struct Item 
    {
        public readonly Transform transform;
        public readonly int ID;

        public Item(Transform transform, int id)
        {
            this.transform = transform;
            this.ID = id;
        }
    }
}