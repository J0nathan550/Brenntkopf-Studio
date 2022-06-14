using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Gun Variables")]
    public int currentBulletAmount = 30; 
    public int totalBulletAmount = 300;
    public int bullet_Damage = 10;

    [SerializeField] private float fireRate = 12f; //кд оружия
    [SerializeField] private float shootingDelay = 0f; //не трогать!
    [SerializeField] private float gunDistance = 200f; //Дальность
	[SerializeField] private float damageRecoil = 50f; // Отдача по обьекту (попал обьект сдвинуло)
		
    [Header("Gun Attachments")]
    
	[SerializeField] private Camera cam; //камера игрока
    [SerializeField] private Transform gunCamera; //вторая камера для оружия (чтобы небыло бага того что оружие проходит сквозь текстурки).

    [SerializeField] private GameObject impactModel; // Эффект отверстия, когда стреляешь эффект попадания.
    [SerializeField] private ParticleSystem gunMuzzleFlash; //партикл выстрела из дула

    [SerializeField] private GameObject flashLightMod; // опционально если фонарик пригодиться. Можешь за комментить и вырезать если не нужно.
    [SerializeField] private bool isHaveFlashLight; //задаёшь параметр есть у тебя фонарик или нет. В любом случае можешь просто закоментить.

    [SerializeField] private Animator gunAnimations; //аниматор оружия нужны анимки: idleanim, shoot, aimshoot, goaiming (прицеливаешься), backaiming(перестал прицеливаться), lookatgun - не обязательно (смотреть на оружие "Помешан на таркове..."), flashlightonoff - включение выключение фонарика (ОДНОЙ АНИМКОЙ!), reloadgun - перезарядка. ФОТО АНИМАТОРА: https://imgur.com/gallery/cmTgjjx
    [SerializeField] private AudioSource gunSoundPoint; //Аудиосурс один! 
    [SerializeField] private AudioClip[] gunSounds; //закидываешь все звуки в массив аудиосурс проиграет их. Индексы: 0 - Выстрел, 1 - Перезарядка. Дальше по вкусу. 
    
	[Header("UI elements")]
	
    [SerializeField]private TextMeshProUGUI bulletsCounterText; // Отображение количества патронов.

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

        if (Input.GetKeyDown(KeyCode.L)) // Можно закоментировать если не будем смотреть на оружие!
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
		
		gunSoundPoint.clip = gunSounds[0];
        gunSoundPoint.Play();
		
        gunMuzzleFlash.Play();
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, gunDistance))
        {
            Dummy dummy = hit.transform.GetComponent<Dummy>();  //Хит по противнику 
            if (dummy != null) dummy.TakeDamage(bullet_Damage);

            if (hit.rigidbody != null) 
            {
                hit.rigidbody.AddForce(-hit.normal * damageRecoil); //Отталкиваем его по приколу.
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
