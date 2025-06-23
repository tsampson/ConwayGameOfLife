using ConwayGameOfLife.Models;
using FluentAssertions;

namespace ConwayGameOfLife.Processing.Tests
{
    public class GameOfLifeProcessorTests
    {
        private GameOfLifeProcessor _processor;

        [SetUp]
        public void Setup()
        {
            _processor = new GameOfLifeProcessor();
        }

        [Test]
        [TestCaseSource(nameof(corner_tests))]
        [TestCaseSource(nameof(normal_tests))]
        public int CountLivingNeighbors(byte[,] grid, int x, int y)
        {
            return _processor.CountLivingNeighbors(grid, x, y);
        }

        [Test]
        public void ProcessNextState_HappyPath_EachRuleExample()
        {
            var id = Guid.NewGuid();
            var initialGrid = new byte[,] 
            {
                { 1, 0, 0, 0, 0 },
                { 0, 1, 1, 1, 0 },
                { 0, 1, 1, 1, 0 },
                { 0, 1, 1, 1, 0 },
                { 0, 0, 0, 0, 0 },
            };

            var expectedGrid = new byte[,]
            {
                { 0, 1, 1, 0, 0 },
                { 1, 0, 0, 1, 0 },
                { 1, 0, 0, 0, 1 },
                { 0, 1, 0, 1, 0 },
                { 0, 0, 1, 0, 0 },
            };

            var boardState = new GameState 
            {
                BoardId = id,
                Board = initialGrid,
                Generation = 0,
                Population = 0               
            };
            var newBoardState = _processor.ProcessNextState(boardState);

            newBoardState.BoardId.Should().Be(id);
            newBoardState.Generation.Should().Be(1);
            newBoardState.Board.Should().BeEquivalentTo(expectedGrid);
            newBoardState.Population.Should().Be(9);
        }

        private static byte[,] CornerEmptyGrid = new byte[,]
        {
            { 0, 0 },
            { 0, 0 }
        };

        private static byte[,] CornerFullGrid = new byte[,]
        {
            { 1, 1 },
            { 1, 1 }
        };

        private static byte[,] NormalEmptyGrid = new byte[,]
        {
            { 0, 0, 0 },
            { 0, 0, 0 },
            { 0, 0, 0 }
        };

        private static byte[,] NormalFullGrid = new byte[,]
        {
            { 1, 1, 1 },
            { 1, 1, 1 },
            { 1, 1, 1 }
        };

        public static IEnumerable<TestCaseData> corner_tests()
        {
            yield return new TestCaseData(CornerEmptyGrid, 0, 0).SetName("Empty Top Left").Returns(0);
            yield return new TestCaseData(CornerEmptyGrid, 0, 1).SetName("Empty Top Right").Returns(0);
            yield return new TestCaseData(CornerEmptyGrid, 1, 0).SetName("Empty Bottom Left").Returns(0);
            yield return new TestCaseData(CornerEmptyGrid, 1, 1).SetName("Empty Bottom Right").Returns(0);
            yield return new TestCaseData(CornerFullGrid, 0, 0).SetName("Full Top Left").Returns(3);
            yield return new TestCaseData(CornerFullGrid, 0, 1).SetName("Full Top Right").Returns(3);
            yield return new TestCaseData(CornerFullGrid, 1, 0).SetName("Full Bottom Left").Returns(3);
            yield return new TestCaseData(CornerFullGrid, 1, 1).SetName("Full Bottom Right").Returns(3);
        }

        public static IEnumerable<TestCaseData> normal_tests()
        {
            yield return new TestCaseData(NormalEmptyGrid, 1, 1).Returns(0);
            yield return new TestCaseData(NormalFullGrid, 1, 1).Returns(8);
        }
    }
}