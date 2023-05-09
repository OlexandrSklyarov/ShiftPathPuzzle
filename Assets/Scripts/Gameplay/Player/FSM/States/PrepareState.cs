using UnityEngine;

namespace Gameplay.Player.FSM.States
{
    public class PrepareState : BasePlayerState
    {
        public PrepareState(IPlayerContextSwitcher context, IPlayer agent) : base(context, agent)
        {
        }


        public override void OnStart()
        {
            Util.Debug.PrintColor($"player Prepare", Color.yellow);       
            _context.SwitchState<ControlState>();     
        }
        

        public override void OnStop()
        {
        }
    }
}