using UnityEngine;

namespace RPG
{
    [System.Serializable]
    public class PlayerControls
    {
        public string HorizontalAxis = "Horizontal";
        public string VerticalAxis = "Vertical";
        public bool UseMouse = false;
        public string RotateXAxis = "";
        public string RotateYAxis = "";

        private Vector3 move;
        private Quaternion rot;
        private Plane raycastPlane = new Plane(Vector3.up, 0);

        public Vector3 GetMovement()
        {
            return move;
        }

        public Quaternion GetRotation()
        {
            return rot;
        }

        public Vector3 GetDirection()
        {
            return rot * Vector3.forward;
        }

        public bool Attack1()
        {
            if (UseMouse)
                return Input.GetMouseButton(0);
            
            return false;
        }
        public bool Attack2()
        {
            if (UseMouse)
                return Input.GetMouseButton(1);

            return false;
        }
        public bool Special1()
        {
            return false;
        }
        public bool Special2()
        {
            return false;
        }

        public void Update(Vector3 position, float movementSpeed, float rotationSpeed)
        {
            // Get movement
            move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            // Rotation using mouse raycast
            if (UseMouse)
            {
                // Raycast to ground plane
                // NOTE: this might fail if we have vertical movement...
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float outDistance;
                if (raycastPlane.Raycast(ray, out outDistance))
                {
                    Vector3 targetPos = ray.GetPoint(outDistance);
                    rot = Quaternion.LookRotation(targetPos - new Vector3(position.x, 0, position.z));
                }
            }
            // NOTE: Untested!
            // Controller stick rotation
            else
            {
                Vector3 targetPos = new Vector3(Input.GetAxis(RotateXAxis), 0, Input.GetAxis(RotateYAxis));
                rot = Quaternion.LookRotation(targetPos);
            }
        }
    }
}