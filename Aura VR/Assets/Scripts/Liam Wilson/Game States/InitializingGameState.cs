namespace AuraHull.AuraVRGame
{
    public class InitializingGameState : BaseGameState
    {
        public override void InitState()
        {
            base.InitState();

            GameModel.Instance.BuildPlayer();
            GameModel.Instance.ConfigureColliders();

            GameModel.Instance.ChangeGameState(new PlayingGameState());
        }
    }
}