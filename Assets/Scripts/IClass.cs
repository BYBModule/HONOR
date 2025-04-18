using UnityEngine;

public interface IClass
{
    public void ClassAbility();
    public void Set_Status(Player player, Status status)
    {
        player.status = status;
    }
}
