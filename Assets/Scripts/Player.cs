using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody _rb;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float sensitivityX = 50.0f;
    [SerializeField] private float sensitivityY = 50.0f;
    private float _mouseY = 0.0f;

    private bool _locked = false;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if(_locked) return;
        Move();
        Camera();
        if(Input.GetKeyDown(KeyCode.E)) Interact();
    }
    
    void Move()
    {
        _rb.velocity = new Vector3(0,_rb.velocity.y,0);
        Vector3 move = new Vector3(0,0,0);
        if(Input.GetKey(KeyCode.W))
        {
            move += transform.forward * speed;
        }
        if(Input.GetKey(KeyCode.S))
        {
            move -= transform.forward * speed;
        }
        if(Input.GetKey(KeyCode.A))
        {
            move -= transform.right * speed;
        }
        if(Input.GetKey(KeyCode.D))
        {
            move += transform.right * speed;
        }
        transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * sensitivityX, 0) * Time.deltaTime);
        transform.GetChild(0).Rotate(new Vector3(-Input.GetAxis("Mouse Y") * sensitivityY, 0, 0) * Time.deltaTime);
        transform.eulerAngles = new Vector3(Mathf.Clamp(transform.eulerAngles.x, -45f, 45f), transform.eulerAngles.y, 0);
        _rb.AddForce(move, ForceMode.Impulse);
    }

    private void Camera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        _mouseY -= Input.GetAxis("Mouse Y");
        
        // Clamp the vertical rotation of the camera so you can not do a 360
        _mouseY = Mathf.Clamp(_mouseY, -45.0f, 45.0f);

        // Rotate the player around its Y axis
        transform.Rotate(Quaternion.Euler(0, mouseX, 0).eulerAngles);

        // Rotate the camera around its X axis
        transform.GetChild(0).transform.localRotation = Quaternion.Euler(_mouseY, 0, 0);
    }

    void Interact()
    {
        if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 2f))
        {
            if(hit.collider.CompareTag("Spaceship"))
            {
                GameObject root = hit.collider.gameObject.transform.root.gameObject;
                root.GetComponent<Spaceship>().enabled = true;
                transform.position = root.transform.GetChild(0).position;
                transform.parent = root.gameObject.transform;
                transform.rotation = root.transform.rotation;
                _locked = true; 
            }
        }
    }

    public void Unlock()
    {
        _locked = false;
        transform.parent = null;
        transform.position += transform.right * 2;
    }
}
