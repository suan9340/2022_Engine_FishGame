using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;


public class DragAndShoot2 : MonoBehaviour
{
    public DefineManager.FishRating rating = DefineManager.FishRating.BRONZE;

    [Header("Movement")]
    public float maxPower;
    float shootPower;
    public float gravity = 1;
    [Range(0f, 0.1f)] public float slowMotion;

    public bool shootWhileMoving = false;
    public bool forwardDraging = true;
    public bool showLineOnScreen = false;

    Transform direction;
    Rigidbody2D rb;
    LineRenderer screenLine;


    private FishManagerSO fishManagerSO;
    public Outline outline;

    // Vectors // 
    Vector2 startPosition;
    Vector2 targetPosition;
    Vector2 startMousePos;
    Vector2 currentMousePos;

    bool canShoot = true;
    public bool isDie = false;

    [Space(30)]
    public FishInformationSO fishInfo;

    private bool isMousePointOn = false;
    private bool isCharging = false;

    public int fishImageNum;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravity;
        direction = transform.GetChild(0);
        screenLine = direction.GetComponent<LineRenderer>();
        outline = GetComponentInChildren<Outline>();

        ConnectingRatingFish();


        outline.enabled = false;
        outline.OutlineColor = fishInfo.outlineColor;
        fishManagerSO.mouseOnFish = null;

    }

    void Update()
    {
        if (GameManager.Instance.gameState != DefineManager.GameState.PLAYING)
        {
            return;
        }

        if (!isMousePointOn) return;

        if (Input.GetMouseButtonDown(0))
        {
            // if (EventSystem.current.currentSelectedGameObject) return;  //ENABLE THIS IF YOU DONT WANT TO IGNORE UI
            MouseClick();
        }
        if (Input.GetMouseButton(0))
        {
            // if (EventSystem.current.currentSelectedGameObject) return;  //ENABLE THIS IF YOU DONT WANT TO IGNORE UI
            MouseDrag();

            if (shootWhileMoving) rb.velocity /= (1 + slowMotion);

        }

        if (Input.GetMouseButtonUp(0))
        {
            // if (EventSystem.current.currentSelectedGameObject) return;  //ENABLE THIS IF YOU DONT WANT TO IGNORE UI
            MouseRelease();
        }


        if (shootWhileMoving)
            return;

        if (rb.velocity.magnitude < 0.7f)
        {
            rb.velocity = new Vector2(0, 0); //ENABLE THIS IF YOU WANT THE BALL TO STOP IF ITS MOVING SO SLOW
            canShoot = true;
        }
    }

    private void OnMouseEnter()
    {
        CheckFishOutline(true);
    }


    private void OnMouseExit()
    {
        if (!isCharging)
            CheckFishOutline(false);
    }

    private void ConnectingRatingFish()
    {
        if (fishInfo != null) return;

        switch (rating)
        {
            case DefineManager.FishRating.BRONZE:
                fishInfo = Resources.Load<FishInformationSO>("SO/Bronze");
                fishImageNum = 0;
                break;

            case DefineManager.FishRating.SLIVER:
                fishInfo = Resources.Load<FishInformationSO>("SO/Sliver");
                fishImageNum = 1;
                break;

            case DefineManager.FishRating.PLATINUM:
                fishInfo = Resources.Load<FishInformationSO>("SO/Platinum");
                fishImageNum = 2;
                break;

            case DefineManager.FishRating.DIAMOND:
                fishInfo = Resources.Load<FishInformationSO>("SO/Diamond");
                fishImageNum = 3;
                break;
        }

        fishManagerSO = Resources.Load<FishManagerSO>("SO/FishManager");
    }

    // MOUSE INPUTS
    void MouseClick()
    {
        isCharging = true;
        fishManagerSO.currrentFish = fishInfo;

        ParticleManager.Instance.AddParticle(ParticleManager.ParticleType.ScreenClick, transform.position);
        ParticleManager.Instance.AddParticle(ParticleManager.ParticleType.ScreenClickRing, transform.position);
        SoundManager.Instance.SoundAudio(4);

        if (shootWhileMoving)
        {
            Vector2 dir = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.right = dir * 1;

            startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else
        {
            if (canShoot)
            {
                Vector2 dir = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.right = dir * 1;

                startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }

    }
    void MouseDrag()
    {
        if (shootWhileMoving)
        {
            LookAtShootDirection();
            DrawLine();

            if (showLineOnScreen)
                DrawScreenLine();

            float distance = Vector2.Distance(currentMousePos, startMousePos);

            if (distance > 1)
            {
                if (showLineOnScreen)
                    screenLine.enabled = true;
            }
        }
        else
        {
            if (canShoot)
            {
                LookAtShootDirection();
                DrawLine();

                if (showLineOnScreen)
                    DrawScreenLine();

                float distance = Vector2.Distance(currentMousePos, startMousePos);

                if (distance > 1)
                {
                    if (showLineOnScreen)
                        screenLine.enabled = true;
                }
            }
        }

    }
    void MouseRelease()
    {
        isCharging = false;
        fishManagerSO.currrentFish = null;

        if (shootWhileMoving /*&& !EventSystem.current.IsPointerOverGameObject()*/)
        {
            Shoot();
            screenLine.enabled = false;
        }
        else
        {
            if (canShoot /*&& !EventSystem.current.IsPointerOverGameObject()*/)
            {
                Shoot();
                screenLine.enabled = false;
            }
        }

        CheckFishOutline(false);
    }


    // ACTIONS  
    void LookAtShootDirection()
    {
        Vector3 dir = startMousePos - currentMousePos;

        if (forwardDraging)
        {
            transform.right = dir * -1;
        }
        else
        {
            transform.right = dir;
        }


        float dis = Vector2.Distance(startMousePos, currentMousePos);
        dis *= 4;


        if (dis < maxPower)
        {
            direction.localPosition = new Vector2(dis / 6, 0);
            shootPower = dis;
        }
        else
        {
            shootPower = maxPower;
            direction.localPosition = new Vector2(maxPower / 6, 0);
        }

    }
    public void Shoot()
    {
        canShoot = false;
        rb.velocity = transform.right * shootPower;
    }


    void DrawScreenLine()
    {
        screenLine.positionCount = 1;
        screenLine.SetPosition(0, startMousePos);


        screenLine.positionCount = 2;
        screenLine.SetPosition(1, currentMousePos);
    }

    void DrawLine()
    {

        startPosition = transform.position;

        targetPosition = direction.transform.position;
        currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    Vector3[] positions;

    private void CheckFishOutline(bool _boolen)
    {
        if (GameManager.Instance.gameState != DefineManager.GameState.PLAYING /*|| isDie*/)
        {
            return;
        }

        fishManagerSO.mouseOnFish = fishInfo;

        if (fishManagerSO.currrentFish != null)
        {
            fishManagerSO.mouseOnFish = fishManagerSO.currrentFish;
            return;
        }

        if (isMousePointOn == _boolen)
        {
            return;
        }

        UIManager.Instance.FishUION(fishImageNum);

        isMousePointOn = _boolen;

        outline.enabled = _boolen;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(ConstantManager.TAG_SHARK))
        {
            if (GameManager.Instance.gameState != DefineManager.GameState.PLAYING) return;
            if (isDie || GameManager.Instance.isFishDie) return;
            isDie = true;

            isCharging = false;
            fishManagerSO.currrentFish = null;

            CheckFishOutline(false);
            SoundManager.Instance.SoundAudio(6);

            UIManager.Instance.FishiUIReset();

            Destroy(gameObject);

            GameManager.Instance.isFishDie = true;

            StageManager.Instance.StageStop();
            GameManager.Instance.ChangeGameState(DefineManager.GameState.DONTCLEAR);
            UIManager.Instance.FishAttackEffect();


            SoundManager.Instance.SoundAudio(1);
            ParticleManager.Instance.AddParticle(ParticleManager.ParticleType.BubbleParticle, new Vector3(transform.position.x, transform.position.y, -2f));
            UIManager.Instance.GameDonClear();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

    }
}
