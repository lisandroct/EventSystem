using NUnit.Framework;
using UnityEngine;
using lisandroct.EventSystem;
using NSubstitute;

public class Event3 : Event<int, int, int> { }

namespace Given_GameEvent3
{
    public class Given {
        protected Event3 Event { get; set; }

        [SetUp]
        public virtual void SetUp() {
            Event = ScriptableObject.CreateInstance<Event3>();
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

            Event.RegisterListener(Substitute.For<IListener<int, int, int>>());
        }

        [Test]
        public void Then_HasOneListener() {
            Assert.AreEqual(1, Event.ListenersCount);
        }
    }

    public class When_ListenerRegisteredTwoTimes : Given {
        public override void SetUp() {
            base.SetUp();

            IListener<int, int, int> listener = Substitute.For<IListener<int, int, int>>();
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

            IListener<int, int, int> listener = Substitute.For<IListener<int, int, int>>();
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

            Event.RegisterListener(Substitute.For<IListener<int, int, int>>());
            Event.RegisterListener(Substitute.For<IListener<int, int, int>>());
        }

        [Test]
        public void Then_HasTwoListeners() {
            Assert.AreEqual(2, Event.ListenersCount);
        }
    }
}

namespace Given_GameEvent3.Given_ThreeListenersRegistered {
    public class Given : Given_GameEvent3.Given
    {
        protected IListener<int, int, int> listener0 { get; set; }
        protected IListener<int, int, int> listener1 { get; set; }
        protected IListener<int, int, int> listener2 { get; set; }

        public override void SetUp() {
            base.SetUp();

            listener0 = Substitute.For<IListener<int, int, int>>();
            listener1 = Substitute.For<IListener<int, int, int>>();
            listener2 = Substitute.For<IListener<int, int, int>>();
            
            Event.RegisterListener(listener0);
            Event.RegisterListener(listener1);
            Event.RegisterListener(listener2);
        }
    }

    public class When_Raised : Given
    {
        public override void SetUp() {
            base.SetUp();

            Event.Raise(0, 1, 2);
        }

        [Test]
        public void Then_TheThreeListenersGetCalled() {
            listener0.Received().OnEventRaised(0, 1, 2);
            listener1.Received().OnEventRaised(0, 1, 2);
            listener2.Received().OnEventRaised(0, 1, 2);
        }
    }
}