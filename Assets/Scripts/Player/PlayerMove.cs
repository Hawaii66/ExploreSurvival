using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Subdivision.Player
{
    public class PlayerMove : MonoBehaviour
    {
        public CharacterController controller;

        public float speed = 12f;
        public float gravity = -9.81f;
        public float jump;

        public Transform groundCheck;
        public float groundDistance = 0.4f;
        public LayerMask groundMask;

        Vector3 velocity;
        bool isGrounded;

        private void Update()
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            if(isGrounded && Input.GetKeyDown(KeyCode.Space))
            {
                velocity += new Vector3(0,jump,0);
            }

            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * speed * Time.deltaTime);

            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);
        }
    }
}