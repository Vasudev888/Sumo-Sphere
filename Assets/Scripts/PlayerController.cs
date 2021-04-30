using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    public bool hasPowerup = false;
    public GameObject powerUpIndicator;

    private GameObject focalPoint;
    private Rigidbody playerRb;
    private float powerUpStrength = 15;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");

        playerRb.AddForce(focalPoint.transform.forward * forwardInput * speed);

        powerUpIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            powerUpIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerUpCoroutine());
        }
    }

    IEnumerator PowerUpCoroutine()
    {
        yield return new WaitForSeconds(7); 
        hasPowerup = false;
        powerUpIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRigidBody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

            enemyRigidBody.AddForce(awayFromPlayer * powerUpStrength, ForceMode.Impulse);

            Debug.Log("Collided with : " + collision.gameObject.name + " with powerup set to : " + hasPowerup);
        }
    }
}
