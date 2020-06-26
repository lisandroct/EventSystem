using NUnit.Framework;
using UnityEngine;
using lisandroct.EventSystem;
using NSubstitute;

public class GameEvent1 : GameEvent<int> { }

namespace Given_GameEvent1
{
    public class Given {
        protected GameEvent1 gameEvent { get; set; }

        [SetUp]
        public virtual void SetUp() {
            gameEvent = ScriptableObject.CreateInstance<GameEvent1>();
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

            gameEvent.RegisterListener(Substitute.For<IGameEventListener<int>>());
        }

        [Test]
        public void Then_HasOneListener() {
            Assert.AreEqual(1, gameEvent.ListenersCount);
        }
    }

    public class When_ListenerRegisteredTwoTimes : Given {
        public override void SetUp() {
            base.SetUp();

            IGameEventListener<int> listener = Substitute.For<IGameEventListener<int>>();
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

            IGameEventListener<int> listener = Substitute.For<IGameEventListener<int>>();
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

            gameEvent.RegisterListener(Substitute.For<IGameEventListener<int>>());
            gameEvent.RegisterListener(Substitute.For<IGameEventListener<int>>());
        }

        [Test]
        public void Then_HasTwoListeners() {
            Assert.AreEqual(2, gameEvent.ListenersCount);
        }
    }
}

namespace Given_GameEvent1.Given_ThreeListenersRegistered
{
    public class Given : Given_GameEvent1.Given
    {
        protected IGameEventListener<int> listener0 { get; set; }
        protected IGameEventListener<int> listener1 { get; set; }
        protected IGameEventListener<int> listener2 { get; set; }

        public override void SetUp() {
            base.SetUp();
            
            listener0 = Substitute.For<IGameEventListener<int>>();
            listener1 = Substitute.For<IGameEventListener<int>>();
            listener2 = Substitute.For<IGameEventListener<int>>();
            
            gameEvent.RegisterListener(listener0);
            gameEvent.RegisterListener(listener1);
            gameEvent.RegisterListener(listener2);
        }
    }

    public class When_Raised : Given
    {
        public override void SetUp() {
            base.SetUp();

            gameEvent.Raise(0);
        }

        [Test]
        public void Then_TheThreeListenersGetCalled() {
            listener0.Received().OnEventRaised(0);
            listener1.Received().OnEventRaised(0);
            listener2.Received().OnEventRaised(0);
        }
    }
}