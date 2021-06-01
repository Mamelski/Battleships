using System.Collections.Generic;
using AutoFixture.Xunit2;
using Battleships.Core.Models;
using Battleships.Core.Models.Ships;
using Moq;
using Shouldly;
using Xunit;

namespace Battleships.Core.Tests
{
    public class GameManagerShould
    {
        [Theory, AutoData]
        public void CallRandomShipPlacerWithSomeShipsWhenStartingGame(
            Mock<IShipPlacer> randomShipPlacerMock,
            Mock<IBattleshipGameEngine> battleshipGameEngineMock)
            {
                var sut = new GameManager(
                    randomShipPlacerMock.Object,
                     battleshipGameEngineMock.Object);

                sut.StartGame();
            
            randomShipPlacerMock.Verify(
                m => m.PlaceShips(It.IsAny<IEnumerable<UnplacedShip>>()), Times.Once);
        }
        
        [Theory, AutoData]
        public void SetupBattleshipGameEngineWithCorrectPlacedShips(
            Mock<IShipPlacer> randomShipPlacerMock,
            Mock<IBattleshipGameEngine> battleshipGameEngineMock,
            List<PlacedShip> placedShips)
        {
            randomShipPlacerMock.Setup(
                    r => r.PlaceShips(It.IsAny<IEnumerable<UnplacedShip>>()))
                .Returns(placedShips);
            
            var sut = new GameManager(
                randomShipPlacerMock.Object,
                battleshipGameEngineMock.Object);

            sut.StartGame();
            
            battleshipGameEngineMock.Verify(
                m => m.SetupBoard(placedShips), Times.Once);
        }
        
        [Theory, AutoData]
        public void CallBattleshipGameEngineWithCorrectShotCoordinate(
            Mock<IShipPlacer> randomShipPlacerMock,
            Mock<IBattleshipGameEngine> battleshipGameEngineMock,
            Coordinate coordinate,
            GameState gameState)
        {
            battleshipGameEngineMock.Setup(
                    b => b.Shoot(It.IsAny<Coordinate>()))
                .Returns(gameState);
            
            var sut = new GameManager(
                randomShipPlacerMock.Object,
                battleshipGameEngineMock.Object);

            sut.Shoot(coordinate);
            
            battleshipGameEngineMock.Verify(
                m => m.Shoot(coordinate), Times.Once);
        }
        
        [Theory, AutoData]
        public void ReturnCorrectStateFromGameEngine(
            Mock<IShipPlacer> randomShipPlacerMock,
            Mock<IBattleshipGameEngine> battleshipGameEngineMock,
            Coordinate coordinate,
            GameState gameState)
        {
            battleshipGameEngineMock.Setup(
                    b => b.Shoot(It.IsAny<Coordinate>()))
                .Returns(gameState);
            
            var sut = new GameManager(
                randomShipPlacerMock.Object,
                battleshipGameEngineMock.Object);

            var result = sut.Shoot(coordinate);
            
            result.ShouldBe(gameState);
        }
    }
}