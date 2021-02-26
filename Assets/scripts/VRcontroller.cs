using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


[RequireComponent(typeof(CharacterController))]
public class VRcontroller : MonoBehaviour
{

    //Controller variables
    [SerializeField] private bool m_IsWalking;
    [SerializeField] public float m_WalkSpeed = 2;
    [SerializeField] public float m_RunSpeed = 10;
    [SerializeField] private float m_JumpSpeed = 10;
    [SerializeField] private float m_StickToGroundForce = 10;
    [SerializeField] private float m_GravityMultiplier = 2;

    //SteamVr Input
    public SteamVR_ActionSet ac;
    public SteamVR_Action_Vector2 joystick;
    public SteamVR_Action_Boolean jump;
    public SteamVR_Action_Boolean run;
    public SteamVR_Action_Boolean walkbutton;


    //script variables
    private bool m_Jumping; //is jumping
    private Camera m_Camera; //Controller camera
    private bool m_Jump; //to jump
    private Vector2 m_Input; //steam vr input
    private Vector3 m_MoveDir = Vector3.zero; //calculated move dir
    private CharacterController m_CharacterController; //character controller
    private CollisionFlags m_CollisionFlags; //CollisionFlags
    private bool m_PreviouslyGrounded; //spreekt voor zich
    private float originalForward = 0f; //original forward
    public Transform thecamera; //De vr camera
    public GameObject handL; //Linker vr hand
    public GameObject handR; //Rechter vr hand
    public float walk = 0; //walk counter
    public float stepsize = 1; //stap groote
    bool isAbove = false;
    public bool UseMotionControls = true;


    //Setup
    void Start()
    {
        m_Jumping = false;
        m_CharacterController = GetComponent<CharacterController>();
        m_Camera = Camera.main;
        ac.Activate(SteamVR_Input_Sources.Any);
        SteamVR_Fade.Start(new Color(0.8f, 0.8f, 0.8f), 0f);
        //set and start fade to
        SteamVR_Fade.Start(Color.clear, 5f);
    }

    // Update
    void Update()
    {

        //pos
        Vector3 pos = thecamera.transform.localPosition;

        //controller code
        m_CharacterController.center = new Vector3(pos.x, m_CharacterController.center.y,pos.z);
        if(!m_Jump)
        {
            m_Jump = jump.GetStateDown(SteamVR_Input_Sources.Any);
        }
        if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
        {
            m_MoveDir.y = 0f;
            m_Jumping = false;
        }
        if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
        {
            m_MoveDir.y = 0f;
        }

        m_PreviouslyGrounded = m_CharacterController.isGrounded;

        if(UseMotionControls) {
            resetwalk();
            if (handL.transform.position.y > handR.transform.position.y)
            {
                if (isAbove)
                {
                    walk+= 0.3f;
                    isAbove = false;
                }
            }
            if (handL.transform.position.y < handR.transform.position.y)
            {
                if (!isAbove)
                {
                    walk+= 0.3f;
                    isAbove = true;
                }
            }
        }
    }

    void resetwalk()
    {
        if (walk > 0) {
            walk = walk * 0.95f;
            if(walk < 0.1)
            {
                walk = 0;
            }
        }
    }


    //Old detection code
    private void FixedUpdate()
    {
        float speed = m_WalkSpeed;
        GetInput(out speed);
        Vector3 desiredMove = Vector3.zero;
        if (walk > 0 && walkbutton.GetLastState(SteamVR_Input_Sources.Any)) { 
            desiredMove = thecamera.transform.forward;
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                               m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;
            if(walk > 3)
            {
                walk = 3;
            }
            m_MoveDir.x = desiredMove.x * speed * walk * stepsize;
            m_MoveDir.z = desiredMove.z * speed * walk * stepsize;
        } else
        {
            desiredMove = transform.forward * (m_Input.x) + transform.right * (-m_Input.y);
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                               m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;
            m_MoveDir.x = desiredMove.x * speed * stepsize;
            m_MoveDir.z = desiredMove.z * speed * stepsize;
        }

        if (m_CharacterController.isGrounded)
        {
            m_MoveDir.y = -m_StickToGroundForce;

            if (m_Jump)
            {
                m_MoveDir.y = m_JumpSpeed;
                m_Jump = false;
                m_Jumping = true;
            }
        }
        else
        {
            m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
        }
        m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);
    }

    private Vector2 lastjoystick = new Vector2(0f, 0f);

    private void GetInput(out float speed)
    {
        if(joystick.GetAxis(SteamVR_Input_Sources.Any) != lastjoystick && lastjoystick == new Vector2(0f, 0f))
        {
            originalForward = thecamera.rotation.eulerAngles.y;
        }
        lastjoystick = joystick.GetAxis(SteamVR_Input_Sources.Any);
        Vector2 input = joystick.GetAxis(SteamVR_Input_Sources.Any);
        Vector3 ii = new Vector3(input.x, 0, input.y);
        ii = Quaternion.Euler(0, originalForward, 0) * ii;
        input.x = ii.x;
        input.y = ii.z;
        float horizontal = input.x;
        float vertical = input.y;
        bool waswalking = m_IsWalking;
        m_IsWalking = !run.GetState(SteamVR_Input_Sources.Any);
        speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
        m_Input = new Vector2(horizontal, vertical);
        if (m_Input.sqrMagnitude > 1)
        {
            m_Input.Normalize();
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (m_CollisionFlags == CollisionFlags.Below)
        {
            return;
        }

        if (body == null || body.isKinematic)
        {
            return;
        }
        body.AddForceAtPosition(m_CharacterController.velocity * 0.1f, hit.point, ForceMode.Impulse);
    }

}
