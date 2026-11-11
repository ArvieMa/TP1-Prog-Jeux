using JetBrains.Annotations;
using NUnit.Framework.Constraints;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public float JumpForce = 17;
    
    public float MoveSpeed = 120; 
    public float MaxSpeed = 8;
    public float SprintMaxSpeed = 12;
    
    public LayerMask GroundLayer;

    private Rigidbody2D m_rigidBody;
    private Animator m_animator;
    private SpriteRenderer m_spriteRenderer;

    [SerializeField]
    private Transform m_feetPosition;
    private bool m_isGrounded;
    private float m_grip = 1f;
    //Gestion des inputs
    private bool m_userJump;
    public int MaxJumps = 2;
    private int m_jumpsLeft;
    private float m_jumpLockout;
    private InputAction m_jumpAction;
    private InputAction m_moveAction;
    private InputAction m_sprintAction;

    //Dash stuff
    public float DashSpeed = 25f;    
    public float DashDuration = 0.3f;
    public float DashCooldown = 1f;

    private InputAction m_dashAction;
    private bool m_userDash;
    private bool m_isDashing;
    private float m_dashTimer;
    private float m_dashCooldownTimer;
    private float m_dashDirection;
    private float m_defaultGravity;


    private void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody2D>();
        m_defaultGravity = m_rigidBody.gravityScale;
        m_animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        

        m_jumpAction = InputSystem.actions.FindAction("Jump"); //project settings, input manager pour trouver le nom
        m_moveAction = InputSystem.actions.FindAction("Move");
        m_sprintAction = InputSystem.actions.FindAction("Sprint");
        m_dashAction = InputSystem.actions.FindAction("Dash");

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (m_jumpAction.WasPressedThisFrame() && m_jumpsLeft > 0 && !m_isDashing)
        {
            m_userJump = true;
        }

        if(m_dashAction.WasPressedThisFrame() && !m_userJump && m_dashCooldownTimer <= 0)
        {
            m_userDash = true;
        }
    }

    private void FixedUpdate()
    {
        if (m_userJump)
        {
            m_rigidBody.linearVelocity = new Vector2(m_rigidBody.linearVelocity.x, 0f); //Reset la vitesse à 0 dans les airs 
            m_rigidBody.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
            m_userJump = false;
            m_jumpsLeft--;
            m_jumpLockout = 0.1f; //duree
            if (m_jumpsLeft == 1)
                m_animator.SetTrigger("Jump");
            if (m_jumpsLeft == 0)
                m_animator.SetTrigger("DoubleJump"); //même animation mais regle un problème


        }
        m_jumpLockout -= Time.fixedDeltaTime;//pour count down le timer

        //Gestion mouvement gauche droite
        float t_move = m_moveAction.ReadValue<Vector2>().x;


       //gestion flip du sprite
       

        //dash timer comme le jump
        m_dashCooldownTimer -= Time.fixedDeltaTime;


        if (m_userDash) //set up le dash et tout le nécéssaire
        {
            m_userDash = false;
            m_isDashing = true;
            m_dashTimer = DashDuration;
            m_dashCooldownTimer = DashCooldown;
            
            m_dashDirection = t_move != 0 ? Mathf.Sign(t_move) : (m_spriteRenderer.flipX ? -1f : 1f); //Permet dash dans direction que l'on tient enfoncé, sinon dans celle que l'on regarde)
            m_rigidBody.gravityScale = 0f; //Désactive la gravité pendant le dash pour que ca reste une ligne droite
            m_animator.SetTrigger("Dash");
        }
        if (m_isDashing) //le mouvement du dash lui-même
        {
            m_rigidBody.linearVelocity = new Vector2(m_dashDirection * DashSpeed, 0f);
            m_dashTimer -= Time.fixedDeltaTime;

            if (m_dashTimer <= 0f)
            {
                m_isDashing = false;
                m_rigidBody.gravityScale = m_defaultGravity;
                m_rigidBody.linearVelocity = new Vector2(m_dashDirection * MaxSpeed, 0f); //ajouté car nyoom un peu trop après le dash, c'était cool mais un peu trop
            }


            
        }

        if (!m_isDashing)
        {
            if (t_move != 0)
                m_spriteRenderer.flipX = t_move <= 0; //d.placé ici aussi pour que le personnage ne puisse pas tourner pendant dash
            //Calcule et application du vecteur de force
            //Je l'ai entièrement rentrer ici pour ne pas que le mouvemnt interfère avec le dash
            float t_effectiveMaxSpeed = (m_sprintAction.IsPressed() ? SprintMaxSpeed : MaxSpeed);// (condition ? Iftrue : ifnot)

            float t_velocityGap = t_effectiveMaxSpeed - m_rigidBody.linearVelocity.x * Mathf.Sign(t_move);
            float t_scale = Mathf.Clamp01(t_velocityGap / t_effectiveMaxSpeed);

            Vector2 t_force = new Vector2(t_move * MoveSpeed * t_scale * m_grip, 0);
            m_rigidBody.AddForce(t_force);
            if (t_move == 0 && m_isGrounded)
                m_rigidBody.AddForce(new Vector2(-m_rigidBody.linearVelocity.x * 10f * m_grip, 0));
        }


       

        //Détection du sol
        Collider2D t_collider = Physics2D.OverlapCircle(m_feetPosition.position, 0.2f, GroundLayer.value);
        m_grip = (t_collider != null && t_collider.TryGetComponent(out Ice t_surface)) ? t_surface.Grip : 1f;
        m_isGrounded = t_collider != null;
        if (m_isGrounded && m_jumpLockout <= 0f)
            m_jumpsLeft = MaxJumps;//redonne les jumps une fois que le personnage touche au sol                
        else if (!m_isGrounded && m_jumpsLeft == MaxJumps)
            m_jumpsLeft = MaxJumps - 1;//enlever le jump même si il ne fait que walk off d'une plateforme, empêche d'avoir 2 double jumps'           
        m_animator.SetBool("Grounded", m_isGrounded);
        
        //Passage de la vitesse à l'animator
        m_animator.SetFloat("Speed", Mathf.Abs(t_move * ((m_sprintAction.IsPressed() ? 2 : 1))));

       

    }

    public void notifyDeath()
    {
        m_animator.SetTrigger("Dead");
    }
}
