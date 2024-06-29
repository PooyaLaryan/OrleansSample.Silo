using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;
using OrleansSample.Grains.Model;
using OrleansSample.IGrains;

namespace OrleansSample.Grains
{
    public class RobotGrain : Grain, IRobotGrain
    {
        private Queue<string> instructions = new Queue<string>();
        IPersistentState<RobotState> state;

        public RobotGrain([PersistentState("robotState", "robotStateStore")]IPersistentState<RobotState> state)
        {
            this.state = state;
        }
        public async Task AddInstruction(string instruction)
        {
            var key = this.GetPrimaryKeyString();

            state.State.Instructions.Enqueue(instruction);
            await state.WriteStateAsync();
        }

        public Task<int> GetInstructionCount()
        {
            return Task.FromResult(5); //state.State.Instructions.Count
        }

        public async Task<string> GetNextInstruction()
        {
            if (this.state.State.Instructions.Count == 0)
            {
                return null;
            }
            var instruction = this.state.State.Instructions.Dequeue();

            /*var key = this.GetPrimaryKeyString();
            logger.LogWarning($"{key} returning '{instruction}'");*/
            
            await state.WriteStateAsync();
            return instruction;
        }
    }
}
