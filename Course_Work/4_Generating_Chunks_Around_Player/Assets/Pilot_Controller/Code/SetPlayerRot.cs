using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gen {
    public class SetPlayerRot : MonoBehaviour
    {
        [SerializeField] Transform Origin_Rot;

        [SerializeField] Rigidbody player_Rb;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            player_Rb.transform.rotation = Origin_Rot.rotation;
        }
    }
}
