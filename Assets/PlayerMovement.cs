using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private Transform player;
    public float speed = 5.0f;
    private Vector3 networkPosition;

    private void Update()
    {
        if (IsOwner) // burası objeyi kontrol eden kişide çalışır
        {
            LocalMoving();
            SentPositionToServerRpc(player.transform.position);
        }
        else // burası objeyi kontrol etmeyen kişide çalışır
        {
            player.transform.position = networkPosition;
        }
    }

    private void LocalMoving()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, verticalInput);
        player.transform.Translate(movement * speed * Time.deltaTime);
    }

    [ServerRpc]
    private void SentPositionToServerRpc(Vector3 position)
    {
        SentPositionFromClientRpc(position);
    }

    [ClientRpc]
    private void SentPositionFromClientRpc(Vector3 position)
    {
        // burası Hareket eden kişide True döner, hareket etmeyen kişide False döner.
        // Böylece sadece hareket eden kişinin hareket Data'sı diger oyunculara yollanır
        if (IsOwner)
            return;

        networkPosition = position;
    }
}