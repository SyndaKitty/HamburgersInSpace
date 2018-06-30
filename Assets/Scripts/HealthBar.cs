using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    GameObject healthPointPrefab;
    Unit unit;
    List<HealthPoint> healthPoints = new List<HealthPoint>();
    int healthAmount;

    public void Initialize(Unit playerUnit, GameObject healthPointPrefab)
    {
        this.unit = playerUnit;
        this.healthPointPrefab = healthPointPrefab;
        HealthUpdate();
    }

    // Use when player's health or maxHealth changes
    public void HealthUpdate()
    {
        // Handle changes in max health
        while (healthPoints.Count < unit.MaxHealth)
        {
            AddHealthPoint();
        }
        while (healthPoints.Count > unit.MaxHealth && healthPoints.Count > 0)
        {
            var healthPoint = healthPoints[healthPoints.Count - 1];
            healthPoints.RemoveAt(healthPoints.Count - 1);
            Destroy(healthPoint.gameObject);
        }

        // Handle changes in health
        while (healthAmount < unit.Health)
        {
            healthPoints[healthAmount].SetFilled(true);
            healthAmount++;
        }

        while (healthAmount > unit.Health)
        {
            healthPoints[healthAmount - 1].SetFilled(false);
            healthAmount--;
        }
    }

    void AddHealthPoint()
    {
        int x = 32 + 60 * healthPoints.Count;
        var healthPointObject = Instantiate(healthPointPrefab, transform);
        var hpTransform = healthPointObject.GetComponent<RectTransform>();
        hpTransform.position = new Vector2(x, 32);
        healthPoints.Add(healthPointObject.GetComponent<HealthPoint>());
    }
}
