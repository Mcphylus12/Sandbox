using NUnit.Framework;
using Scorer;
using System;

namespace ScorerTests
{
    [TestFixture]
    public class YahtzeeStringProcessorTests
    {
        [TestCase("(1, 2, 3, 5, 6) ones", Category.Ones)]
        public void ProcessorSuccessfullyResolvesScoreCategory(string input, Category expected)
        {
            YahtzeeStringProcessor stringProcessor = new YahtzeeStringProcessor(input);
            var model = stringProcessor.BuildModel();

            Assert.AreEqual(expected, model.Category);
        }

        [TestCase("invalid")]
        [TestCase("(1, 2, 3, 5, 6) badscorecategory")]
        public void ProcessorThrowsScoreCategoryException(string input)
        {
            YahtzeeStringProcessor stringProcessor = new YahtzeeStringProcessor(input);

            Assert.Throws<Exception>(() => stringProcessor.BuildModel());
        }

        [TestCase("(1, 2, 3, 5, 6) ones", 1, 2, 3, 5, 6)]
        [TestCase("(1, 2, 3, 5, 6, 3) ones", 1, 2, 3, 5, 6, 3)]
        public void DiceRollsAreProcessed(string input, params int[] expectedRolls)
        {
            YahtzeeStringProcessor stringProcessor = new YahtzeeStringProcessor(input);
            var model = stringProcessor.BuildModel();

            Assert.AreEqual(expectedRolls, model.Values);
        }
    }
}
