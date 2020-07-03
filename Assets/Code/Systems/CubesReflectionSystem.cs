using Components;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Systems
{
    // public class CubesReflectionSystem : SystemBase
    // {
    //     protected override void OnUpdate()
    //     {
    //         var queryTrans = GetEntityQuery(typeof(MagnetTag), typeof(LocalToWorld));
    //         var magnetTransformArr = queryTrans.ToComponentDataArray<LocalToWorld>(Allocator.TempJob);
    //         var magnetTransform = magnetTransformArr[0];
    //
    //         var upperLeftCoord = new float3(-8, 0, 7);
    //         var upperRightCoord = new float3(8, 0, 7);
    //         var upperCenterCoord = new float3(0, 0, 13);
    //
    //         var bottomLeftCoord = new float3(-8, 0, -7);
    //         var bottomRightCoord = new float3(8, 0, -7);
    //         var bottomCenterCoord = new float3(0, 0, -13);
    //
    //         var angleChecker = 4;
    //
    //         var horRange = new Vector2(-10.2f, 10.2f);
    //         var vertRange = new Vector2(-15.3f, 15.3f);
    //
    //         Entities.ForEach((ref CubeData cubeData, ref Translation translation) =>
    //         {
    //             #region Limits detection procedural
    //
    //             var isCollidesLeft = DetectLimitsLeft(translation.Value);
    //             var isCollidesRight = DetectLimitsRight(translation.Value);
    //             var isCollidesUp = DetectLimitsUp(translation.Value);
    //             var isCollidesDown = DetectLimitsDown(translation.Value);
    //
    //             bool DetectLimitsLeft(float3 translationValue)
    //             {
    //                 return translationValue.x <= horRange.x;
    //             }
    //
    //             bool DetectLimitsRight(float3 translationValue)
    //             {
    //                 return translationValue.x >= horRange.y;
    //             }
    //
    //             bool DetectLimitsUp(float3 translationValue)
    //             {
    //                 return translationValue.z >= vertRange.y;
    //             }
    //
    //             bool DetectLimitsDown(float3 translationValue)
    //             {
    //                 return translationValue.z <= vertRange.x;
    //             }
    //
    //             #endregion
    //
    //             if (isCollidesLeft || isCollidesRight || isCollidesUp || isCollidesDown)
    //             {
    //                 var newDirection = float3.zero;
    //
    //                 if (isCollidesLeft)
    //                 {
    //                     newDirection = math.reflect(cubeData.Direction, Vector3.right);
    //                 }
    //
    //                 if (isCollidesRight)
    //                 {
    //                     newDirection = math.reflect(cubeData.Direction, -Vector3.right);
    //                 }
    //
    //                 if (isCollidesUp)
    //                 {
    //                     newDirection = math.reflect(cubeData.Direction, Vector3.back);
    //                 }
    //
    //                 if (isCollidesDown)
    //                 {
    //                     newDirection = math.reflect(cubeData.Direction, Vector3.forward);
    //                 }
    //
    //                 var newPoint = GetReflectionPoint(translation.Value);
    //                 var isCubeInAngle = newPoint.x - 999f <= 0.1f;
    //
    //                 var distanceToMagnet = math.distance(magnetTransform.Position, translation.Value);
    //                 var isCubeInMangetField = distanceToMagnet <= 4f;
    //
    //                 if (isCubeInAngle && isCubeInMangetField)
    //                 {
    //                     var newDirectionRaw = newPoint - translation.Value;
    //                     cubeData.Direction = math.normalize(new float3(newDirectionRaw.x, cubeData.Direction.y, newDirectionRaw.z));
    //
    //                     cubeData.Acceleration = .45f;
    //
    //                     cubeData.IsAccelerated = false;
    //                     cubeData.IsReflected = true;
    //                 }
    //                 else
    //                 {
    //                     cubeData.Direction = new Vector3(newDirection.x, cubeData.Direction.y, newDirection.z);
    //                     cubeData.Acceleration = .2f;
    //                 }
    //
    //                 // чтоб стали реагировать на магнит после отбивания от углов
    //                 if (math.distance(translation.Value, newPoint) <= 6f && isCubeInMangetField)
    //                 {
    //                     cubeData.IsReflected = false;
    //                 }
    //
    //                 ProtectFromOutbounding(ref translation);
    //             }
    //         }).WithBurst().Schedule();
    //
    //         void ProtectFromOutbounding(ref Translation trans)
    //         {
    //             var colliderSize = .01f;
    //
    //             if (trans.Value.x >= horRange.y)
    //             {
    //                 trans.Value = new Vector3(horRange.y - colliderSize, trans.Value.y, trans.Value.z);
    //             }
    //
    //             if (trans.Value.x <= horRange.x)
    //             {
    //                 trans.Value = new Vector3(horRange.x + colliderSize, trans.Value.y, trans.Value.z);
    //             }
    //
    //             if (trans.Value.z >= vertRange.y)
    //             {
    //                 trans.Value = new Vector3(trans.Value.x, trans.Value.y, vertRange.y - colliderSize);
    //             }
    //
    //             if (trans.Value.z <= vertRange.x)
    //             {
    //                 trans.Value = new Vector3(trans.Value.x, trans.Value.y, vertRange.x + colliderSize);
    //             }
    //         }
    //
    //         float3 GetReflectionPoint(float3 position)
    //         {
    //             // on top
    //             if (position.z >= vertRange.y - angleChecker)
    //             {
    //                 // left
    //                 if (position.x <= horRange.x + angleChecker)
    //                 {
    //                     return upperLeftCoord;
    //                 }
    //
    //                 // right
    //                 if (position.x >= horRange.y - angleChecker)
    //                 {
    //                     return upperRightCoord;
    //                 }
    //             }
    //
    //             // on bottom
    //             if (position.z <= vertRange.x + angleChecker)
    //             {
    //                 // left
    //                 if (position.x <= horRange.x + angleChecker)
    //                 {
    //                     return bottomLeftCoord;
    //                 }
    //
    //                 // right
    //                 if (position.x >= horRange.y - angleChecker)
    //                 {
    //                     return bottomRightCoord;
    //                 }
    //             }
    //
    //             return new float3(999f, 0, 0);
    //         }
    //
    //         magnetTransformArr.Dispose();
    //     }
    // }
}