using UnityEngine;
using UnityEngine.UI;

namespace _Game.Scripts.Player
{
    public class PlayerHud : MonoBehaviour
    {
        [SerializeField] private Slider hpBar;
        [SerializeField] private Slider staminaBar;
        
        
        public void SetHpMax(float maxHp)
        {
            hpBar.maxValue = maxHp;
        }
        
        public void SetStaminaMax(float maxStamina)
        {
            staminaBar.maxValue = maxStamina;
        }
        
        
        public void SetHp(int hp)
        {
            hpBar.value = hp;
        }
        
        public void SetStamina(float stamina)
        {
            staminaBar.value = stamina;
        }
    }
}