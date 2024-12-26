using UnityEngine;

namespace Game.Client
{
    public class PCInputCameraHandler
    {
        public float MouseRotationSensitivityX { get; set; } = 1;
        public float MouseRotationSensitivityY { get; set; } = 1;
        public Vector2 RotationVector { get; private set; } = Vector2.zero; 
        
        public void Update( float deltaTime)
        {
            var mouseX = Input.GetAxis("Mouse X") * MouseRotationSensitivityX;
            var mouseY = Input.GetAxis("Mouse Y") * MouseRotationSensitivityY;
            if (Mathf.Abs(mouseX) <= 0 && Mathf.Abs(mouseY) <= 0)
            {
                RotationVector = Vector2.zero;
                return;
            }
            var mouseMove = new Vector2(mouseX, mouseY);

            RotationVector = mouseMove;
        }
    }
}