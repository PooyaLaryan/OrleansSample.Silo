using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;
using Orleans.Streams;
using OrleansSample.Grains.Model;
using OrleansSample.IGrains;

namespace OrleansSample.Grains
{
    public class RobotGrainStateless : Grain, IRobotGrainStateless
    {
        private Queue<string> instructions = new Queue<string>();
        private readonly ILogger<RobotGrain> logger;
        private IAsyncStream<InstructionMessage> stream;
        private string key = string.Empty;
        public RobotGrainStateless(ILogger<RobotGrain> logger)
        {
            this.logger = logger;

            this.stream = GetStreamProvider("SMSProvider")
                .GetStream<InstructionMessage>(Guid.Empty, "StartingInstruction");
        }

        Task Publish(string instruction)
        {
            var message = new InstructionMessage(instruction, key);

            return this.stream.OnNextAsync(message);
        }

        public Task AddInstruction(string instruction)
        {
            var key = this.GetPrimaryKeyString();
            this.instructions.Enqueue(instruction);
            return Task.CompletedTask;
        }

        public Task<int> GetInstructionCount()
        {
            return Task.FromResult(this.instructions.Count);
        }

        public async Task<string> GetNextInstruction()
        {
            if (this.instructions.Count == 0)
            {
                return null;
            }

            var instruction = this.instructions.Dequeue();
            await this.Publish(instruction);

            return instruction;
        }
    }
}
