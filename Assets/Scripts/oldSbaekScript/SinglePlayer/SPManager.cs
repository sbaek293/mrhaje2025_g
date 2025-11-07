//using Com.Sibaek.FPS;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//public class SPManager : MonoBehaviour
//{
//    public GameObject[] enemy_prefab;
//    public Transform[] spawnpoints;
//    private float cooldown = 3;
//    private float current_cd = 3;
//    public SPPlayer spplayer;
//    public float timer;
    
//    public float minutes;
//    public Text timeCountText;
//    public Text xpText;

//    public int curFullXP;

//    public GameObject augments;
//    public GameObject augmentPrefab;

//    public Sprite testImage;
//    //damage,attackspeed,speed,health,life steal,health regen,ability(multiple)

//    public Image xpBar;
//    public GameObject[] augementCards;
//    // Start is called before the first frame update
//    void Start()
//    {
        
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if(current_cd <= 0)
//        {
//            Spawn();
//        }
//        timer += Time.deltaTime;

//        if (timer > 59)
//        {
//            minutes += 1;
//            timer = 0;
//        }

//        timeCountText.text = minutes.ToString("00") + " : " + timer.ToString("00");

//        RefreshXP();
//    }

//    private void FixedUpdate()
//    {
//        //cooldown
//        if (current_cd > 0) current_cd -= Time.fixedDeltaTime;
//    }

//    public void Spawn()
//    {
//        Instantiate(enemy_prefab[Random.Range(0, 2)], spawnpoints[Random.Range(0,4)].position, spawnpoints[Random.Range(0, 4)].rotation);
//        current_cd = cooldown;
//    }

//    void RefreshXP()
//    {

//        float xp_ratio = (float)spplayer.xp / (float)curFullXP;

//        xpBar.fillAmount = Mathf.Lerp(xpBar.fillAmount, xp_ratio, Time.fixedDeltaTime * 8f);

//        xpText.text = spplayer.xp + "/"+ curFullXP;

//        if(spplayer.xp >= curFullXP) {
//            curFullXP += 50;
//            spplayer.xp = 0;
//            createAugments();
           
//        }
//    }

//    void createAugments()
//    {
//        Time.timeScale = 0;
//        SPPause.paused = true;
//        Cursor.lockState = CursorLockMode.None ;
//        Cursor.visible = true;

//        augments.SetActive(true);
//        GameObject group = augments.transform.GetChild(1).gameObject;


//        augementCards[0] = Instantiate(augmentPrefab, group.transform);
//        augementCards[0].GetComponent<Augment>().init("test", "test", "test", "test", "test",testImage,0);
//        augementCards[1] = Instantiate(augmentPrefab, group.transform);
//        augementCards[1].GetComponent<Augment>().init("test", "test", "test", "test", "test", testImage, 1);
//        augementCards[2] = Instantiate(augmentPrefab, group.transform);
//        augementCards[2].GetComponent<Augment>().init("test", "test", "test", "test", "test", testImage, 2);


       
//    }

//    public void augment_click(int id)
//    {
//        Time.timeScale = 1;
//        SPPause.paused = false;
//        Cursor.lockState = CursorLockMode.Locked;
//        Cursor.visible = false;
//        augments.SetActive(false);
//        Destroy(augementCards[0]);
//        Destroy(augementCards[1]);
//        Destroy(augementCards[2]);
//    }
//}
