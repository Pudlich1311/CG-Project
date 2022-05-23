
using UnityEngine;
using TMPro;

public class GunSystem : MonoBehaviour
{
    //Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    public int bulletsLeft, bulletsShot;
    public int allBullets;


    public GameObject shootPrefab;
    public bool enb=false;

    [SerializeField] ParticleSystem MuzzleParticle = null;
    [SerializeField] ParticleSystem ReloadParticle = null;

    //bools 
    bool shooting, readyToShoot, reloading;

    //sound
    public AudioSource audioSource;
    public AudioClip fire, reload;

    //Reference
    private Camera fpsCam;  
    public Transform attackPoint;
    public RaycastHit hit;
    public LayerMask whatIsEnemy;

    //Graphics
   // public CamShake camShake;

    public TextMeshProUGUI text;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        fpsCam = GameObject.Find("MainCamera").GetComponent<Camera>();
        text = GameObject.Find("UI").GetComponent<TextMeshProUGUI>();
    }


    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
        
    }
    public void Update()
    {
        MyInput();

        //SetText
        if(enb)
        {
            text.SetText(bulletsLeft + " / " + allBullets);
        }
        
    }
    private void MyInput()
    {
        if(enb)
        {
            if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
            else shooting = Input.GetKeyDown(KeyCode.Mouse0);

            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading && allBullets != 0) Reload();

            //Shoot
            if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
            {
                bulletsShot = bulletsPerTap;
                Shoot();
            }
        }

    }
    private void Shoot()
    {
        readyToShoot = false;

        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate Direction with Spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

        //RayCast
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(fpsCam.transform.position, direction, out hit, range))
        {
            GameObject laser = GameObject.Instantiate(shootPrefab, attackPoint.position, attackPoint.rotation) as GameObject;
            laser.GetComponent<ShotBehavior>().setTarget(hit.point);
            GameObject.Destroy(laser, 2f);

             if (hit.collider.CompareTag("Enemy"))
             {
                if (hit.collider.name == "PA_WarriorControl")
                {
                    hit.collider.GetComponentInParent<Drone>().TakeDamage(damage);
                }
                else if(hit.collider.GetComponent<Crab>() != null)
                {
                    hit.collider.GetComponent<Crab>().TakeDamage(damage);
                }
             }
        }


        MuzzleParticle.Play();
        audioSource.PlayOneShot(fire, 1);

        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if(bulletsShot > 0 && bulletsLeft > 0)
        Invoke("Shoot", timeBetweenShots);
    }
    private void ResetShot()
    {
        readyToShoot = true;
    }
    private void Reload()
    {
        reloading = true;
        ReloadParticle.Play();
        audioSource.PlayOneShot(reload, 0.5f);
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        if(allBullets < magazineSize && (allBullets+bulletsLeft)<=magazineSize)
        {
            bulletsLeft += bulletsLeft + allBullets;
            allBullets = 0;
            reloading = false;
        }
        else
        {
            allBullets -= magazineSize - bulletsLeft;
            bulletsLeft = magazineSize;
            reloading = false;
        }

    }



}
