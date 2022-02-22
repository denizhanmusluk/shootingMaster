using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
public class PlayerController : MonoBehaviour
{
    [SerializeField] public float damp;
    public bool gameActive;
    public bool speedUp;
    bool _hit = true;
    Vector3 stickDirection;

    Animator animator;
    Rigidbody playerRigidbody;
    [HideInInspector] public AudioSource audioSource;
    [HideInInspector] public float walkSpeed = 1;
    [Range(1, 20)] [SerializeField] public float rotationSpeed;
    [Range(1, 50)] [SerializeField] public float moveSpeed;

    int heart = 2;
    [SerializeField] CinemachineVirtualCamera cam1;
    [SerializeField] CinemachineVirtualCamera camFinish;

    public int maxHealth = 3;
    public int currnetHealth = 3;
    [SerializeField] GameObject weaponPos;
    [SerializeField] GameObject eye;

    public GameObject finish;
    public GameObject cross;

    [Range(50, 500)]
    public float sens;
    public Transform body;
    public Transform cameraTarget;
    float xRot = 0f;
    float yRot = 0f;
    [SerializeField] AudioClip shot;
    AudioSource audio;
    [SerializeField] Slider healthBar;
    [SerializeField] Image redPanelImage;
    [SerializeField] RectTransform crosshair;
    void Start()
    {
        currnetHealth = maxHealth;
        healthBar.value = currnetHealth / maxHealth;

        speedUp = true;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        playerRigidbody = GetComponent<Rigidbody>();
        walkSpeed = 1;
        audio = GetComponent<AudioSource>();
        audio.clip = shot;
    }

  
    public void healthDown()
    {
        currnetHealth--;
        healthBar.value = (float)currnetHealth / (float)maxHealth;

        if (currnetHealth <= 0)
        {
            this.GetComponent<RagdollToggle>().RagdollActivate2(true, 0);
            GameManager.Instance.Notify_LoseObservers();
            cam1.Priority = 0;
            camFinish.Priority = 1;
        }
        StartCoroutine(redPanel());
    }
    IEnumerator redPanel()
    {
        float counter = 0f;
        float value = 0f;
        while (counter < Mathf.PI)
        {
            counter += 10 * Time.deltaTime;
            value = Mathf.Sin(counter);
            redPanelImage.color = new Color(1, 0, 0, value * 0.2f);
            yield return null;
        }
    }
    private void Update()
    {
        if (Globals.isGameActive)
        {
            behaviour();
            rotateDirection();
        }
    }
    public void finishDirection(float angle)
    {
        stickDirection = new Vector3(stickDirection.x, angle, stickDirection.y);
    }
    void rotateDirection()
    {
        float rotX = Input.GetAxisRaw("Mouse X") * sens * Time.deltaTime;
        float rotY = Input.GetAxisRaw("Mouse Y") * sens * Time.deltaTime;

        xRot += rotY;
        xRot = Mathf.Clamp(xRot, -40f, 40f);

        yRot -= rotX;

        cameraTarget.rotation = Quaternion.Euler(-xRot, -yRot, 0f);
        body.localRotation = Quaternion.Euler(0f, 0f, xRot);
        transform.rotation = Quaternion.Euler(0, -yRot, 0);
    }
    void behaviour()
    {
        Vector3 direction = Vector3.zero;
        string key = null;
        float speed = moveSpeed;
        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetMouseButtonDown(0))
            {
                animator.SetTrigger("firewalk");
                audio.Play();
                StartCoroutine(crossHairScaling());
            }
            key += "w";
            direction += transform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            key += "s";
            direction += -transform.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            key += "a";
            direction += -transform.right;
        }
     
        if (Input.GetKey(KeyCode.D))
        {
            key += "d";
            direction += transform.right;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = moveSpeed * 1.5f;
            animator.speed = 1.5f;
        }
        else
        {
            speed = moveSpeed;
            animator.speed = 1;
        }

        animator.SetBool("a", false);
        animator.SetBool("w", false);
        animator.SetBool("s", false);
        animator.SetBool("d", false);
        animator.SetBool("wa", false);
        animator.SetBool("wd", false);
        animator.SetBool("sa", false);
        animator.SetBool("sd", false);
        if (key != null)
        {
            animator.SetBool(key, true);
        }
        else if (Input.GetMouseButton(0))
        {
            animator.SetBool("fire", true);
            if (Input.GetKey(KeyCode.W))
            {
                animator.SetTrigger("firewalk");
            }
            if (Input.GetMouseButtonDown(0))
            {
                audio.Play();
                StartCoroutine(crossHairScaling());
            }
        }
        else
        {
            animator.SetBool("fire", false);
        }


        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);



    }
    IEnumerator crossHairScaling()
    {
        crosshair.localScale = new Vector3(2, 2, 2);
        float counter = 0;
        while (counter < Mathf.PI)
        {
            counter += 5 * Time.deltaTime;

            float scaleMult = 2 + Mathf.Sin(counter);
            crosshair.localScale = new Vector3(scaleMult, scaleMult, scaleMult);
            yield return null;

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("action");
                break;
            }
        }
        crosshair.localScale = new Vector3(2, 2, 2);
    }
}
