//#define DEBUG_LifeComponentTower

using UnityEngine;

public class LifeComponentTower : LifeComponent {
    /// <summary>
    /// The life of the tower they depend on
    /// </summary>
    private LifeComponent tower;

    public void Awake() {
        tower = gameObject.transform.parent.GetComponent<LifeComponent>();
        Debug.Assert(tower != null);

        tower.RegisterOnDeath(OnDeathListener);
        tower.RegisterOnLifeChange(OnLifeChangeListener);
    }

    public void Start () {
        
	}

    /// <summary>
    /// Made damage (or health)
    /// Negative amounts health life
    /// it forwards the damage to the tower
    /// </summary>
    /// <param name="damage"></param>
    public override void Damage(int damage) {
        #if DEBUG_LifeComponentTower
        Debug.Log("Damage: " + damage + ".");
        #endif
        tower.Damage(damage);
    }

    public override bool IsDead() {
        return tower.IsDead();
    }

    protected override void Die() { }

    /// <summary>
    /// When the tower died, it died too
    /// </summary>
    private void OnDeathListener() {
        died = true;
        life = 0;
        if (onDeath != null) {
            onDeath();
        }
    }

    /// <summary>
    /// warn about life changes
    /// </summary>
    /// <param name="previousLife"></param>
    private void OnLifeChangeListener(int previousLife) {
        if (onLifeChange != null) {
            onLifeChange(life);
        }
    }
}
