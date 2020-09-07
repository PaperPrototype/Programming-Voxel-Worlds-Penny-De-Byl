using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovePlayer : MonoBehaviour
{
    [Header("player rb")]
    [SerializeField] Rigidbody player_Rb;

    [Header("Vars")]
    [SerializeField] [Range(1f, 2500f)] float zForce = 2000f;
    [SerializeField] [Range(1f, 2500f)] float xForce = 2000f;
    [SerializeField] [Range(1f, 2500f)] float jumpForce = 2500f;


    private float zMoveForce;
    private float xMoveForce;

    // Update is called once per frame
    void Update()
    {
        // Calculating
        zMoveForce = Input.GetAxis("Vertical")  * zForce * Time.deltaTime;
        xMoveForce = Input.GetAxis("Horizontal") * xForce * Time.deltaTime;

        //Adding movement forces
        player_Rb.AddForce(transform.forward * zMoveForce, ForceMode.Impulse);
        player_Rb.AddForce(transform.right * xMoveForce, ForceMode.Impulse);

        //jump
        if (Input.GetButtonDown("Jump"))
        {
            player_Rb.AddForce(Vector3.up * jumpForce * Time.deltaTime, ForceMode.Impulse);
        }
    }
}
