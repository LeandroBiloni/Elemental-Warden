using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Manager : MonoBehaviour
{
    public GameObject elemental;
    public FireElementBody fireBody;
    public WaterElementBody waterBody;
    public EarthElementBody earthBody;
    public GameObject warden;
    public WardenBody wBody;
    public GameObject bossLife;
    public Image bossLifeBar;
    public GameObject lifeImg;
    public Image lifeBar;
    public GameObject portalObject;
    public Portal portal;
    public GameObject pause;
    public GameObject resume;
    public GameObject menu;
    public GameObject previous;
    public GameObject next;
    public Scene scene;
    public GameObject memoryObj;
    public Memory memory;
    public GameObject changeDiffButton;
    public GameObject changeDiffScreen;
    public GameObject easy;
    public GameObject normal;
    public GameObject hard;
    public GameObject back;
    public int difficulty;
    public float hpMultiplier;
    public float dmgMultiplier;
    public string sceneName;

    public string actualBoss;
    public float bossDamage;
    public float bossHealth;
    public float bossMaxHealth;
    public bool bossAlive = false;

    // Start is called before the first frame update
    void Start()
    {
        sceneName = scene.name;

        GetDifficulty();

        FindButtons();

        changeDiffScreen = GameObject.Find("ChangeDifficultyBG");
        changeDiffScreen.SetActive(false);

        FindPortal();

        portalObject.SetActive(false);

        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (elemental == null)
            FindElemental();

        if (warden == null)
            FindPlayer();    

        if (bossLife == null || lifeImg == null)
            FindBossLifeBar();

        if (actualBoss == "Tutorial")
            portalObject.SetActive(true);

        if (lifeImg != null)
            WardenLifeBar();

        if (bossAlive && bossHealth != 0)
        {
            CheckBossLife();
            BossLifeBar();
        }
    }

    public void FindPlayer()
    {
        warden = GameObject.Find("WARDEN");
        wBody = warden.GetComponent<WardenBody>();
    }

    public void FindElemental()
    {
        elemental = GameObject.Find("Elemental");

        if (elemental != null)
        {
            if (elemental.layer == 9) //LAYER 9 = FUEGO
            {
                fireBody = elemental.GetComponent<FireElementBody>();
                actualBoss = fireBody.elementType;
            }
            else if (elemental.layer == 13) //LAYER 13 = AGUA
            {
                waterBody = elemental.GetComponent<WaterElementBody>();
                actualBoss = waterBody.elementType;
            }
            else if (elemental.layer == 14) //LAYER 14 = TIERRA
            {
                earthBody = elemental.GetComponent<EarthElementBody>();
                actualBoss = earthBody.elementType;
            }
            bossAlive = true;

            TellDifficulty();
        }
        else actualBoss = "Tutorial";
    }

    public void FindPortal()
    {
        portalObject = GameObject.Find("Portal");
        portal = portalObject.GetComponent<Portal>();
    }

    public void FindButtons()
    {
        pause = GameObject.Find("Pause");
        resume = GameObject.Find("Resume");
        menu = GameObject.Find("Menu");
        previous = GameObject.Find("PreviousLevel");
        next = GameObject.Find("NextLevel");
        changeDiffButton = GameObject.Find("ChangeDifficulty");
        easy = GameObject.Find("Easy");
        normal = GameObject.Find("Normal");
        hard = GameObject.Find("Hard");
        back = GameObject.Find("Back");
        resume.SetActive(false);
        menu.SetActive(false);
        previous.SetActive(false);
        next.SetActive(false);
        changeDiffButton.SetActive(false);
        easy.SetActive(false);
        normal.SetActive(false);
        hard.SetActive(false);
        back.SetActive(false);
    }

    public void FindBossLifeBar()
    {
        if (actualBoss != "Tutorial")
        {
            bossLife = GameObject.Find("BossLifeBar");
            bossLifeBar = bossLife.GetComponent<Image>();
            lifeImg = GameObject.Find("LifeBarFill");
            lifeBar = lifeImg.GetComponent<Image>();
            lifeBar.fillAmount = wBody.maxHealth;
        }
    }

    public void GetDifficulty()
    {
        memoryObj = GameObject.Find("Memory");
        memory = memoryObj.GetComponent<Memory>();
        difficulty = memory.difficulty;
    }

    public void GetElementalStats(float maxHp, float hp, float dmg)
    {
        bossHealth = hp;
        print("Boss max: " + actualBoss + bossHealth);
        bossMaxHealth = maxHp;
        print("Boss health: " + actualBoss + bossMaxHealth);
        bossDamage = dmg;
        print("Boss dmg: " + actualBoss + bossDamage);
    }

    public void TellDifficulty()
    {
        switch (difficulty)
        {
            case 1:
                hpMultiplier = 1;
                dmgMultiplier = 1;
                break;

            case 2:
                hpMultiplier = 1.5f;
                dmgMultiplier = 1.5f;
                break;

            case 3:
                hpMultiplier = 2;
                dmgMultiplier = 2;
                break;
        }

        switch (actualBoss)
        {
            case "Water":
                waterBody.AssignStats(hpMultiplier, dmgMultiplier, difficulty);
                break;

            case "Earth":
                earthBody.AssignStats(hpMultiplier, dmgMultiplier, difficulty);
                break;

            case "Fire":
                fireBody.AssignStats(hpMultiplier, dmgMultiplier, difficulty);
                break;
        }
    }

    public void PlayerDamaged()
    {
        wBody.DamageCalculator(bossDamage, actualBoss);
    }

    public void BossDamaged()
    {
        switch (actualBoss)
        {
            case "Water":
                waterBody.DamageCalculator(wBody.damage, wBody.elementType);
                break;

            case "Earth":
                earthBody.DamageCalculator(wBody.damage, wBody.elementType);
                break;

            case "Fire":
                fireBody.DamageCalculator(wBody.damage, wBody.elementType);
                break;
        }
    }

    public void WardenLifeBar()
    {
        lifeBar.fillAmount = wBody.health / wBody.maxHealth;

        if (wBody.health <= 0)
        {
            GoToLose();
        }
    }

    public void CheckBossLife()
    {
        switch (actualBoss)
        {
            case "Water":
                bossHealth = waterBody.health;
                break;

            case "Earth":
                bossHealth = earthBody.health;
                break;

            case "Fire":
                bossHealth = fireBody.health;
                break;
        }
    }


    public void BossLifeBar()
    {
        bossLifeBar.fillAmount = bossHealth / bossMaxHealth;

        if (bossHealth <= 0)
        {
            elemental.SetActive(false);
            portalObject.SetActive(true);
        }
    }

    public void PortalPlayerDistance()
    {
        portal.distance = Mathf.Abs(warden.transform.position.x - portal.transform.position.x);
    }

    public void PreviousScreen()
    {
        switch (actualBoss)
        {
            case "Water":
                SceneManager.LoadScene("Tutorial");
                break;

            case "Earth":
                SceneManager.LoadScene("WaterRoom");
                break;

            case "Fire":
                SceneManager.LoadScene("EarthRoom");
                break;
        }
    }

    public void NextScreen()
    {
        switch (actualBoss)
        {
            case "Tutorial":
                SceneManager.LoadScene("WaterRoom");
                break;

            case "Water":
                SceneManager.LoadScene("EarthRoom");
                break;

            case "Earth":
                SceneManager.LoadScene("FireRoom");
                break;

            case "Fire":
                SceneManager.LoadScene("Win");
                break;
        }
    }

    public void Pause()
    {
        {
            Time.timeScale = 0;
            pause.SetActive(false);
            resume.SetActive(true);
            menu.SetActive(true);
            changeDiffButton.SetActive(true);
            if (actualBoss != "Tutorial")
                previous.SetActive(true);
            if (actualBoss != "Fire")
                next.SetActive(true);
        }
    }

    public void Resume()
    {
        pause.SetActive(true);
        resume.SetActive(false);
        menu.SetActive(false);
        previous.SetActive(false);
        next.SetActive(false);
        Time.timeScale = 1;
    }

    public void ChangeDifficultyButton()
    {
        changeDiffScreen.SetActive(true);

        switch (difficulty)
        {
            case 1:
                easy.SetActive(false);
                normal.SetActive(true);
                hard.SetActive(true);
                break;

            case 2:
                easy.SetActive(true);
                normal.SetActive(false);
                hard.SetActive(true);
                break;

            case 3:
                easy.SetActive(true);
                normal.SetActive(true);
                hard.SetActive(false);
                break;
        }
    }

    public void EasyButton()
    {
        memory.difficulty = 1;

        ReloadScene();
    }

    public void NormalButton()
    {
        memory.difficulty = 2;

        ReloadScene();
    }

    public void HardButton()
    {
        memory.difficulty = 3;

        ReloadScene();
    }

    public void BackButton()
    {
        changeDiffScreen.SetActive(false);
    }

    public void ReloadScene()
    {
        switch (actualBoss)
        {
            case "Water":
                SceneManager.LoadScene("WaterRoom");
                break;

            case "Earth":
                SceneManager.LoadScene("EarthRoom");
                break;

            case "Fire":
                SceneManager.LoadScene("FireRoom");
                break;

            case "Tutorial":
                SceneManager.LoadScene("Tutorial");
                break;
        }
    }

    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToLose()
    {
            SceneManager.LoadScene("Lose");
    }
}
