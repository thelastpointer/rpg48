using UnityEngine;

namespace RPG
{
	/// <summary>
	/// Follows the player. Might do fancy stuff later on.
	/// </summary>
	public class CameraHandler : MonoBehaviour
	{
        public Transform Target;

        private void LateUpdate()
        {
            transform.position = Target.position;
        }
    }
}