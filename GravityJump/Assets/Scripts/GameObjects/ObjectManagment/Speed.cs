using UnityEngine;

public class Speed
{
    private float Value { get; set; }

    public Speed(float initSpeed)
    {
        this.Value = initSpeed;
    }

    public float GetValue()
    {
        this.Value += 0.01f; // linearly increase the speed, could be another function.
        return this.Value;
    }
}
