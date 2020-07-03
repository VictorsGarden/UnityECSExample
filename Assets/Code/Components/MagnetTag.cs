using System;
using Unity.Entities;
using Vector3 = UnityEngine.Vector3;

namespace Components
{
    [Serializable]
    public struct MagnetTag : IComponentData
    {
        public Vector3 MouseDelta;
        public float   DeltaTime;
    }
}