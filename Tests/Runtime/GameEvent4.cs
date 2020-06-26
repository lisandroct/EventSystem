using NUnit.Framework;
using UnityEngine;
using lisandroct.EventSystem;
using NSubstitute;

public class GameEvent4 : GameEvent<int, int, int, int> { }

namespace Given_GameEvent4
{
    public class Given {
        protected GameEvent4 gameEvent { get; set; }

        [SetUp]
        public virtual void SetUp() {
            gameEvent = ScriptableObject.CreateInstance<GameEvent4>();
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

            gameEvent.RegisterListener(Substitute.For<IGameEventListener<int, int, int, int>>());
        }

        [Test]
        public void Then_HasOneListener() {
            Assert.AreEqual(1, gameEvent.ListenersCount);
        }
    }

    public class When_ListenerRegisteredTwoTimes : Given {
        public override void SetUp() {
            base.SetUp();

            IGameEventListener<int, int, int, int> listener = Substitute.For<IGameEventListener<int, int, int, int>>();
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

            IGameEventListener<int, int, int, int> listener = Substitute.For<IGameEventListener<int, int, int, int>>();
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

            gameEvent.RegisterListener(Substitute.For<IGameEventListener<int, int, int, int>>());
            gameEvent.RegisterListener(Substitute.For<IGameEventListener<int, int, int, int>>());
        }

        [Test]
        public void Then_HasTwoListeners() {
            Assert.AreEqual(2, gameEvent.ListenersCount);
        }
    }
}

namespace Given_GameEvent4.Given_ThreeListenersRegistered
{
    public class Given : Given_GameEvent4.Given
    {
        protected IGameEventListener<int, int, int, int> listener0 { get; set; }
        protected IGameEventListener<int, int, int, int> listener1 { get; set; }
        protected IGameEventListener<int, int, int, int> listener2 { get; set; }

        public override void SetUp() {
            gameEvent = ScriptableObject.CreateInstance<GameEvent4>();
            
            listener0 = Substitute.For<IGameEventListener<int, int, int, int>>();
            listener1 = Substitute.For<IGameEventListener<int, int, int, int>>();
            listener2 = Substitute.For<IGameEventListener<int, int, int, int>>();
            
            gameEvent.RegisterListener(listener0);
            gameEvent.RegisterListener(listener1);
            gameEvent.RegisterListener(listener2);
        }
    }

    public class When_Raised : Given
    {
        public override void SetUp() {
            base.SetUp();

            gameEvent.Raise(0, 1, 2, 3);
        }

        [Test]
        public void Then_TheThreeListenersGetCalled() {
            listener0.Received().OnEventRaised(0, 1, 2, 3);
            listener1.Received().OnEventRaised(0, 1, 2, 3);
            listener2.Received().OnEventRaised(0, 1, 2, 3);
        }
    }
}