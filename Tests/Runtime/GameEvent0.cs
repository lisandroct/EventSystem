using NUnit.Framework;
using UnityEngine;
using lisandroct.EventSystem;
using NSubstitute;

namespace Given_GameEvent0
{
    public class Given {
        protected GameEvent gameEvent { get; set; }

        [SetUp]
        public virtual void SetUp() {
            gameEvent = ScriptableObject.CreateInstance<GameEvent>();
        }
    }

    public class When_Created : Given {
        [Test]
        public void Then_HasNoListeners() {
            Assert.AreEqual(0, gameEvent.ListenersCount);
        }
    }

    public class When_ListenerRegistered : Given {
        public override void SetUp() {
            base.SetUp();

            gameEvent.RegisterListener(Substitute.For<IGameEventListener>());
        }

        [Test]
        public void Then_HasOneListener() {
            Assert.AreEqual(1, gameEvent.ListenersCount);
        }
    }

    public class When_ListenerRegisteredTwoTimes : Given {
        public override void SetUp() {
            base.SetUp();

            IGameEventListener listener = Substitute.For<IGameEventListener>();
            gameEvent.RegisterListener(listener);
            gameEvent.RegisterListener(listener);
        }

        [Test]
        public void Then_HasOneListener() {
            Assert.AreEqual(1, gameEvent.ListenersCount);
        }
    }

    public class When_ListenerRegisteredAndUnregistered : Given {
        public override void SetUp() {
            base.SetUp();

            IGameEventListener listener = Substitute.For<IGameEventListener>();
            gameEvent.RegisterListener(listener);
            gameEvent.UnregisterListener(listener);
        }

        [Test]
        public void Then_HasNoListener() {
            Assert.AreEqual(0, gameEvent.ListenersCount);
        }
    }

    public class When_TwoListenersRegistered : Given
    {
        public override void SetUp() {
            base.SetUp();

            gameEvent.RegisterListener(Substitute.For<IGameEventListener>());
            gameEvent.RegisterListener(Substitute.For<IGameEventListener>());
        }

        [Test]
        public void Then_HasTwoListeners() {
            Assert.AreEqual(2, gameEvent.ListenersCount);
        }
    }
}

namespace Given_GameEvent0.Given_ThreeListenersRegistered
{
    public class Given : Given_GameEvent0.Given
    {
        protected IGameEventListener listener0 { get; set; }
        protected IGameEventListener listener1 { get; set; }
        protected IGameEventListener listener2 { get; set; }

        public override void SetUp() {
            base.SetUp();
            
            listener0 = Substitute.For<IGameEventListener>();
            listener1 = Substitute.For<IGameEventListener>();
            listener2 = Substitute.For<IGameEventListener>();
            
            gameEvent.RegisterListener(listener0);
            gameEvent.RegisterListener(listener1);
            gameEvent.RegisterListener(listener2);
        }
    }

    public class When_Raised : Given
    {
        public override void SetUp() {
            base.SetUp();

            gameEvent.Raise();
        }

        [Test]
        public void Then_TheThreeListenersGetCalled() {
            listener0.Received().OnEventRaised();
            listener1.Received().OnEventRaised();
            listener2.Received().OnEventRaised();
        }
    }
}