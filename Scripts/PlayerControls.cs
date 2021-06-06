using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Photon.Pun;
using UnityEngine.UI;
using static GestureController;

public class PlayerControls : MonoBehaviour, IPunObservable
{
    private PhotonView photonView;
    Animator anim;
    Rigidbody2D rb;

    //кнопки управления
    private GameObject btnLight;

    //Начальные позиции кнопок 
    float PosBtnLight;

    //другие объекты
    public GameObject groundContact;
    public GameObject ammo;
    public GameObject exhaust;

    bool run;
    bool up;
    bool MovToLeft;
    bool MovToRight;
    float x;
    float y;

    public Light someLight;
    public Renderer rend;

    private GameObject camera;
    private GameObject sky;
    private GameObject clouds;
    private float timeShot;
    private bool isGrounded1, isGrounded2;
    public Transform groundCheck1, groundCheck2;
    public float checkRadius;
    public LayerMask whatIsGround;
    public bool onGround;
    public Transform shotDir; 

    private SpriteRenderer spriteRenderer;
    private Vector2 Direction;

    public Image life_scale;
    private GameObject panelWin;
    private GameObject panelLoss;
    private GameObject darkPanel;
    private Text Winner; 

    private GameObject Canvas;
    private LayerMask mask;
    public bool is_death;
    public string dir;
    
    private void Start()
    {
        camera = GameObject.Find("Main Camera");
        sky = GameObject.Find("sky");
        clouds = GameObject.Find("clouds");

        btnLight = GameObject.Find("ButtonLight");
        Canvas = GameObject.Find("CanvasManagement");

        photonView = GetComponent<PhotonView>();
        PosBtnLight = btnLight.transform.position.y;
        
        Direction = Vector2.right;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rend = GetComponent<Renderer>();
        is_death = false;
    }

    void Run()
    {
        //------- run left ----->//
        if (dir == "Left" && onGround){
            rb.velocity = new Vector2(-7, 0);
            anim.SetBool("is_running", true);
            Direction = Vector2.left;
        }
        //------- run right ------->//
        if (dir == "Right" && onGround){
            rb.velocity = new Vector2(7, 0);
            anim.SetBool("is_running", true);
            Direction = Vector2.right;
        }
    }

    void Attack()
    {
        if (dir == "Attack" && timeShot <= 0.5)
        {
            anim.SetBool("is_attack", true);           
        }
        else {
            timeShot -= Time.deltaTime;
            return;
        }   

        if (gameObject.layer == 9)
        {
            mask = LayerMask.GetMask("Player2");
        }
        if (gameObject.layer == 10)
        {
            mask = LayerMask.GetMask("Player1");
        }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Direction, 10, mask);
        if (hit.collider == null)
            return;
        float distance = Mathf.Abs(hit.point.x - transform.position.x);
        GameObject Enemy = hit.collider.gameObject;

