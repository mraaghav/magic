﻿/*
 * Magic, Copyright(c) Thomas Hansen 2019 - thomas@gaiasoul.com
 * Licensed as Affero GPL unless an explicitly proprietary license has been obtained.
 */

using System;
using System.Linq;
using System.Collections.Generic;
using magic.node;
using magic.node.extensions;
using magic.signals.contracts;

namespace magic.lambda.change
{
    [Slot(Name = "set-x")]
    public class SetX : ISlot, IMeta
    {
        readonly ISignaler _signaler;

        public SetX(ISignaler signaler)
        {
            _signaler = signaler ?? throw new ArgumentNullException(nameof(signaler));
        }

        public void Signal(Node input)
        {
            if (input.Children.Count() > 1)
                throw new ApplicationException("[set-x] can have maximum one child node");

            var destinations = input.Evaluate().ToList();
            if (destinations.Count == 0)
                return;

            _signaler.Signal("eval", input);

            var source = input.Children.FirstOrDefault()?.GetEx(_signaler, false);
            foreach (var idx in destinations)
            {
                idx.Value = source;
            }
        }

        public IEnumerable<Node> GetArguments()
        {
            yield return new Node(":", "x");
            yield return new Node("*", 1);
        }
    }
}
