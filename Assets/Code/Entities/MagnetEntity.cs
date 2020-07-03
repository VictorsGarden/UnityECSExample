using Components;
using Unity.Entities;
using UnityEngine;

namespace Entities
{
    public class MagnetEntity : MonoBehaviour, IConvertGameObjectToEntity
    {
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponent(entity, typeof(MagnetTag));
        }
    }
}