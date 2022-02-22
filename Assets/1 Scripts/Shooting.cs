using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shooting : MonoBehaviour
{
    // Start is called before the first frame update
    //public GameObject sphere;
    [SerializeField] Transform barrel;
    Vector3 shotTarget;
    private Camera cam;
    public GameObject Crosshair;
    public GameObject bulletPrefab;
    [SerializeField] ParticleSystem gunEffect;
    public bool raycastActive = true;
    [SerializeField] TextMeshProUGUI bulletText;
    [SerializeField] TextMeshProUGUI totalBulletText;
    int currentBulletCount;
    [SerializeField] int chargerCapacity;
    [SerializeField] int totalBulletCount;
    Animator animator;
    [SerializeField] public GameObject pointPrefab;
    void Start()
    {
        animator = this.GetComponent<Animator>();
        cam = GameObject.Find("Camera").GetComponent<Camera>();

        currentBulletCount = chargerCapacity;
        bulletText.text = currentBulletCount.ToString() + " / " + chargerCapacity.ToString();
        totalBulletText.text = totalBulletCount.ToString();
    }

    void Update()
    {
        shotTarget = cam.ScreenToWorldPoint(new Vector3(Crosshair.transform.position.x, Crosshair.transform.position.y, 50f));
        if (totalBulletCount > 0 && Input.GetMouseButtonDown(0) && Globals.isGameActive)
        {
            totalBulletCount--;
            currentBulletCount--;
            if(currentBulletCount == 0)
            {
                Globals.isGameActive = false;

                animator.SetBool("a", false);
                animator.SetBool("w", false);
                animator.SetBool("s", false);
                animator.SetBool("d", false);
                animator.SetBool("wa", false);
                animator.SetBool("wd", false);
                animator.SetBool("sa", false);
                animator.SetBool("sd", false);
                animator.SetBool("fire", false);

                animator.SetTrigger("reload");
            }
            bulletText.text = currentBulletCount.ToString() + " / " + chargerCapacity.ToString();
            totalBulletText.text = totalBulletCount.ToString();

            shotRay();
            shotBullet();
        }

    }
    public void reloading()
    {
        currentBulletCount = chargerCapacity;
        bulletText.text = currentBulletCount.ToString() + " / " + chargerCapacity.ToString();
        Globals.isGameActive = true;

    }
    void shotRay()
    {
        RaycastHit hit;
        if(raycastActive && Physics.Raycast(cam.transform.position,cam.transform.forward,out hit, 100f))
        {
            if(hit.transform.root.tag == "zombie")
            {
                GameObject zombi = hit.transform.root.gameObject;
                zombi.GetComponent<Enemy>().enemyHit(hit.transform, barrel);
            }
        }
    }
    void shotBullet()
    {
        gunEffect.Play();
        GameObject bullet;
        bullet = Instantiate(bulletPrefab, barrel.transform.position, Quaternion.identity);
        bullet.transform.LookAt(shotTarget);
        bullet.GetComponent<IShot>().shot();
    }
}
