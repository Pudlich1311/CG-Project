
using UnityEngine;
using TMPro;

public class GunSystem : MonoBehaviour
{
    //Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;


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
    private void Update()
    {
        MyInput();

        //SetText
        text.SetText(bulletsLeft + " / " + magazineSize);
    }
    private void MyInput()
    {
        if(enb)
        {
            if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
            else shooting = Input.GetKeyDown(KeyCode.Mouse0);

            if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

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
        //if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range))
        if(Physics.Raycast(ray, out hit, range))
        {
            GameObject laser = GameObject.Instantiate(shootPrefab, attackPoint.position, attackPoint.rotation) as GameObject;
            laser.GetComponent<ShotBehavior>().setTarget(hit.point);
            GameObject.Destroy(laser, 2f);
            // Debug.Log(rayHit.collider.name);
            // if (rayHit.collider.CompareTag("Enemy"))
            //  rayHit.collider.GetComponent<ShootingAi>().TakeDamage(damage);
        }

        //ShakeCamera
      //  camShake.Shake(camShakeDuration, camShakeMagnitude);

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
        bulletsLeft = magazineSize;
        reloading = false;
    }



}
