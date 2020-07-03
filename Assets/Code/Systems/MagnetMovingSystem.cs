using Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    public class MagnetMovingSystem : SystemBase
    {
        private Vector2 horRange  = new Vector2(-9.5f, 9.5f);
        private Vector2 vertRange = new Vector2(-14.5f, 14.5f);

        protected override void OnUpdate()
        {
            if (Input.GetMouseButton(0))
            {
                var movingSpeed = 2f;
                var rotatingSensitivity = 1.5f;
                
                Entities
                    .ForEach((ref MagnetTag magnet, ref Translation translation, ref Rotation rotation, ref LocalToWorld transform) =>
                    {
                        var mouseDelta = magnet.MouseDelta;
                        magnet.DeltaTime = Time.DeltaTime;

                        var inputAngle = Mathf.Atan2(mouseDelta.x, mouseDelta.y) * Mathf.Rad2Deg;
                        var newRotation = Quaternion.AngleAxis(inputAngle, Vector3.up);

                        if (mouseDelta.magnitude > rotatingSensitivity)
                        {
                            rotation.Value = Quaternion.Lerp(rotation.Value, newRotation, magnet.DeltaTime * 5f);
                        }

                        var isCollides = DetectLimits(translation.Value);

                        if (!isCollides)
                        {
                            var newPosition = new float3(
                                translation.Value.x + mouseDelta.x * magnet.DeltaTime * movingSpeed,
                                translation.Value.y,
                                translation.Value.z + mouseDelta.y * magnet.DeltaTime * movingSpeed);

                            translation.Value = newPosition;
                        }

                        ProtectFromOutbounding(ref translation);
                    }).WithoutBurst().Run();
            }
        }

        private bool DetectLimits(Vector3 checkPoint)
        {
            return checkPoint.x >= horRange.y || checkPoint.x <= horRange.x ||
                   checkPoint.z >= vertRange.y || checkPoint.z <= vertRange.x;
        }

        private void ProtectFromOutbounding(ref Translation translation)
        {
            var colliderSize = .01f;

            if (translation.Value.x >= horRange.y)
            {
                translation.Value = new Vector3(horRange.y - colliderSize, translation.Value.y, translation.Value.z);
            }

            if (translation.Value.x <= horRange.x)
            {
                translation.Value = new Vector3(horRange.x + colliderSize, translation.Value.y, translation.Value.z);
            }

            if (translation.Value.z >= vertRange.y)
            {
                translation.Value = new Vector3(translation.Value.x, translation.Value.y, vertRange.y - colliderSize);
            }

            if (translation.Value.z <= vertRange.x)
            {
                translation.Value = new Vector3(translation.Value.x, translation.Value.y, vertRange.x + colliderSize);
            }
        }
    }
}