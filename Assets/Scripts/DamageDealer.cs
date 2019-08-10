using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private int baseDamage = 100;
    private int damage = -1;

    private void Awake()
    {
        damage = baseDamage;
    }

    public int GetDamage()
    {
        return damage;
    }

    public void IncreaseDamage(float damageModifier)
    {
        damage = (int)(baseDamage * damageModifier);
    }
}