        if (Enemy.tag == "Player" && distance < 3)
        { 
            var pv = gameObject.GetComponent<PhotonView>();
            pv.RPC("death", RpcTarget.All, Enemy.name);
        }   
    }
    [PunRPC]
    void death(string enemy){
        GameObject Enemy = GameObject.Find(enemy);
        Animator animEnemy;
        animEnemy = Enemy.GetComponent<Animator>(); 
        animEnemy.SetBool("is_death", true);
        Destroy(Enemy, 1);
    }

    void Jump()
    { 
        if (dir == "Up" && onGround)
        {
            anim.SetBool("is_up", true);
            rb.AddForce(Vector2.up * 1200f);
        }
    }
    void checkingGraund()
    {
        isGrounded1 = Physics2D.OverlapCircle(groundCheck1.position, checkRadius, whatIsGround);
        isGrounded2 = Physics2D.OverlapCircle(groundCheck2.position, checkRadius, whatIsGround);

        if (isGrounded1 || isGrounded2) {
            onGround = true;
        }        
        else
        {
            onGround = false;
        }
    }
    public bool CheckButtonPressed(GameObject btn, float PosBtn){
        if ((btn.transform.position.y - PosBtn) > 5){
            return true;
        }
        else{
            return false;
        }
    }
    //сериализация данных между пользователями
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
    }

    void ProcedureLossLife()
    {
        var pv = gameObject.GetComponent<PhotonView>();
            
        life_scale.fillAmount -= 0.2f;
        if (life_scale.fillAmount < 0.1)
        {
            pv.RPC("won_or_lost", RpcTarget.All);
            pv.RPC("TextWhoWinner", RpcTarget.All);
            anim.SetBool("is_death", true);
        }
    }

    [PunRPC]
    void TextWhoWinner()
    {   
        Winner = GameObject.Find("winner").GetComponent<Text>();
        string PlayerName = PhotonNetwork.NickName;
        Winner.text = PlayerName + " is winner";
    }
    [PunRPC]
    void won_or_lost()
    {
        //table won
        if (!photonView.IsMine)
        {
            darkPanel.transform.localScale = new Vector3(2.3125f,2.3125f,0);  
            panelWin.transform.localScale = new Vector3(2.3125f,2.3125f,0);
        } 
        //table lost
        if (photonView.IsMine)
        {
            darkPanel.transform.localScale = new Vector3(2.3125f,2.3125f,0);
            panelLoss.transform.localScale = new Vector3(2.3125f,2.3125f,0);
        }
    }
    [PunRPC]
    void loss_life()
    {
        life_scale.fillAmount -= 0.2f;
    }
    void OffAnim()
    {
        anim.SetBool("is_running", false);
        anim.SetBool("is_up", false);
        anim.SetBool("is_attack", false);
        if (!is_death)
            anim.SetBool("is_death", false);
    }
    
    [PunRPC]
    void MakeRotation(string direction){
        if (direction == "left")
            transform.rotation = Quaternion.Euler(0, 180, 0);
        if (direction == "right")
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    void direction()
    {
        var pv = gameObject.GetComponent<PhotonView>();
        if (Direction == Vector2.left)
            pv.RPC("MakeRotation", RpcTarget.All, "left");
        if (Direction == Vector2.right)
            pv.RPC("MakeRotation", RpcTarget.All, "right");
    }
    [PunRPC]
    void turnLight(){
        Color colorStart = new Color(0.490566f, 0.490566f, 0.490566f, 1f);
        Color colorEnd = new Color(0.1886792f, 0.1886792f, 0.1886792f, 1f);
        
        if (this.someLight.range > 9.0f){
            Debug.Log("OFF");
            this.someLight.range = 0f;
            this.rend.material.color = colorEnd;
        }
        else{
            Debug.Log("ON");
            this.someLight.range = 25.0f;
            this.rend.material.color = colorStart;
        }
    }
    float timer = 0; 
    void LightStateDetection(){
        var pv = gameObject.GetComponent<PhotonView>();

        timer = timer + 1 * Time.deltaTime;
        if (timer < 0.5)
            return;

        if (CheckButtonPressed(btnLight, PosBtnLight)){
            pv.RPC("turnLight", RpcTarget.All);
            timer = 0;
        }
    }
    void FollowForPlayer(){
        x = transform.position.x;
        y = transform.position.y;
        camera.transform.position = new Vector3(x,y,camera.transform.position.z);
        sky.transform.position = new Vector3(x,y+1.8f,sky.transform.position.z);
        clouds.transform.position = new Vector3(x,y+0.8f,clouds.transform.position.z);
    }

    public void Update()
    {
        if (!photonView.IsMine)
            return;
    
        checkingGraund();
        OffAnim();
        Attack();
        if (dir != GestureController.dir){
            dir = GestureController.dir;
        }

        Run();
        direction();
        FollowForPlayer();
        LightStateDetection();
        if (dir != "Left" && dir != "Right"){
            GestureController.dir = null;
        }
    }

    public void FixedUpdate() 
    {
        Jump();
    }
}
