using NUnit.Framework;
using Scorer;

namespace ScorerTests
{
    [TestFixture]
    public class ScorerTests
    {
        [TestCase(2, Category.Ones, 1, 5, 2, 4, 1)]
        [TestCase(6, Category.Twos, 2, 5, 2, 3, 2)]
        [TestCase(0, Category.Twos, 1, 5, 1, 3, 1)]
        public void ScorerStoresFirstScore(int expected, Category scoreCategory, params int[] diceValues)
        {
            RollModel input = new RollModel();
            input.Category = scoreCategory;
            input.Values = diceValues;
            GameScorer scorer = new GameScorer();
            scorer.Roll(input);

            Assert.AreEqual(expected, scorer.Score);
        }
    }
}