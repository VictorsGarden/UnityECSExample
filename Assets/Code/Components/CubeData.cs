using Structs;
using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    [GenerateAuthoringComponent]
    public struct CubeData : IComponentData
    {
        public CubeType CubeType;
        public float3   Direction;
        public float3   StickingPoint;
        public float    Acceleration;
        public bool     IsAccelerated;
        public bool     IsReflected;
        public bool     IsCollected;
    }
}