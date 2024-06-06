using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    private Rigidbody _rb;
    [SerializeField] private float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)) Dismount();
        Move();
        Camera();
    }

    void Move()
    {
        _rb.velocity = new Vector3(0,0,0);
        if(Input.GetKey(KeyCode.W))
        {
            _rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        }
        if(Input.GetKey(KeyCode.S))
        {
            _rb.AddForce(-transform.forward * speed, ForceMode.Impulse);
        }
        if(Input.GetKey(KeyCode.A))
        {
            _rb.AddForce(-transform.right * speed, ForceMode.Impulse);
        }
        if(Input.GetKey(KeyCode.D))
        {
            _rb.AddForce(transform.right * speed, ForceMode.Impulse);
        }
        if(Input.GetKey(KeyCode.Space))
        {
            _rb.AddForce(transform.up * speed, ForceMode.Impulse);
        }
        if(Input.GetKey(KeyCode.C))
        {
            _rb.AddForce(-transform.up * speed, ForceMode.Impulse);
        }
    }

    void Camera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(Quaternion.Euler(mouseY, mouseX, 0).eulerAngles);
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
    }

    void Dismount()
    {
        GameObject player = transform.GetChild(transform.childCount - 1).gameObject;

        _rb.constraints = RigidbodyConstraints.FreezeAll;

        player.GetComponent<Player>().Unlock();
        enabled = false;
    }
}
