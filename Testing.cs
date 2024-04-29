using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace CMP1903_A2_2324 {

  // TODO: Add XML comments to class.

  public class Testing {

    public const int TEST_COUNT = 10_000;

    public static void RunTests() {
      TestDie(); // Test the Die object.
      TestFrequencies(); // Test colle.cting frequencies into a dictionary
      TestOccurances(); // Test converting frequencies dictionary to just frequencies.
      TestTwoOfAKind(); // Test finding two of a kind from an integer array.
      TestThreeOrMoreScore(); // Test score calculation.
      for (int i = 0; i < TEST_COUNT; i++) {
        TestSevensOut(); // Test full SevensOut game.
        TestThreeOrMore(); // Test full ThreeOrMore game.
      }
    }

    public static void TestTwoOfAKind() {
      int first = ThreeOrMore.TwoOfAKind(new int[] { 1, 1, 2, 3, 4, 4 });
      int second = ThreeOrMore.TwoOfAKind(new int[] { 1, 2, 3, 4, 4 });
      int third = ThreeOrMore.TwoOfAKind(new int[] { 1, 3, 3, 1, 3, 4 });
      int fourth = ThreeOrMore.TwoOfAKind(new int[] { 1, 2, 3, 4 });

      Debug.Assert(first == 1, "Failed to find double.");
      Debug.Assert(second == 4, "Failed to find double.");
      Debug.Assert(third == 1, "Failed to find double.");
      Debug.Assert(fourth == -1, "Failed to find double.");
    }

    public static void TestFrequencies() {
      Dictionary<int, int> freqs = ThreeOrMore.GetFrequencies(
        new int[] { 1, 4, 1, 2, 2, 1, 3, 5, 6 }
      );

      Debug.Assert(freqs[1] == 3, "Invalid frequency check.");
      Debug.Assert(freqs[2] == 2, "Invalid frequency check.");
      Debug.Assert(freqs[3] == 1, "Invalid frequency check.");
      Debug.Assert(freqs[4] == 1, "Invalid frequency check.");
      Debug.Assert(freqs[5] == 1, "Invalid frequency check.");
      Debug.Assert(freqs[6] == 1, "Invalid frequency check.");
    }

    public static void TestOccurances() {
      int[] occurances = ThreeOrMore.Occurances(new Dictionary<int, int>() {
        [1] = 3, [2] = 8, [3] = 5, [4] = 6
      });
      Debug.Assert(occurances[0] == 3, "First occurance checked.");
      Debug.Assert(occurances[1] == 8, "Second occurance checked.");
      Debug.Assert(occurances[2] == 5, "Third occurance checked.");
      Debug.Assert(occurances[3] == 6, "Fourth occurance checked.");
    }

    public static void TestDie() {
      Die die = new Die();
      Die lockedDie = new Die();

      int previousNumber = lockedDie.Roll();
      lockedDie.Locked = true;

      HashSet<int> numsOccured = new HashSet<int>();
      for (int i = 0; i < TEST_COUNT; i++) {
        int roll = die.Roll();
        numsOccured.Add(roll);

        // Die value needs to be within set range.
        Debug.Assert(roll > 0 && roll <= die.NumberOfSides, "Die not in corret range.");

        // Needs to always return same value.
        Debug.Assert(lockedDie.Roll() == previousNumber, "Die not locking.");
      }

      // Sets cannot have duplicates, so number of sides on the dice must equal number of items
      // inside of the set.
      Debug.Assert(numsOccured.Count == die.NumberOfSides, "Too many dice values.");
    }

    public static void TestThreeOrMoreScore() {
      int scoreTwelve = ThreeOrMore.CalculateScore(new int[] { 4, 2, 5, 3 });
      int scoreSix = ThreeOrMore.CalculateScore(new int[] { 4, 2, 3, 1 });
      int scoreThree = ThreeOrMore.CalculateScore(new int[] { 2, 3, 1 });
      int scoreZero = ThreeOrMore.CalculateScore(new int[] { 1, 1, 1 });

      Debug.Assert(scoreTwelve == 12, "Score should be 12.");
      Debug.Assert(scoreSix == 6, "Score should be 6.");
      Debug.Assert(scoreThree == 3, "Score should be 3.");
      Debug.Assert(scoreZero == 0, "Score should be 0.");
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
