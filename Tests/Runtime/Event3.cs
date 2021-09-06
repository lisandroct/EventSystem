using NUnit.Framework;
using UnityEngine;
using lisandroct.EventSystem;
using NSubstitute;

public class Event3 : GameEvent<int, int, int> { }

namespace Given_Event3
{
    public class Given {
        protected Event3 Event { get; private set; }

        [SetUp]
        public virtual void SetUp() {
            Event = ScriptableObject.CreateInstance<Event3>();
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

            Event.Register(Substitute.For<IListener<int, int, int>>());
        }

        [Test]
        public void Then_HasOneListener() {
            Assert.AreEqual(1, Event.HandlersCount);
        }
    }

    public class When_ListenerRegisteredTwoTimes : Given {
        public override void SetUp() {
            base.SetUp();

            IListener<int, int, int> listener = Substitute.For<IListener<int, int, int>>();
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

            IListener<int, int, int> listener = Substitute.For<IListener<int, int, int>>();
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

            Event.Register(Substitute.For<IListener<int, int, int>>());
            Event.Register(Substitute.For<IListener<int, int, int>>());
        }

        [Test]
        public void Then_HasTwoListeners() {
            Assert.AreEqual(2, Event.HandlersCount);
        }
    }
}

namespace Given_Event3.Given_ThreeListenersRegistered {
    public class Given : Given_Event3.Given
    {
        protected IListener<int, int, int> listener0 { get; private set; }
        protected IListener<int, int, int> listener1 { get; private set; }
        protected IListener<int, int, int> listener2 { get; private set; }

        public override void SetUp() {
            base.SetUp();

            listener0 = Substitute.For<IListener<int, int, int>>();
            listener1 = Substitute.For<IListener<int, int, int>>();
            listener2 = Substitute.For<IListener<int, int, int>>();
            
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

            Event.Invoke(0, 1, 2);
        }

        [Test]
        public void Then_OnlyTheTwoRegisteredListenersGetCalled() {
            listener0.Received().OnEventRaised(0, 1, 2);
            listener1.DidNotReceive().OnEventRaised(0, 1, 2);
            listener2.Received().OnEventRaised(0, 1, 2);
        }
    }
}