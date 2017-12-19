using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Utility;
using UnityEngine;

public class PlayerMovement : Photon.MonoBehaviour {

    private PhotonView photonView;
    Vector3 realPosition = Vector3.zero;
    Quaternion realRotation = Quaternion.identity;
    public GameObject myChar;
    public GameObject standbyCamera;
    private Camera myFpsCamera;
    private int Health;

    [SerializeField]
    private UnityStandardAssets.Characters.FirstPerson.MouseLook m_MouseLook;

    CharacterController charController;
	// Use this for initialization
	void Awake () {

        photonView = GetComponent<PhotonView>();
        Health = 100;
        if (photonView.isMine)
        {
            myFpsCamera = myChar.GetComponent<Camera>();
            standbyCamera = GameObject.Find("StandbyCamera");
            standbyCamera.SetActive(false);
            myChar.SetActive(true);
            m_MouseLook.Init(transform, myFpsCamera.transform);

        }
    }

    // Update is called once per frame
    void Update() {
        if (photonView.isMine)
        {
            CheckInput();
            RotateView();
        }
        else
        {
            SmoothMove();
        }
	}

    private void RotateView()
    {
        m_MouseLook.LookRotation(transform, myFpsCamera.transform);
    }

    private void FixedUpdate()
    {
        m_MouseLook.UpdateCursorLock();
    }

    public void setHealth(int health)
    {
        Health = health;
    }
    public int getHealth()
    {
        return Health;
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            realPosition = (Vector3)stream.ReceiveNext();
            realRotation = (Quaternion)stream.ReceiveNext();
        }
    }

    private void SmoothMove()
    {
        transform.position = Vector3.Lerp(transform.position, realPosition, .25f);
        transform.rotation = Quaternion.Lerp(transform.rotation, realRotation, .25f);
    }

    private void CheckInput()
    {
        float moveSpeed = 10f;

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        transform.position += transform.forward * (vertical * moveSpeed * Time.deltaTime);
        transform.position += transform.right * (horizontal * moveSpeed * Time.deltaTime);
    }
}
