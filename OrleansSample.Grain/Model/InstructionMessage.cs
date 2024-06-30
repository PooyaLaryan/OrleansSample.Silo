using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrleansSample.Grains.Model
{
    public class InstructionMessage
    {
        public InstructionMessage()
        {
        }
        public InstructionMessage(string instruction, string robot)
        {
            this.Instruction = instruction;
            this.Robot = robot;
        }
        public string Instruction { get; set; }
        public string Robot { get; set; }
    }
}
