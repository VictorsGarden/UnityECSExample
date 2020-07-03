namespace Systems
{
    // [UpdateAfter(typeof(EndFramePhysicsSystem))]
    // public class WallTriggerSystem : JobComponentSystem
    // {
    //     private BuildPhysicsWorld buildPhysicsWorld;
    //     private StepPhysicsWorld  stepPhysicsWorld;
    //
    //     protected override void OnCreate()
    //     {
    //         base.OnCreate();
    //
    //         buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
    //         stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    //     }
    //
    //     [BurstCompile]
    //     private struct WallTriggerSystemJob : ITriggerEventsJob
    //     {
    //         [ReadOnly] public ComponentDataFromEntity<WallData> WallGroup;
    //
    //         public ComponentDataFromEntity<CubeData> CubeGroup;
    //         // public ComponentDataFromEntity<PhysicsVelocity>      PhysicsVelocityGroup;
    //
    //         public void Execute(TriggerEvent triggerEvent)
    //         {
    //             Entity entityA = triggerEvent.Entities.EntityA;
    //             Entity entityB = triggerEvent.Entities.EntityB;
    //
    //             var isBodyATrigger = WallGroup.Exists(entityA);
    //             var isBodyBTrigger = WallGroup.Exists(entityB);
    //
    //             if (isBodyATrigger && isBodyBTrigger)
    //                 return;
    //
    //             // var isBodyADynamic = PhysicsVelocityGroup.Exists(entityA);
    //             // var isBodyBDynamic = PhysicsVelocityGroup.Exists(entityB);
    //
    //             var isBodyACube = CubeGroup.Exists(entityA);
    //             var isBodyBCube = CubeGroup.Exists(entityB);
    //
    //             if ((isBodyATrigger && !isBodyACube) ||
    //                 (isBodyBTrigger && !isBodyACube))
    //                 return;
    //
    //             var triggerEntity = isBodyATrigger ? entityA : entityB;
    //             var dynamicEntity = isBodyATrigger ? entityB : entityA;
    //
    //             
    //         }
    //     }
    //
    //     protected override JobHandle OnUpdate(JobHandle inputDeps)
    //     {
    //         var job = new WallTriggerSystemJob
    //         {
    //             WallGroup = GetComponentDataFromEntity<WallData>(true),
    //             CubeGroup = GetComponentDataFromEntity<CubeData>(),
    //             // PhysicsVelocityGroup = GetComponentDataFromEntity<PhysicsVelocity>()
    //         };
    //
    //         JobHandle jobHandle = job.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);
    //
    //         return jobHandle;
    //     }
    // }
}

// var horRange = new Vector2(-9.5f, 9.5f);
// var vertRange = new Vector2(-14.5f, 14.5f);
//     
// var isCollidesLeft = DetectLimitsLeft(translation.Value);
// var isCollidesRight = DetectLimitsRight(translation.Value);
// var isCollidesUp = DetectLimitsUp(translation.Value);
// var isCollidesDown = DetectLimitsDown(translation.Value);
//     
// bool DetectLimitsLeft(float3 translationValue)
// {
// return translationValue.x <= horRange.x;
// }
//     
// bool DetectLimitsRight(float3 translationValue)
// {
// return translationValue.x >= horRange.y;
// }
//     
// bool DetectLimitsUp(float3 translationValue)
// {
// return translationValue.z >= vertRange.y;
// }
//     
// bool DetectLimitsDown(float3 translationValue)
// {
// return translationValue.z <= vertRange.x;
// }
//     
// if (isCollidesLeft || isCollidesRight || isCollidesUp || isCollidesDown)
// {
// var newDirection = Vector3.one;
//     
//     if (isCollidesLeft)
// {
//     newDirection = Vector3.Reflect(cubeData.Direction, Vector3.right);
// }
//     
// if (isCollidesRight)
// {
//     newDirection = Vector3.Reflect(cubeData.Direction, -Vector3.right);
// }
//     
// if (isCollidesUp)
// {
//     newDirection = Vector3.Reflect(cubeData.Direction, Vector3.up);
// }
//     
// if (isCollidesDown)
// {
//     newDirection = Vector3.Reflect(cubeData.Direction, Vector3.down);
// }
//     
// cubeData.Direction = new Vector3(newDirection.x, cubeData.Direction.y, newDirection.z);
// cubeData.Acceleration = .5f;
// cubeData.IsReflected = true;
// }