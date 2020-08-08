using NUnit.Framework;
using UnityEngine;
using lisandroct.EventSystem;
using NSubstitute;
using Event = lisandroct.EventSystem.Event;

namespace Given_GameEvent0
{
    public class Given {
        protected Event Event { get; set; }

        [SetUp]
        public virtual void SetUp() {
            Event = ScriptableObject.CreateInstance<Event>();
        }
    }

    public class When_Created : Given {
        [Test]
        public void Then_HasNoListeners() {
            Assert.AreEqual(0, Event.ListenersCount);
        }
    }

    public class When_ListenerRegistered : Given {
        public override void SetUp() {
            base.SetUp();

            Event.RegisterListener(Substitute.For<IListener>());
        }

        [Test]
        public void Then_HasOneListener() {
            Assert.AreEqual(1, Event.ListenersCount);
        }
    }

    public class When_ListenerRegisteredTwoTimes : Given {
        public override void SetUp() {
            base.SetUp();

            IListener listener = Substitute.For<IListener>();
            Event.RegisterListener(listener);
            Event.RegisterListener(listener);
        }

        [Test]
        public void Then_HasOneListener() {
            Assert.AreEqual(1, Event.ListenersCount);
        }
    }

    public class When_ListenerRegisteredAndUnregistered : Given {
        public override void SetUp() {
            base.SetUp();

            IListener listener = Substitute.For<IListener>();
            Event.RegisterListener(listener);
            Event.UnregisterListener(listener);
        }

        [Test]
        public void Then_HasNoListener() {
            Assert.AreEqual(0, Event.ListenersCount);
        }
    }

    public class When_TwoListenersRegistered : Given
    {
        public override void SetUp() {
            base.SetUp();

            Event.RegisterListener(Substitute.For<IListener>());
            Event.RegisterListener(Substitute.For<IListener>());
        }

        [Test]
        public void Then_HasTwoListeners() {
            Assert.AreEqual(2, Event.ListenersCount);
        }
    }
}

namespace Given_GameEvent0.Given_ThreeListenersRegistered
{
    public class Given : Given_GameEvent0.Given
    {
        protected IListener listener0 { get; set; }
        protected IListener listener1 { get; set; }
        protected IListener listener2 { get; set; }

        public override void SetUp() {
            base.SetUp();
            
            listener0 = Substitute.For<IListener>();
            listener1 = Substitute.For<IListener>();
            listener2 = Substitute.For<IListener>();
            
            Event.RegisterListener(listener0);
            Event.RegisterListener(listener1);
            Event.RegisterListener(listener2);
        }
    }

    public class When_Raised : Given
    {
        public override void SetUp() {
            base.SetUp();

            Event.Raise();
        }

        [Test]
        public void Then_TheThreeListenersGetCalled() {
            listener0.Received().OnEventRaised();
            listener1.Received().OnEventRaised();
            listener2.Received().OnEventRaised();
        }
    }
}