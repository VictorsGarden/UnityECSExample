using Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    public class CubesMovingSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var queryTrans = GetEntityQuery(typeof(MagnetTag), typeof(LocalToWorld));
            var magnetDataArr = queryTrans.ToComponentDataArray<MagnetTag>(Allocator.TempJob);
            var magnetData = magnetDataArr[0];

            float yPos = 0.5f;

            Entities
                .ForEach((ref Translation cubePos, ref CubeData cubeData) =>
                {
                    var velocity = cubeData.Direction * cubeData.Acceleration;
                    var newPoint = cubePos.Value + velocity;
                    cubePos.Value = new float3(newPoint.x, yPos, newPoint.z);

                    cubePos.Value = math.lerp(cubePos.Value, newPoint, .5f);

                    if (cubeData.Acceleration > 0f)
                    {
                        cubeData.Acceleration -= .5f * magnetData.DeltaTime;
                    }

                    if (cubeData.Acceleration <= 0.05f)
                    {
                        cubeData.Acceleration = 0;
                        cubeData.IsAccelerated = false;
                        cubeData.IsReflected = false;
                    }
                }).WithBurst().Schedule();

            magnetDataArr.Dispose();
            CompleteDependency();
        }
    }
}