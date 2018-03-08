#define DEBUG_RandomHealDamage

using UnityEngine;

//TODO improves: que sea un porcentaje de su vida, no un valor fijo
//add the particles (here or in life component?)

public class RandomHealDamage : MonoBehaviour {

    /// <summary>
    /// Damage it does
    /// </summary>
    [SerializeField]
    private int damage = 1;

    /// <summary>
    /// Heal it does
    /// </summary>
    [SerializeField]
    private int heal = 1;

    /// <summary>
    /// Tags to be detected
    /// </summary>
    [SerializeField]
    private string[] tagsDetection;


    /// <summary>
    /// </summary>
    public void Start() {
    }

    /// <summary>
    /// Check if the tag is one of the configured tags
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    private bool CheckTag(string tag) {
        bool result = false;
        for (int i = 0; i < tagsDetection.Length && !result; ++i) {
            result = tag.CompareTo(tagsDetection[i]) == 0;
        }
        return result;
    }


    /// <summary>
    /// If someone gets inside the trigger of the healer/Damager, it react
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {
        if (CheckTag(other.gameObject.tag)) {
            #if DEBUG_RandomHealDamage
            Debug.Log("HealDamage triggered.");
            #endif
            HealDamage(other.gameObject);

            //destroy itself
            Destroy(gameObject);
        }
    }

    private void HealDamage(GameObject poorGuy) {
        int value = 0;
        if (Random.value > 0.5f) {
            //heal
            value = heal * -1;
            #if DEBUG_RandomHealDamage
            Debug.Log("Healing");
            #endif
        } else {
            //damage
            value = damage;
            #if DEBUG_RandomHealDamage
            Debug.Log("Damaging");
            #endif
        }
        LifeComponent life = poorGuy.GetComponent<LifeComponent>();
        if (life != null) {
            life.Damage(value);
        }
    }
}
