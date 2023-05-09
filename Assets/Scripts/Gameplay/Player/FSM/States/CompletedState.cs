
namespace Gameplay.Player.FSM.States
{
    public class CompletedState : BasePlayerState
    {
        public CompletedState(IPlayerContextSwitcher context, IPlayer agent) : base(context, agent)
        {
        }


        public override void OnStart()
        {
            Util.Debug.PrintColor($"Complete", UnityEngine.Color.yellow); 
            _agent.Complete(); 
        }
        

        public override void OnStop()
        {
        }
    }
}