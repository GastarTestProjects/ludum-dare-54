using UnityEngine;

namespace _Game.Scripts.Player
{
    public class PlayerWeapon : MonoBehaviour
    {
        public void Shoot(Rigidbody ownerRigidbody, float forceMultiplier = 100f)
        {
            // add force to the owner rigidbody to simulate recoil
            ownerRigidbody.AddForce(-transform.forward * forceMultiplier, ForceMode.Impulse);
        }
    }
}