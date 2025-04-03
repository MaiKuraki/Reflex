using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Reflex.Core;

namespace Reflex.EditModeTests
{
    internal class EnumerableInjectionTests
    {
        private class NumberManager
        {
            public IEnumerable<int> Numbers { get; }

            public NumberManager(IEnumerable<int> numbers)
            {
                Numbers = numbers;
            }
        }
        
        [Test]
        public void Container_ShouldBeAbleToConstructObjectWithIEnumerableDependency()
        {
            var container = new ContainerBuilder()
                .Add(Singleton.FromValue(1))
                .Add(Singleton.FromValue(2))
                .Add(Singleton.FromValue(3))
                .Build();
            
            Action construct = () => container.Construct<NumberManager>();
            construct.Should().NotThrow();
        }

        [Test]
        public void NestedEnumerableShouldBeSupported()
        {
            var container = new ContainerBuilder()
                .Add(Singleton.FromValue(new List<int> {1, 2, 3}))
                .Add(Singleton.FromValue(new List<int> {4, 5, 6}))
                .Build();

            container.All<List<int>>().SelectMany(x => x).Should().BeEquivalentTo(new[] {1, 2, 3, 4, 5, 6});
        }
    }
}