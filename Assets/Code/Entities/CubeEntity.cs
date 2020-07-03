using Components;
using Structs;
using Unity.Entities;
using UnityEngine;

namespace Entities
{

    public class CubeEntity : MonoBehaviour, IConvertGameObjectToEntity
    {
        [SerializeField] private CubeType cubeType;
        
        [SerializeField] private bool isCollected;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponentData(entity, new CubeData {CubeType = cubeType, IsCollected = isCollected});
        }
    }
}