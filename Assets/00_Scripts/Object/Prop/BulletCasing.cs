using UnityEngine;

public class BulletCasing : MonoBehaviour
{
    private Rigidbody rb;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetEjectData(float power, Vector3 position, Vector3 direction)
    {
        if(rb != null)
        {
            float finalPower = Random.Range(power * 0.7f, power);
            rb.AddExplosionForce(finalPower, position + direction, 1f);
            rb.AddTorque(new Vector3(0, Random.Range(100, 500), Random.Range(100, 1000)), ForceMode.Impulse);
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Gun"))
        {
            SoundManager.Instance.PlaySFXForName("Shell", this.transform.position);
        }
    }
}
