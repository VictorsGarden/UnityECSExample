using Components;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    public class CheckFinishZoneSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            Entities.ForEach((ref FinishZoneData finishZoneData, ref Translation translation) =>
            {
                if (finishZoneData.HasCollected)
                {
                    Debug.Log(finishZoneData.ZoneType.ToString() + " COLLECTED!!!");
                }
                
            }).WithoutBurst().Run();
        }
    }
}