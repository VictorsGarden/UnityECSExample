using Structs;
using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct FinishZoneData : IComponentData
    {
        public CubeType ZoneType;
        public int      MaxCount;
        public float2   Horizontal;
        public float2   Vertical;
        public bool     HasCollected;
    }
}