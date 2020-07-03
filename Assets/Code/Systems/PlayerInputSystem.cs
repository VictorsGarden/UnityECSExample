using Components;
using Unity.Entities;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace Systems
{
    public class PlayerInputSystem : SystemBase
    {
        private Vector3 lastMousePosition;

        protected override void OnUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                lastMousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                var mouseDelta = Input.mousePosition - lastMousePosition;

                Entities
                    .ForEach((ref MagnetTag magnet) =>
                {
                    if (Input.GetMouseButton(0))
                    {
                        lastMousePosition = mouseDelta;
                    }

                    magnet.MouseDelta = lastMousePosition;
                }).WithoutBurst().Run();

                lastMousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                lastMousePosition = Input.mousePosition;
            }
        }
    }
}