using NUnit.Framework;
using UnityEngine;
using lisandroct.EventSystem;
using NSubstitute;

namespace Given_Event1
{
    public class Event1 : Event<int> { }
    
    public class Given {
        protected Event1 Event { get; private set; }

        [SetUp]
        public virtual void SetUp() {
            Event = ScriptableObject.CreateInstance<Event1>();
        }
    }

    public class When_Created : Given {
        [Test]
        public void Then_HasNoListeners() {
            Assert.AreEqual(0, Event.HandlersCount);
        }
    }

    public class When_ListenerRegistered : Given {
        public override void SetUp() {
            base.SetUp();

            Event.Register(Substitute.For<IListener<int>>());
        }

        [Test]
        public void Then_HasOneListener() {
            Assert.AreEqual(1, Event.HandlersCount);
        }
    }

    public class When_ListenerRegisteredTwoTimes : Given {
        public override void SetUp() {
            base.SetUp();

            var listener = Substitute.For<IListener<int>>();
            Event.Register(listener);
            Event.Register(listener);
        }

        [Test]
        public void Then_HasOneListener() {
            Assert.AreEqual(1, Event.HandlersCount);
        }
    }

    public class When_ListenerRegisteredAndUnregistered : Given {
        public override void SetUp() {
            base.SetUp();

            var listener = Substitute.For<IListener<int>>();
            Event.Register(listener);
            Event.Unregister(listener);
        }

        [Test]
        public void Then_HasNoListener() {
            Assert.AreEqual(0, Event.HandlersCount);
        }
    }

    public class When_TwoListenersRegistered : Given
    {
        public override void SetUp() {
            base.SetUp();

            Event.Register(Substitute.For<IListener<int>>());
            Event.Register(Substitute.For<IListener<int>>());
        }

        [Test]
        public void Then_HasTwoListeners() {
            Assert.AreEqual(2, Event.HandlersCount);
        }
    }
}

namespace Given_Event1.Given_ThreeListenersRegistered
{
    public class Given : Given_Event1.Given
    {
        protected IListener<int> listener0 { get; private set; }
        protected IListener<int> listener1 { get; private set; }
        protected IListener<int> listener2 { get; private set; }

        public override void SetUp() {
            base.SetUp();
            
            listener0 = Substitute.For<IListener<int>>();
            listener1 = Substitute.For<IListener<int>>();
            listener2 = Substitute.For<IListener<int>>();
            
            Event.Register(listener0);
            Event.Register(listener1);
            Event.Register(listener2);
            
            Event.Unregister(listener1);
        }
    }

    public class When_Raised : Given
    {
        public override void SetUp() {
            base.SetUp();

            Event.Invoke(0);
        }

        [Test]
        public void Then_OnlyTheTwoRegisteredListenersGetCalled() {
            listener0.Received().OnEventRaised(0);
            listener1.DidNotReceive().OnEventRaised(0);
            listener2.Received().OnEventRaised(0);
        }
    }
}