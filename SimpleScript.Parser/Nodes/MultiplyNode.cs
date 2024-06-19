﻿using SimpleScript.Parser.Nodes.Interfaces;

namespace SimpleScript.Parser.Nodes
{
    public class MultiplyNode : IBinaryOperation, IExpression
    {
        public List<IMultiplyable> ChildNodes { get; set; } = [];
    }
}