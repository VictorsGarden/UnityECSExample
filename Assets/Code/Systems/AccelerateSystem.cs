using Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Systems
{
    public class AccelerateSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var queryTrans = GetEntityQuery(typeof(MagnetTag), typeof(LocalToWorld));

            var magnetTransformArr = queryTrans.ToComponentDataArray<LocalToWorld>(Allocator.TempJob);
            var magnetTransform = magnetTransformArr[0];

            var magnetDataArr = queryTrans.ToComponentDataArray<MagnetTag>(Allocator.TempJob);
            var magnetData = magnetDataArr[0];

            var isMouseHolds = Input.GetMouseButton(0);

            Entities.ForEach((ref Translation cubePos, ref LocalToWorld cubeTrans, ref CubeData cubeData) =>
            {
                var distance = math.distance(magnetTransform.Position, cubePos.Value);

                var accelerationFactor = 5f;

                if (distance <= 5f && !cubeData.IsReflected && !cubeData.IsCollected)
                {
                    var newDirection = cubePos.Value - magnetTransform.Position;
                    var direction = math.normalize(new float3(newDirection.x, cubePos.Value.y, newDirection.z));

                    cubeData.Direction = direction;

                    var force = magnetData.MouseDelta.magnitude;

                    if (isMouseHolds)
                    {
                        var accelerationRaw = accelerationFactor * force * magnetData.DeltaTime;
                        cubeData.Acceleration = Mathf.Clamp(accelerationRaw, 0, .3f);
                        cubeData.IsAccelerated = true;
                    }
                    else
                    {
                        var newPosition = magnetTransform.Position + direction * 11f; // last value is radius
                        var newPositionXZ = new Vector3(newPosition.x, cubePos.Value.y, newPosition.z);

                        force = 5f;
                        cubePos.Value = Vector3.Lerp(cubePos.Value, newPositionXZ, magnetData.DeltaTime * force);
                    }
                }
            }).WithBurst().Schedule();

            CompleteDependency();

            magnetTransformArr.Dispose();
            magnetDataArr.Dispose();
        }
    }
}