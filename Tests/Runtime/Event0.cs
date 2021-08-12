using NUnit.Framework;
using UnityEngine;
using lisandroct.EventSystem;
using NSubstitute;

namespace Given_Event0
{
    public class Given {
        protected GameEvent Event { get; private set; }

        [SetUp]
        public virtual void SetUp() {
            Event = ScriptableObject.CreateInstance<GameEvent>();
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

            Event.Register(Substitute.For<IListener>());
        }

        [Test]
        public void Then_HasOneListener() {
            Assert.AreEqual(1, Event.HandlersCount);
        }
    }

    public class When_ListenerRegisteredTwoTimes : Given {
        public override void SetUp() {
            base.SetUp();

            IListener listener = Substitute.For<IListener>();
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

            IListener listener = Substitute.For<IListener>();
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

            Event.Register(Substitute.For<IListener>());
            Event.Register(Substitute.For<IListener>());
        }

        [Test]
        public void Then_HasTwoListeners() {
            Assert.AreEqual(2, Event.HandlersCount);
        }
    }
}

namespace Given_Event0.Given_ThreeListenersRegisteredAndOneUnregistered
{
    public class Given : Given_Event0.Given
    {
        protected IListener listener0 { get; private set; }
        protected IListener listener1 { get; private set; }
        protected IListener listener2 { get; private set; }

        public override void SetUp() {
            base.SetUp();
            
            listener0 = Substitute.For<IListener>();
            listener1 = Substitute.For<IListener>();
            listener2 = Substitute.For<IListener>();
            
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

            Event.Invoke();
        }

        [Test]
        public void Then_OnlyTheTwoRegisteredListenersGetCalled() {
            listener0.Received().OnEventRaised();
            listener1.DidNotReceive().OnEventRaised();
            listener2.Received().OnEventRaised();
        }
    }
}