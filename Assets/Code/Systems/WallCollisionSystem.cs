using Components;
using Structs;
using Unity.Burst;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Collections;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static Unity.Mathematics.math;

namespace Systems
{
    [UpdateAfter(typeof(EndFramePhysicsSystem))]
    public class WallCollisionSystem : JobComponentSystem
    {
        private BuildPhysicsWorld buildPhysicsWorld;
        private StepPhysicsWorld  stepPhysicsWorld;
    
        protected override void OnCreate()
        {
            base.OnCreate();
    
            buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
            stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        }
    
        [BurstCompile]
        private struct WallCollisionSystemJob : ICollisionEventsJob
        {
            [ReadOnly] public ComponentDataFromEntity<WallData> wallGroup;
            public            ComponentDataFromEntity<CubeData> cubeGroup;
    
            public void Execute(CollisionEvent collisionEvent)
            {
                Entity entityA = collisionEvent.Entities.EntityA;
                Entity entityB = collisionEvent.Entities.EntityB;
    
                var aEntityIsWall = wallGroup.Exists(entityA);
                var bEntityIsWall = wallGroup.Exists(entityB);
    
                var aEntityIsCube = cubeGroup.Exists(entityA);
                var bEntityIsCube = cubeGroup.Exists(entityB);
    
                if (aEntityIsWall && bEntityIsCube)
                {
                    CubeData cubeData = cubeGroup[entityB];
                    var newDirection = reflect(cubeData.Direction, collisionEvent.Normal);
                    cubeData.Direction = new Vector3(newDirection.x, cubeData.Direction.y, newDirection.z);
                    cubeData.Acceleration = .2f;
                }
    
                if (bEntityIsWall && aEntityIsCube)
                {
                    CubeData cubeData = cubeGroup[entityA];
                    var newDirection = reflect(cubeData.Direction, collisionEvent.Normal);
                    cubeData.Direction = new Vector3(newDirection.x, cubeData.Direction.y, newDirection.z);
                    cubeData.Acceleration = .2f;
                }
            }
        }
    
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            var job = new WallCollisionSystemJob
            {
                wallGroup = GetComponentDataFromEntity<WallData>(true),
                cubeGroup = GetComponentDataFromEntity<CubeData>()
            };
    
            JobHandle jobHandle = job.Schedule(stepPhysicsWorld.Simulation, ref buildPhysicsWorld.PhysicsWorld, inputDeps);
    
            return jobHandle;
        }
    }
}