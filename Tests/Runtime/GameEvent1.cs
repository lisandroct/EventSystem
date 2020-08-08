using NUnit.Framework;
using UnityEngine;
using lisandroct.EventSystem;
using NSubstitute;

public class Event1 : Event<int> { }

namespace Given_GameEvent1
{
    public class Given {
        protected Event1 Event { get; set; }

        [SetUp]
        public virtual void SetUp() {
            Event = ScriptableObject.CreateInstance<Event1>();
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

            Event.RegisterListener(Substitute.For<IListener<int>>());
        }

        [Test]
        public void Then_HasOneListener() {
            Assert.AreEqual(1, Event.ListenersCount);
        }
    }

    public class When_ListenerRegisteredTwoTimes : Given {
        public override void SetUp() {
            base.SetUp();

            IListener<int> listener = Substitute.For<IListener<int>>();
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

            IListener<int> listener = Substitute.For<IListener<int>>();
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

            Event.RegisterListener(Substitute.For<IListener<int>>());
            Event.RegisterListener(Substitute.For<IListener<int>>());
        }

        [Test]
        public void Then_HasTwoListeners() {
            Assert.AreEqual(2, Event.ListenersCount);
        }
    }
}

namespace Given_GameEvent1.Given_ThreeListenersRegistered
{
    public class Given : Given_GameEvent1.Given
    {
        protected IListener<int> listener0 { get; set; }
        protected IListener<int> listener1 { get; set; }
        protected IListener<int> listener2 { get; set; }

        public override void SetUp() {
            base.SetUp();
            
            listener0 = Substitute.For<IListener<int>>();
            listener1 = Substitute.For<IListener<int>>();
            listener2 = Substitute.For<IListener<int>>();
            
            Event.RegisterListener(listener0);
            Event.RegisterListener(listener1);
            Event.RegisterListener(listener2);
        }
    }

    public class When_Raised : Given
    {
        public override void SetUp() {
            base.SetUp();

            Event.Raise(0);
        }

        [Test]
        public void Then_TheThreeListenersGetCalled() {
            listener0.Received().OnEventRaised(0);
            listener1.Received().OnEventRaised(0);
            listener2.Received().OnEventRaised(0);
        }
    }
}