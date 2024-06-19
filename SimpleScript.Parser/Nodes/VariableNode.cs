﻿using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes
{
    public class VariableNode : IAddable, IMultiplyable
    {
        public string Name { get; set; }
        public VariableNode(string name)
        {
            Name = name;
        }
    }
}