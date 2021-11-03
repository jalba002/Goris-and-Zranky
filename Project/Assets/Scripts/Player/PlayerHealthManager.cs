using Sirenix.OdinInspector;

public class PlayerHealthManager : HealthManager
{
    public void Start()
    {
        //onCharacterDeath.AddListener(GameManager.GM.PlayerDied);
    }

    [Button("Force Respawn")]
    public override void Respawn()
    {
        onCharacterRespawn.Invoke();
    }
}

