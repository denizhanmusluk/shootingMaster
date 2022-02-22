using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, ILoseObserver
{
    [Range(0, 50)] [SerializeField] public float walkSpeed, rotSpeed;
    public bool gun;

    public enum States {  attack, followingPlayer, idle, dead }
    public States currentBehaviour;
    GameObject player;
    Animator anim;
    Rigidbody rigidbody;
    [SerializeField] float deadTime;
    float counter = 0f;
    public int health = 3,maxHealth = 3;
    bool hitActive = true;
    protected Canvas[] canvas;
    GameObject point;
    void Start()
    {
        GameManager.Instance.Add_LoseObserver(this);
        canvas = GetComponentsInChildren<Canvas>();

        rigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        player = GameObject.Find("Player");
        anim.SetTrigger("walk");
        point = player.GetComponent<Shooting>().pointPrefab;
    }
    public void LoseScenario()
    {
        currentBehaviour = States.idle;
        anim.SetTrigger("idle");

    }
    // Update is called once per frame
    void Update()
    {
        switch (currentBehaviour)
        {
            case States.followingPlayer:
                {
                    movement();
                }
                break;
            case States.attack:
                {
                    //_attack();
                }
                break;
            case States.dead:
                {
                    deadDest();
                }
                break;

            case States.idle:

                break;
        }
    }
    void deadDest()
    {
        if(counter == 0)
        {
            var pnt = Instantiate(point, transform.position, Quaternion.identity);
            pnt.transform.rotation = Quaternion.Euler(pnt.transform.eulerAngles.x, player.transform.eulerAngles.y, pnt.transform.eulerAngles.z);
        }
        counter += Time.deltaTime;

        if (counter >= 4)
        {
            Destroy(gameObject);
        }
    }
    void movement()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, Time.deltaTime * walkSpeed);
        if (Vector3.Distance(player.transform.position, transform.position) > 2)
        {
            followingRotation(player.transform.position);
            anim.SetBool("hit", false);
        }
        else
        {
            anim.SetBool("hit", true);
        }

    }
  
    private void followingRotation(Vector3 target)
    {

        Vector3 relativeVector = transform.InverseTransformPoint(target);
        relativeVector /= relativeVector.magnitude;
        float newSteer = (relativeVector.x / relativeVector.magnitude);
        transform.Rotate(0, newSteer * Time.deltaTime * 50 * rotSpeed, 0);
    }

  public void healthBarSet()
    {
        canvas[0].transform.GetChild(0).GetComponent<Slider>().value = (float)health / (float)maxHealth;
    }
    public void enemyHit(Transform hitPart, Transform barrel)
    {
        if (hitActive)
        {
            float drag = 10;
            float hitForce;
            bool deadActive = false;
            if (hitPart.tag == "head")
            {
                health -= 3;
            }
            else
            {
                anim.SetBool("bottomwalk", false);

                health--;
            }
            healthBarSet();
            if (health <= 0)
            {
                hitActive = false;
                drag = 0;
                deadActive = true;
                hitForce = 20000;
                currentBehaviour = States.dead;
                canvas[0].gameObject.SetActive(false);
                GameManager.Instance.scoreSet();
     
            }
            else
            {
                hitForce = 10000;
            }
            transform.GetComponent<RagdollToggle>().RagdollActivate2(true, drag);

            StartCoroutine(hitPArtForce(hitPart, barrel, hitForce, deadActive, drag));
        }
    }
    IEnumerator hitPArtForce(Transform hitPart, Transform barrel, float multiple,bool dying,float drag)
    {

        yield return null;
        hitPart.GetComponent<Rigidbody>().AddForce((hitPart.transform.position - barrel.transform.position).normalized * multiple);
        Debug.Log(hitPart.transform.name);
        yield return new WaitForSeconds(0.5f);
        if (!dying)
        {
            transform.GetComponent<RagdollToggle>().RagdollActivate2(dying, drag);
        }
    }
}
