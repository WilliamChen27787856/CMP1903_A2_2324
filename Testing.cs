using System;
using System.Linq;
using System.Diagnostics;

namespace CMP1903_A2_2324 {

  public class Testing {

    public const int TEST_COUNT = 1_000;

    public static void RunGameTests() {
      for (int i = 0; i < TEST_COUNT; i++) {
        TestSevensOut();
        TestThreeOrMore();
      }
    }

    public static void TestSevensOut() {
      Game.DEBUG = true;
      Game game = new SevensOut(true);
      game.Play(); // Auto-play Game.

      int playerOne = game.PlayerOneScore;
      int playerTwo = game.PlayerTwoScore;
      int[] diceValues = game.DieValues;
      int sum = game.DieSum;

      // Check if successfully stops before 7.
      Debug.Assert(diceValues.Sum() == 7, "Game ended with non-zero sum.");

      // Ensure total die values are different to the sum property, this should never occur.
      Debug.Assert(diceValues.Sum() == sum, "Game sum invalidated.");
      Game.DEBUG = false;
    }

    public static void TestThreeOrMore() {
      Game.DEBUG = true;
      Game.DEBUG_INPUT = "1"; // Responds to re-roll questions with to re-roll remaining die.
      Game game = new ThreeOrMore(true);
      int[] scores = { 0, 0 };
      while (game.NextMove()) {
        // Assign scores inversed, since after game played it switches player before return.
        scores[(game.PlayerOneMove ? 1 : 0)] += ThreeOrMore.GetScoreFromValues(game.DieValues);
      }

      // Early return so does not switch player, can use normal indices.
      scores[(game.PlayerOneMove ? 0 : 1)] += ThreeOrMore.GetScoreFromValues(game.DieValues);

      int playerOne = game.PlayerOneScore;
      int playerTwo = game.PlayerTwoScore;
      int highestScore = Math.Max(playerOne, playerTwo);

      // Total >= 20 recognised.
      Debug.Assert(highestScore >= 20, "Highest score below 20, invalid game occured.");

      // Scores added correctly.
      Debug.Assert(scores[0] == playerOne, "Player one score not matching final score.");
      Debug.Assert(scores[1] == playerTwo, "Player one score not matching final score.");


      Game.DEBUG_INPUT = "";
      Game.DEBUG = false;
    }


  }

}
