using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrleansSample.IGrains
{
    public interface IRobotGrainStateless : IGrainWithStringKey
    {
        Task AddInstruction(string instruction);
        Task<string> GetNextInstruction();
        Task<int> GetInstructionCount();
    }
}
