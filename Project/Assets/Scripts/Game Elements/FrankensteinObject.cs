public class FrankensteinObject : PickableObject
{
    public override void Deactivate()
    {
        meshRenderer.enabled = false;
        Disconnect();
        this.gameObject.SetActive(false);
    }
}