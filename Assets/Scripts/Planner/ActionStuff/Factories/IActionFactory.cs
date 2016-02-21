using System.Collections.Generic;

namespace GOAP
{
    public interface IActionFactory
    {
        IEnumerable<PlannerAction> GetActions(Dictionary<string, object> knowledge);
    }
}