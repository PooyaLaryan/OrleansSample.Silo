﻿using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Runtime;
using Orleans.Streams;
using OrleansSample.Grains.Model;
using OrleansSample.IGrains;

namespace OrleansSample.Grains
{
    public class RobotGrain : Grain, IRobotGrain
    {
        private Queue<string> instructions = new Queue<string>();
        private IPersistentState<RobotState> state;
        private readonly ILogger<RobotGrain> logger;
        private IAsyncStream<InstructionMessage> stream;
        private string key = string.Empty;
        public RobotGrain([PersistentState("robotState", "robotStateStore")] IPersistentState<RobotState> state, ILogger<RobotGrain> logger)
        {
            this.state = state;
            this.logger = logger;

            this.stream = GetStreamProvider("SMSProvider")
                .GetStream<InstructionMessage>(Guid.Empty, "StartingInstruction");
        }

        Task Publish(string instruction)
        {
            var message = new InstructionMessage(instruction, key);

            return this.stream.OnNextAsync(message);
        }

        public async Task AddInstruction(string instruction)
        {
            var key = this.GetPrimaryKeyString();

            state.State.Instructions.Enqueue(instruction);
            await state.WriteStateAsync();
        }

        public Task<int> GetInstructionCount()
        {
            return Task.FromResult(state.State.Instructions.Count);
        }

        public async Task<string> GetNextInstruction()
        {
            if (this.state.State.Instructions.Count == 0)
            {
                return null;
            }
            var instruction = this.state.State.Instructions.Dequeue();

            var key = this.GetPrimaryKeyString();
            logger.LogWarning($"{key} returning '{instruction}'");

            await this.Publish(instruction);

            await state.WriteStateAsync();
            return instruction;
        }
    }
}
