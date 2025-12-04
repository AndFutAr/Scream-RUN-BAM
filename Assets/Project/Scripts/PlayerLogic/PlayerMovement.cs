using UnityEngine;

namespace Project.Scripts.PlayerLogic
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private GameObject original, moveRus, hitRus, deadRus;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 10f;
        
        private Rigidbody rb;
        private Transform cameraTransform;
        private Vector3 moveDirection;
        private bool isMoving, isHitting;
        private float currentSpeed;
        
        public void SetHit(bool isHit) => isHitting = isHit;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            cameraTransform = Camera.main.transform;
            
            rb.freezeRotation = true;
        }

        public void Die()
        {
            deadRus.SetActive(true);
            original.SetActive(false);
            moveRus.SetActive(false);
            hitRus.SetActive(false);
        }

        void Update()
        {
            HandleInput();
            
            if (isMoving)
            {
                RotateTowardsMovement();
            }
            
            UpdateAnimations();
        }

        void FixedUpdate()
        {
            MovePlayer();
        }

        void HandleInput()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;
            
            cameraForward.y = 0;
            cameraRight.y = 0;
            cameraForward.Normalize();
            cameraRight.Normalize();

            moveDirection = (cameraForward * vertical + cameraRight * horizontal);
            
            if (moveDirection.magnitude > 1f)
            {
                moveDirection.Normalize();
            }
            
            isMoving = moveDirection.magnitude > 0.1f;
            currentSpeed = moveDirection.magnitude;
        }

        void MovePlayer()
        {
            Vector3 targetVelocity = moveDirection * moveSpeed;
            
            targetVelocity.y = rb.linearVelocity.y;
            
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, targetVelocity, Time.fixedDeltaTime * 10f);
        }

        void RotateTowardsMovement()
        {
            if (moveDirection.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                moveRus.transform.rotation = targetRotation;
                hitRus.transform.rotation = targetRotation;
                original.transform.eulerAngles = new Vector3(-90, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);
            }
        }

        void UpdateAnimations()
        {
            if (isHitting)
            {
                hitRus.SetActive(true);
                original.SetActive(false);
                moveRus.SetActive(false);
            }
            else if (isMoving)
            {
                moveRus.SetActive(true);
                original.SetActive(false);
                hitRus.SetActive(false);
            }
            else
            {
                
                original.SetActive(true);
                moveRus.SetActive(false);
                hitRus.SetActive(false);
            }
        }
    }
}