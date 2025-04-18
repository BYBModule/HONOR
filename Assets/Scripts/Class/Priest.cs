using UnityEngine;

public class Priest : MonoBehaviour
{
    public Status status;
    public void ClassAbility()
    {
    }
    public Priest()
    {
        this.status.maxHp = 0;

    }
    public void Set_Status(Player player, Status status)
    {
        player.status = status;
    }
}
