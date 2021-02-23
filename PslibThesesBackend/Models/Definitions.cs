using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PslibThesesBackend.Models
{
    public static class Definitions
    {
        public static readonly Dictionary<WorkState, List<WorkState>> StateTransitions = new Dictionary<WorkState, List<WorkState>>
        {
            { WorkState.InPreparation, new List<WorkState> {WorkState.WorkedOut} },
            { WorkState.WorkedOut, new List<WorkState> {WorkState.Completed, WorkState.Failed} },
            { WorkState.Completed, new List<WorkState> {WorkState.Undefended, WorkState.Succesful} },
            { WorkState.Failed, new List<WorkState> {} },
            { WorkState.Succesful, new List<WorkState> {} },
            { WorkState.Undefended, new List<WorkState> {} }
        };
    }
}
