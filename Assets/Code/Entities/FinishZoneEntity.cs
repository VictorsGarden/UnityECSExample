using Components;
using Structs;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Entities
{
    public class FinishZoneEntity : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] private CubeType zoneType;
        [SerializeField] private int      maxCount;

        [SerializeField] private Transform upperLeft;
        [SerializeField] private Transform upperRight;
        [SerializeField] private Transform bottomLeft;
        [SerializeField] private Transform bottomRight;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity,
                new FinishZoneData
                {
                    ZoneType = zoneType,
                    Horizontal = new float2(upperLeft.position.x, upperRight.position.x),
                    Vertical = new float2(bottomLeft.position.z, upperLeft.position.z)
                });
        }
    }
}