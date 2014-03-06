﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TurnItUp.Locations;
using TurnItUp.Interfaces;
using Moq;
using TurnItUp.Components;
using System.Collections.Generic;
using Entropy;
using TurnItUp.Pathfinding;
using TurnItUp.Tmx;

namespace Tests.Locations
{
    [TestClass]
    public class LevelTests
    {
        // The sample level:
        // XXXXXXXXXXXXXXXX
        // X....EEE.......X
        // X..........X...X
        // X.......E......X
        // X.E.X..........X
        // X.....E....E...X
        // X........X.....X
        // X..........XXXXX
        // X..........X...X
        // X..........X...X
        // X......X.......X
        // X.X........X...X
        // X..........X...X
        // X..........X...X
        // X......P...X...X
        // XXXXXXXXXXXXXXXX
        // X - Obstacles, P - Player, E - Enemies

        private Level _level;
        private Mock<ICharacterManager> _characterManagerMock;
        private World _world;

        [TestInitialize]
        public void Initialize()
        {
            _world = new World();
            _level = new Level(_world, "../../Fixtures/FullExample.tmx");
            _characterManagerMock = new Mock<ICharacterManager>();
        }

        [TestMethod]
        public void Level_Construction_IsSuccessful()
        {
            Level level = new Level(_world, "../../Fixtures/FullExample.tmx");

            Assert.IsNotNull(level.Map);

            // The TurnManager should have been automatically set up to track the turns of any sprites in the layer which has IsCharacters property set to true
            Assert.AreEqual(_world, _level.World);
            Assert.IsNotNull(level.CharacterManager);
            Assert.AreEqual(9, level.CharacterManager.Characters.Count);
            Assert.AreEqual(level, level.CharacterManager.Level);
            Assert.AreEqual(9, level.CharacterManager.TurnQueue.Count);
            Assert.IsNotNull(level.PathFinder);
            Assert.IsFalse(level.PathFinder.AllowDiagonalMovement);
            Assert.AreEqual(level, level.PathFinder.Level);
        }

        [TestMethod]
        public void Level_Initialization_IsSuccessful()
        {
            Level level = new Level();
            level.Initialize(_world, "../../Fixtures/FullExample.tmx");

            Assert.IsNotNull(level.Map);

            // The TurnManager should have been automatically set up to track the turns of any sprites in the layer which has IsCharacters property set to true
            Assert.AreEqual(_world, _level.World);
            Assert.IsNotNull(level.CharacterManager);
            Assert.AreEqual(9, level.CharacterManager.Characters.Count);
            Assert.AreEqual(level, level.CharacterManager.Level);
            Assert.AreEqual(9, level.CharacterManager.TurnQueue.Count);
            Assert.IsNotNull(level.PathFinder);
            Assert.IsFalse(level.PathFinder.AllowDiagonalMovement);
            Assert.AreEqual(level, level.PathFinder.Level);
        }

        [TestMethod]
        public void Level_ConstructionWithAPathFinderThatAllowsDiagonalMovement_IsSuccessful()
        {
            Level level = new Level(_world, "../../Fixtures/FullExample.tmx", true);

            Assert.IsNotNull(level.Map);

            // The TurnManager should have been automatically set up to track the turns of any sprites in the layer which has IsCharacters property set to true
            Assert.AreEqual(_world, _level.World);
            Assert.IsNotNull(level.CharacterManager);
            Assert.AreEqual(9, level.CharacterManager.Characters.Count);
            Assert.AreEqual(level, level.CharacterManager.Level);
            Assert.AreEqual(9, level.CharacterManager.TurnQueue.Count);
            Assert.IsNotNull(level.PathFinder);
            Assert.IsTrue(level.PathFinder.AllowDiagonalMovement);
            Assert.AreEqual(level, level.PathFinder.Level);
        }

        [TestMethod]
        public void Level_DeterminingObstacles_TakesIntoAccountLayerHavingTrueForIsCollisionProperty()
        {
            // The example level has a "wall" around the entire 15x15 level
            Assert.IsTrue(_level.IsObstacle(0, 0));
            Assert.IsTrue(_level.IsObstacle(0, 1));
            Assert.IsTrue(_level.IsObstacle(1, 0));
            Assert.IsTrue(_level.IsObstacle(2, 0));

            Assert.IsFalse(_level.IsObstacle(1, 1));
        }

        // Facade implementation tests
        [TestMethod]
        public void Level_DeterminingIfCharacterIsAtAPosition_DelegatesToCharacterManager()
        {
            _level.CharacterManager = _characterManagerMock.Object;

            _level.IsCharacterAt(0, 0);
            _characterManagerMock.Verify(cm => cm.IsCharacterAt(0, 0));
        }

        [TestMethod]
        public void Level_MovingAPlayer_DelegatesToCharacterManager()
        {
            _level.CharacterManager = _characterManagerMock.Object;

            _level.MovePlayer(Direction.Down);
            _characterManagerMock.Verify(cm => cm.MovePlayer(Direction.Down));
        }

        [TestMethod]
        public void Level_MovingACharacterToAPosition_DelegatesToCharacterManager()
        {
            _level.CharacterManager = _characterManagerMock.Object;

            _level.MoveCharacterTo(null, new Position(0, 0));
            _characterManagerMock.Verify(cm => cm.MoveCharacterTo(null, new Position(0, 0)));
        }
    }
}