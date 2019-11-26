namespace Collectibles
{
    public class Minimizer : Collectible
    {
        public float ratio;
        override public void OnCollect()
        {
            this.target.gameObject.transform.localScale *= ratio;
        }
    }
}
