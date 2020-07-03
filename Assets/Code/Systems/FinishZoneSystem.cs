using Components;
using Structs;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace Systems
{
    public class FinishZoneSystem : SystemBase
    {
        private FinishZoneData firstZoneData;
        private FinishZoneData secondZoneData;

        private int firstCollectedAmount;
        private int secondCollectedAmount;

        protected override void OnStartRunning()
        {
            base.OnStartRunning();

            Entities.ForEach((ref FinishZoneData finishZoneData) =>
            {
                if (finishZoneData.ZoneType == CubeType.First)
                {
                    firstZoneData = finishZoneData;
                }
                else if (finishZoneData.ZoneType == CubeType.Second)
                {
                    secondZoneData = finishZoneData;
                }
            }).WithoutBurst().Run();
        }

        protected override void OnUpdate()
        {
            var horFirst = firstZoneData.Horizontal;
            var vertFirst = firstZoneData.Vertical;

            var horSecond = secondZoneData.Horizontal;
            var vertSecond = secondZoneData.Vertical;

            Entities.ForEach((ref CubeData cubeData, ref Translation translation) =>
            {
                var centerXFirst = (horFirst.x + horFirst.y) / 2;
                var centerZFirst = (vertFirst.x + vertFirst.y) / 2;

                var centerXSecond = (horSecond.x + horSecond.y) / 2;
                var centerZSecond = (vertSecond.x + vertSecond.y) / 2;

                var value = translation.Value;

                var isInsideFirstHor = value.x >= horFirst.x && value.x <= horFirst.y;
                var isInsideFirstVert = value.z >= vertFirst.x && value.z <= vertFirst.y;

                var isInsideSecondHor = value.x >= horSecond.x && value.x <= horSecond.y;
                var isInsideSecondVert = value.z >= vertSecond.x && value.z <= vertSecond.y;

                var isInsideFirstFinish = isInsideFirstHor && isInsideFirstVert && cubeData.CubeType == CubeType.First;
                var isInsideSecondFinish = isInsideSecondHor && isInsideSecondVert && cubeData.CubeType == CubeType.Second;

                if (isInsideFirstFinish || isInsideSecondFinish)
                {
                    float3 stickingPoint;

                    if (isInsideFirstFinish)
                    {
                        stickingPoint = new float3(centerXFirst, translation.Value.y, centerZFirst);

                        if (!cubeData.IsCollected)
                            firstCollectedAmount += 1;
                    }
                    else
                    {
                        stickingPoint = new float3(centerXSecond, translation.Value.y, centerZSecond);

                        if (!cubeData.IsCollected)
                            secondCollectedAmount += 1;
                    }

                    cubeData.Acceleration = 0;
                    cubeData.IsAccelerated = false;
                    cubeData.IsReflected = false;
                    cubeData.IsCollected = true;
                    cubeData.StickingPoint = stickingPoint;
                }

                if (cubeData.IsCollected)
                {
                    translation.Value = math.lerp(translation.Value, cubeData.StickingPoint, .05f);
                }
            }).WithoutBurst().Run();

            Entities.ForEach((ref FinishZoneData finishZoneData) =>
            {
                if (firstCollectedAmount >= firstZoneData.MaxCount)
                {
                    if (finishZoneData.ZoneType == CubeType.First)
                    {
                        finishZoneData.HasCollected = true;
                    }
                }

                if (secondCollectedAmount >= secondZoneData.MaxCount)
                {
                    if (finishZoneData.ZoneType == CubeType.Second)
                    {
                        finishZoneData.HasCollected = true;
                    }
                }
            }).WithoutBurst().Run();
        }
    }
}