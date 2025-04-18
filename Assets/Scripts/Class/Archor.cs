using UnityEngine;

public class Archor : MonoBehaviour
{
    public Status status;
    public void ClassAbility()
    {
    }
    public void Set_Status(Player player, Status status)
    {
        player.status = status;
    }
}
