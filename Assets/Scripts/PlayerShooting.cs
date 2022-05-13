using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Gun Variables")]
    public int currentBulletAmount = 30;
    public int totalBulletAmount = 300;
    public int bullet_Damage = 10;

    [SerializeField] private float fireRate = 12f;
    [SerializeField] private float shootingDelay = 0f;
    [SerializeField] private float gunDistance = 200f;

    [Header("Gun Attachments")]
    [SerializeField] private Camera cam;
    [SerializeField] private Transform gunCamera;

    [SerializeField] private GameObject impactModel;
    [SerializeField] private ParticleSystem gunMuzzleFlash;

    [SerializeField] private GameObject flashLightMod;
    [SerializeField] private bool isHaveFlashLight;

    [SerializeField] private Animator gunAnimations;
    [SerializeField] private AudioSource gunSoundPoint;
    [SerializeField] private AudioClip[] gunSounds;
    [Header("UI elements")]
    [SerializeField]private TextMeshProUGUI bulletsCounterText;

    private bool isReloading = false;

    private Vector3 pos;

    private bool aiming;
    private void Start()
    {
        pos = gunCamera.localPosition;    
    }


    private void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= shootingDelay && currentBulletAmount > 0)
        {
            shootingDelay = Time.time + 1f / fireRate;
            Shoot();
            if (aiming)
            {
                gunAnimations.Play("aimshoot");
            }
            else {
                gunAnimations.Play("shoot");
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            aiming = !aiming;
            if (aiming)
            {
                gunAnimations.Play("goaiming");
            }
            else
            {
                gunAnimations.Play("backaiming");
            }
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading && currentBulletAmount < 30)
        {
            StartCoroutine(Reloading());
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            gunAnimations.Play("lookatgun");
        }

        if (Input.GetKeyDown(KeyCode.T) && isHaveFlashLight) // && haveFlashLight
        {
            gunAnimations.Play("flashlightonoff");
            bool isActive = flashLightMod.activeSelf;
            flashLightMod.SetActive(!isActive);
        }

    }
    private void Shoot() {
        currentBulletAmount--;
        bulletsCounterText.text = $"{currentBulletAmount}/{totalBulletAmount}";
        RaycastHit hit;
        gunMuzzleFlash.Play();
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, gunDistance))
        {
            Dummy dummy = hit.transform.GetComponent<Dummy>();
            if (dummy != null) dummy.TakeDamage(bullet_Damage);

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * 1200f);
            }

            Instantiate(impactModel, hit.point, Quaternion.LookRotation(hit.normal));
        }
    }

    private IEnumerator Reloading()
    {
        int maxCurrentBulletAmount = 30;
        isReloading = true;
        gunSoundPoint.clip = gunSounds[1];
        gunSoundPoint.Play();
        gunAnimations.Play("reloadgun");
        yield return new WaitForSeconds(2);
        for (int i = 0; i < maxCurrentBulletAmount; i++)
        {
            bool isEnough = false;
            while (!isEnough)
            {
                if (currentBulletAmount != maxCurrentBulletAmount && totalBulletAmount >= 0)
                {
                    currentBulletAmount++;
                    totalBulletAmount--;
                    bulletsCounterText.text = $"{currentBulletAmount}/{totalBulletAmount}";
                }
                else
                {
                    isEnough = true;
                }
            }
        }
        isReloading = false;
    }
}