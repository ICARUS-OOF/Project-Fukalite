﻿using System;
using System.Collections;
using UnityEngine;
using ProjectFukalite.Data.Containment;
using ProjectFukalite.Handlers;
using ProjectFukalite.Systems;
using ProjectFukalite.Data;
using ProjectFukalite.Interfaces;
namespace ProjectFukalite.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour, IUnscalable
    {
        //Assingables
        public Transform playerCam;
        public Transform orientation;
        public AudioSource FootStepSource;

        //Other
        private Rigidbody rb;
        private PlayerReferencer referencer;
        private FootstepSystem fsSystem;
        private PlayerData playerData;

        //Rotation and look
        private float xRotation;
        private float sensitivity = 50f;
        private float sensMultiplier = 1f;

        //Movement
        public float normalSpeed = 1700;
        public float dashSpeed = 2300;
        private float moveSpeed = 1700;
        public float maxSpeed = 20;
        public bool grounded;
        public LayerMask whatIsGround;

        public float counterMovement = 0.175f;
        private float threshold = 0.01f;
        public float maxSlopeAngle = 35f;

        //Crouch & Slide
        private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
        private Vector3 playerScale;
        public float slideForce = 400;
        public float slideCounterMovement = 0.2f;

        //Jumping
        private bool readyToJump = true;
        private float jumpCooldown = 0.25f;
        public float jumpForce = 550f;

        //Input
        private float x, y;
        private bool jumping, dashing, crouching, strafing;
        public bool canMove = true;

        //Sliding
        private Vector3 normalVector = Vector3.up;
        private Vector3 wallNormalVector;

        //Strafing
        public float strafeDuration = .7f;
        public float horizStrafeForce = 2f;
        public float vertStrafeForce = 2f;

        //Falling
        private float FallDuration;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            fsSystem = GetComponent<FootstepSystem>();
        }

        void Start()
        {
            playerScale = transform.localScale;
            referencer = PlayerReferencer.singleton;
            playerData = referencer.playerData;
            moveSpeed = dashing ? dashSpeed : normalSpeed;
            FootStepSource.volume = 0f;
            StartCoroutine(UnscaledUpdate(Time.timeScale));
        }

        private void FixedUpdate()
        {
            if (PlayerUI.singleton.isPanel)
            {
                FootStepSource.volume = 0f;
                return;
            }

            if (TutorialHandler.singleton == null)
            {
                CheckFallDamage();

                if (playerData.Stamina <= 0)
                {
                    canMove = false;
                }
                else
                {
                    canMove = true;
                }
            }

            if (canMove)
            {
                Movement();
            }

            if (TutorialHandler.singleton == null)
            {
                if (playerData.Stamina >= 15f)
                {
                    CheckStrafe();
                }
            } else if (TutorialHandler.singleton != null)
            {
                CheckStrafe();
            }

            SetMagnitudeProperties();
        }

        private void Update()
        {
            if (PlayerUI.singleton.isPanel)
            { return; }

            if (canMove)
            {
                MyInput();
            }

            if (PlayerReferencer.singleton.camMovement.canMove)
            {
                Look();
            }
        }

        public IEnumerator UnscaledUpdate(float timeScale)
        {
            while (true)
            {
                yield return null;
                if (!canMove)
                {
                    continue;
                }
                if (PlayerUI.singleton.isPanel)
                {
                    FootStepSource.Pause();
                } else
                {
                    FootStepSource.UnPause();
                }
            }
        }

        /// <summary>
        /// Find user input. Should put this in its own class but im lazy
        /// </summary>
        private void MyInput()
        {
            x = KeyHandler.GetInputX();
            y = KeyHandler.GetInputY();
            jumping = Input.GetKey(KeyHandler.JumpKey);
            dashing = Input.GetKey(KeyHandler.DashKey);

            moveSpeed = dashing ? dashSpeed : normalSpeed;
        }

        /*
        private void StartCrouch()
        {
            transform.localScale = crouchScale;
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
            if (rb.velocity.magnitude > 0.5f)
            {
                if (grounded)
                {
                    rb.AddForce(orientation.transform.forward * slideForce);
                }
            }
        }

        private void StopCrouch()
        {
            transform.localScale = playerScale;
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        }
        */

        private void Movement()
        {
            //Extra gravity
            rb.AddForce(Vector3.down * Time.fixedDeltaTime * 10);

            //Find actual velocity relative to where player is looking
            Vector2 mag = FindVelRelativeToLook();
            float xMag = mag.x, yMag = mag.y;

            //Counteract sliding and sloppy movement
            CounterMovement(x, y, mag);

            //If holding jump && ready to jump, then jump
            if (readyToJump && jumping) Jump();

            //Set max speed
            float maxSpeed = this.maxSpeed;

            //If sliding down a ramp, add force down so player stays grounded and also builds speed
            if (crouching && grounded && readyToJump)
            {
                rb.AddForce(Vector3.down * Time.fixedDeltaTime * 3000);
                return;
            }

            //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
            if (x > 0 && xMag > maxSpeed) x = 0;
            if (x < 0 && xMag < -maxSpeed) x = 0;
            if (y > 0 && yMag > maxSpeed) y = 0;
            if (y < 0 && yMag < -maxSpeed) y = 0;

            //Some multipliers
            float multiplier = 1f, multiplierV = 1f;

            // Movement in air
            if (!grounded)
            {
                multiplier = 0.5f;
                multiplierV = 0.5f;
            }

            // Movement while sliding
            if (grounded && crouching) multiplierV = 0f;

            //Apply forces to move player
            rb.AddForce(orientation.transform.forward * y * moveSpeed * Time.fixedDeltaTime * multiplier * multiplierV);
            rb.AddForce(orientation.transform.right * x * moveSpeed * Time.fixedDeltaTime * multiplier);
        }

        private void Jump()
        {
            if (grounded && readyToJump)
            {
                readyToJump = false;

                //Add jump forces
                rb.AddForce(Vector2.up * jumpForce * 1.5f);
                rb.AddForce(normalVector * jumpForce * 0.5f);

                //If jumping while falling, reset y velocity.
                Vector3 vel = rb.velocity;
                if (rb.velocity.y < 0.5f)
                    rb.velocity = new Vector3(vel.x, 0, vel.z);
                else if (rb.velocity.y > 0)
                    rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);

                Invoke(nameof(ResetJump), jumpCooldown);
            }
        }

        private void CheckStrafe()
        {
            if (strafing || !grounded)
            {
                return;
            }
            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyHandler.StrafeKey))
            {
                rb.AddForce(-referencer.camMovement.transform.right * horizStrafeForce);
                strafing = true;
                StartCoroutine(ResetStrafe());
            } else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyHandler.StrafeKey))
            {
                rb.AddForce(referencer.camMovement.transform.right * horizStrafeForce);
                strafing = true;
                StartCoroutine(ResetStrafe());
            }
        }

        private IEnumerator ResetStrafe()
        {
            if (TutorialHandler.singleton == null)
            {
                playerData.ReduceStamina(15f);
            }
            yield return new WaitForSeconds(strafeDuration);
            strafing = false;
        }

        private void CheckFallDamage()
        {
            if (grounded)
            {
                if (FallDuration >= 1.8f)
                {
                    playerData.Damage(Mathf.RoundToInt(FallDuration * 10f));
                }
                FallDuration = 0;
            } else
            {
                FallDuration += Time.fixedDeltaTime;
            }
        }

        private void SetMagnitudeProperties()
        {
            if (TutorialHandler.singleton == null)
            {
                if (rb.velocity.magnitude > .3f && rb.velocity.magnitude <= 10f)
                {
                    playerData.ReduceStamina(Time.fixedDeltaTime * playerData.walkStaminaReductionMultiplier);
                }
                else if (rb.velocity.magnitude > 10f)
                {
                    playerData.ReduceStamina(Time.fixedDeltaTime * playerData.dashStaminaReductionMultiplier);
                }
                else
                {
                    playerData.IncreaseStamina(Time.fixedDeltaTime * playerData.staminaRegainMultiplier);
                }
            }
            if (fsSystem.textureValues[0] > 0)
            {
                if (rb.velocity.magnitude > .3f && rb.velocity.magnitude <= 3f && grounded)
                {
                    FootStepSource.volume = Mathf.Lerp(FootStepSource.volume, .3f * GameHandler.Settings.SFXVolume, Time.fixedDeltaTime * 3f);
                    FootStepSource.clip = AudioHandler.GetSoundEffect("Grass Footstep Walk").clip;
                    if (!FootStepSource.isPlaying)
                    {
                        FootStepSource.Play();
                    }
                } else if (rb.velocity.magnitude > 3f && grounded)
                {
                    FootStepSource.volume = Mathf.Lerp(FootStepSource.volume, .3f * GameHandler.Settings.SFXVolume, Time.fixedDeltaTime * 3f);
                    FootStepSource.clip = AudioHandler.GetSoundEffect("Grass Footstep Run").clip;
                    if (!FootStepSource.isPlaying)
                    {
                        FootStepSource.Play();
                    }
                } else
                {
                    FootStepSource.volume = Mathf.Lerp(FootStepSource.volume, 0f, Time.fixedDeltaTime * 3f);
                }
            } else
            {
            }
        }

        private void ResetJump()
        {
            readyToJump = true;
        }

        private float desiredX;
        private void Look()
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier * GameHandler.Settings.MouseSens;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier * GameHandler.Settings.MouseSens;

            //Find current look rotation
            Vector3 rot = playerCam.transform.localRotation.eulerAngles;
            desiredX = rot.y + mouseX;

            //Rotate, and also make sure we dont over- or under-rotate.
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);

            //Perform the rotations
            playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
            orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
        }

        private void CounterMovement(float x, float y, Vector2 mag)
        {
            if (!grounded || jumping) return;

            //Slow down sliding
            if (crouching)
            {
                rb.AddForce(moveSpeed * Time.fixedDeltaTime * -rb.velocity.normalized * slideCounterMovement);
                return;
            }

            //Counter movement
            if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0))
            {
                rb.AddForce(moveSpeed * orientation.transform.right * Time.fixedDeltaTime * -mag.x * counterMovement);
            }
            if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0))
            {
                rb.AddForce(moveSpeed * orientation.transform.forward * Time.fixedDeltaTime * -mag.y * counterMovement);
            }

            //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
            if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed)
            {
                float fallspeed = rb.velocity.y;
                Vector3 n = rb.velocity.normalized * maxSpeed;
                rb.velocity = new Vector3(n.x, fallspeed, n.z);
            }
        }

        /// <summary>
        /// Find the velocity relative to where the player is looking
        /// Useful for vectors calculations regarding movement and limiting movement
        /// </summary>
        /// <returns></returns>
        public Vector2 FindVelRelativeToLook()
        {
            float lookAngle = orientation.transform.eulerAngles.y;
            float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

            float u = Mathf.DeltaAngle(lookAngle, moveAngle);
            float v = 90 - u;

            float magnitue = rb.velocity.magnitude;
            float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
            float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

            return new Vector2(xMag, yMag);
        }

        private bool IsFloor(Vector3 v)
        {
            float angle = Vector3.Angle(Vector3.up, v);
            return angle < maxSlopeAngle;
        }

        private bool cancellingGrounded;

        /// <summary>
        /// Handle ground detection
        /// </summary>
        public void OnCollisionStay(Collision other)
        {
            //Make sure we are only checking for walkable layers
            int layer = other.gameObject.layer;
            if (whatIsGround != (whatIsGround | (1 << layer))) return;

            //Iterate through every collision in a physics update
            for (int i = 0; i < other.contactCount; i++)
            {
                Vector3 normal = other.contacts[i].normal;
                //FLOOR
                if (IsFloor(normal))
                {
                    grounded = true;
                    cancellingGrounded = false;
                    normalVector = normal;
                    CancelInvoke(nameof(StopGrounded));
                }
            }

            //Invoke ground/wall cancel, since we can't check normals with CollisionExit
            float delay = 3f;
            if (!cancellingGrounded)
            {
                cancellingGrounded = true;
                Invoke(nameof(StopGrounded), Time.fixedDeltaTime * delay);
            }
        }

        private void StopGrounded()
        {
            grounded = false;
        }
    }
}