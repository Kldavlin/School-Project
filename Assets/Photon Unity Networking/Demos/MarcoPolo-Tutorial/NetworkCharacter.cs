using UnityEngine;

public class NetworkCharacter : Photon.MonoBehaviour
{
    private Vector3 correctPlayerPos = Vector3.zero; // We lerp towards this
    private Quaternion correctPlayerRot = Quaternion.identity; // We lerp towards this
    private Vector3 myDirection = Vector3.zero;
    float speed = 10f;
    CharacterController charController;

    private void Start()
    {
        charController = GetComponent<CharacterController>();
    }
    // Update is called once per frame
    void Update()
    {
        if (photonView.isMine)
        {
            myDirection = new Vector3(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical")).normalized;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
        }
    }

    private void FixedUpdate()
    {
        if (photonView.isMine)
        {
            charController.SimpleMove(myDirection * speed);
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);

            myThirdPersonController myC = GetComponent<myThirdPersonController>();
            stream.SendNext((int)myC._characterState);
        }
        else
        {
            // Network player, receive data
            this.correctPlayerPos = (Vector3)stream.ReceiveNext();
            this.correctPlayerRot = (Quaternion)stream.ReceiveNext();

            myThirdPersonController myC = GetComponent<myThirdPersonController>();
        }
    }
}