using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gen {
    public class MousePlayerLook : MonoBehaviour
    {
        [Header("Player Rigidbody")]
        [SerializeField] Rigidbody player_Rb;
        

        [Header("Look Variables")]
        [SerializeField] Transform Y_Rot; // gets the y rotating transform  
        [SerializeField] Transform Z_Rot; // gets the z rotating transform
        [SerializeField] [Range(0f, 3f)] float leanMultiplier = 3f;
        [SerializeField] [Range(0.5f, 5f)] float leanInterpolation = 0.5f;
        [SerializeField] Transform X_Rot; // gets the x rotating transform
        [SerializeField] [Range(1f, 10f)] float lookSpeed = 2f; // the look rotation speed
        [SerializeField] bool lockCursor = true; // tells whether the cursor is locked or not


        private Vector2 MouseLook = Vector2.zero; // sets the mouse position to zero (center)

        private float lean = 0f;

        // Start is called before the first frame update
        void Start() {

            if (lockCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        // Update is called once per frame
        private void Update() {

            // mapping the mouse position variable to unitys input system
            MouseLook.y += -Input.GetAxis("Mouse Y");
            MouseLook.x += Input.GetAxis("Mouse X");

            lean-= Input.GetAxis("Mouse X") * leanMultiplier;

            lean = Mathf.MoveTowards(lean, 0f, leanInterpolation);

            // rotation  the individual look transforms
            Y_Rot.localRotation = Quaternion.Euler(0f, MouseLook.x * lookSpeed, 0f);
            X_Rot.localRotation = Quaternion.Euler(MouseLook.y * lookSpeed, 0f, 0f);

            Z_Rot.localRotation = Quaternion.Euler(0f, 0f, lean);

        }
    }

}
