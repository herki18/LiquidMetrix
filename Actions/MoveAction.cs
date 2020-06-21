using System.Collections.Generic;
using LiquidMetrix.Data;

namespace LiquidMetrix.Actions
{
    public struct MoveAction : IAction
    {
        public List<Move> MoveActions;

        public MoveAction(List<Move> moveActions)
        {
            MoveActions = moveActions;
        }
    }
}
