using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace CMP1903_A2_2324 {

  /// <summary>
  /// This is the testing class, it is used to run tests to ensure no parts of the program are
  /// broken.
  /// </summary>
  public class Testing {

    /// <value>
    /// A constant integer that is used to specify a standard number of tests to ensure the
    /// the different bits don't have any other issues.
    /// </value>
    public const int TEST_COUNT = 10_000;

    /// <summary>
    /// A static method used to run all the tests from one method.
    /// </summary>
    public static void RunTests() {
      TestDie(); // Test the Die object.
      TestFrequencies(); // Test colle.cting frequencies into a dictionary
      TestOccurances(); // Test converting frequencies dictionary to just frequencies.
      TestTwoOfAKind(); // Test finding two of a kind from an integer array.
      TestThreeOrMoreScore(); // Test score calculation.
      TestSevensOut(); // Test full SevensOut game.
      TestThreeOrMore(); // Test full ThreeOrMore game.
    }

    /// <summary>
    /// A static method to test the <c>ThreeOrMore.TwoOfAKind</c> method.
    /// </summary>
    public static void TestTwoOfAKind() {
      int first = ThreeOrMore.TwoOfAKind(new int[] { 1, 1, 2, 3, 4, 4 });
      int second = ThreeOrMore.TwoOfAKind(new int[] { 1, 2, 3, 4, 4 });
      int third = ThreeOrMore.TwoOfAKind(new int[] { 1, 3, 3, 1, 3, 4 });
      int fourth = ThreeOrMore.TwoOfAKind(new int[] { 1, 2, 3, 4 });

      using (StreamWriter file = Testing.CreateFile("twoofakind")) {
        Testing.DebugWriter(first == 1, $"Find array number pair. {first} == 1", file);
        Testing.DebugWriter(second == 4, $"Find array number pair. {second} == 4", file);
        Testing.DebugWriter(third == 1, $"Find array number pair. {third} == 1", file);
        Testing.DebugWriter(fourth == -1, $"Find array number pair. {fourth} == -1", file);
      }
    }

    /// <summary>
    /// A static method to test the <c>ThreeOrMore.GetFrequencies</c> method.
    /// </summary>
    public static void TestFrequencies() {
      Dictionary<int, int> freqs = ThreeOrMore.GetFrequencies(
        new int[] { 1, 4, 1, 2, 2, 1, 3, 5, 6 }
      );

      using (StreamWriter file = Testing.CreateFile("frequencies")) {
        Testing.DebugWriter(freqs[1] == 3, $"Invalid frequency check. {freqs[1]} == 3", file);
        Testing.DebugWriter(freqs[2] == 2, $"Invalid frequency check. {freqs[2]} == 2", file);
        Testing.DebugWriter(freqs[3] == 1, $"Invalid frequency check. {freqs[3]} == 1", file);
        Testing.DebugWriter(freqs[4] == 1, $"Invalid frequency check. {freqs[4]} == 1", file);
        Testing.DebugWriter(freqs[5] == 1, $"Invalid frequency check. {freqs[5]} == 1", file);
        Testing.DebugWriter(freqs[6] == 1, $"Invalid frequency check. {freqs[6]} == 1", file);
      }
    }

    /// <summary>
    /// A static method to test the <c>ThreeOrMore.Occurances</c> method.
    /// </summary>
    public static void TestOccurances() {
      int[] occurances = ThreeOrMore.Occurances(new Dictionary<int, int>() {
        [1] = 3, [2] = 8, [3] = 5, [4] = 6
      });
      using (StreamWriter file = Testing.CreateFile("occurances")) {
        Testing.DebugWriter(
          occurances[0] == 3,
          $"First occurance checked. {occurances[0]} == 3",
          file
        );
        Testing.DebugWriter(
          occurances[1] == 8,
          $"Second occurance checked. {occurances[1]} == 8",
          file
        );
        Testing.DebugWriter(
          occurances[2] == 5,
          $"Third occurance checked. {occurances[2]} == 5",
          file
        );
        Testing.DebugWriter(
          occurances[3] == 6,
          $"Fourth occurance checked. {occurances[3]} == 6",
          file
        );
      }
    }

    /// <summary>
    /// A static method to test the <c>Die</c> class.
    /// </summary>
    public static void TestDie() {
      Die die = new Die();
      Die lockedDie = new Die();

      int previousNumber = lockedDie.Roll();
      lockedDie.Locked = true;

      HashSet<int> numsOccured = new HashSet<int>();

      using (StreamWriter file = Testing.CreateFile("die")) {
        for (int i = 0; i < TEST_COUNT; i++) {
          int roll = die.Roll();
          numsOccured.Add(roll);

          // Die value needs to be within set range.
          Testing.DebugWriter(
            roll > 0 && roll <= die.NumberOfSides,
            $"Die roll value range check. 0 < {roll} < {die.NumberOfSides}",
            file
          );

          // Needs to always return same value.
          int lockRoll = lockedDie.Roll();
          Testing.DebugWriter(
            lockRoll == previousNumber,
            $"Locked die check. {lockRoll} == {previousNumber}",
            file
          );
        }

        // Sets cannot have duplicates, so number of sides on the dice must equal number of items
        // inside of the set.
        Testing.DebugWriter(
          numsOccured.Count == die.NumberOfSides,
          $"Dice roll value occurance check. {numsOccured.Count} == {die.NumberOfSides}",
          file
        );
      }
    }

    /// <summary>
    /// A static method to test the <c>ThreeOrMore.CalculateScore</c> method.
    /// </summary>
    public static void TestThreeOrMoreScore() {
      int scoreTwelve = ThreeOrMore.CalculateScore(new int[] { 4, 2, 5, 3 });
      int scoreSix = ThreeOrMore.CalculateScore(new int[] { 4, 2, 3, 1 });
      int scoreThree = ThreeOrMore.CalculateScore(new int[] { 2, 3, 1 });
      int scoreZero = ThreeOrMore.CalculateScore(new int[] { 1, 1, 1 });

      using (StreamWriter file = Testing.CreateFile("threeormorescores")) {
        Testing.DebugWriter(
          scoreTwelve == 12,
          $"Score calculation correct. {scoreTwelve} == 12",
          file
        );
        Testing.DebugWriter(
          scoreSix == 6,
          $"Score calculation correct. {scoreSix} == 6",
          file
        );
        Testing.DebugWriter(
          scoreThree == 3,
          $"Score calculation correct. {scoreThree} == 3",
          file
        );
        Testing.DebugWriter(
          scoreZero == 0,
          $"Score calculation correct. {scoreZero} == 0",
          file
        );
      }
    }

    /// <summary>
    /// A static method to test the <c>SevensOut</c> game.
    /// </summary>
    public static void TestSevensOut() {
      Game.DEBUG = true;

      using (StreamWriter file = Testing.CreateFile("sevensout")) {
        for (int i = 0; i < TEST_COUNT; i++) {
          Game game = new SevensOut(true);
          game.Play(); // Auto-play Game.

          int playerOne = game.PlayerOneScore;
          int playerTwo = game.PlayerTwoScore;
          int[] diceValues = game.DieValues;
          int sum = game.DieSum;

          // Check if successfully stops before 7.
          int diceSum = diceValues.Sum();
          Testing.DebugWriter(
            diceSum == 7,
            $"Correct game ending condition. {diceSum} == 7",
            file
          );

          // Ensure total die values are different to the sum property, this should never occur.
          Testing.DebugWriter(diceSum == sum, $"Game sum check. {diceSum} == {sum}", file);
        }
      }

      Game.DEBUG = false;
    }

    /// <summary>
    /// A static method to test the <c>ThreeOrMore</c> game.
    /// </summary>
    public static void TestThreeOrMore() {
      Game.DEBUG = true;
      Game.DEBUG_INPUT = "1"; // Responds to re-roll questions with to re-roll remaining die.

      using (StreamWriter file = Testing.CreateFile("threeormore")) {
        for (int i = 0; i < TEST_COUNT; i++) {
          Game game = new ThreeOrMore(true);
          int[] scores = { 0, 0 };
          while (game.NextTurn()) {
            // Assign scores inversed, since after game played it switches player before return.
            scores[(game.PlayerOneMove ? 1 : 0)] += ThreeOrMore.GetScoreFromValues(game.DieValues);
          }

          // Early return so does not switch player, can use normal indices.
          scores[(game.PlayerOneMove ? 0 : 1)] += ThreeOrMore.GetScoreFromValues(game.DieValues);

          int playerOne = game.PlayerOneScore;
          int playerTwo = game.PlayerTwoScore;
          int highestScore = Math.Max(playerOne, playerTwo);

          // Total >= 20 recognised.
          Testing.DebugWriter(
            highestScore >= 20,
            $"Highest end score check. {highestScore} >= 20",
            file
          );

          // Scores added correctly.
          Testing.DebugWriter(
            scores[0] == playerOne,
            $"Player one matching final score. {scores[0]} == {playerOne}",
            file
          );
          Testing.DebugWriter(
            scores[1] == playerTwo,
            $"Player two matching final score. {scores[1]} == {playerTwo}",
            file
          );
        }
      }
      Game.DEBUG_INPUT = "";
      Game.DEBUG = false;
    }

    public static void DebugWriter(bool check, string msg, StreamWriter file) {
      Debug.Assert(check, msg);
      string prefix = check ? "PASS" : "ERR";
      file.Write($"{Testing.GetTimestamp()} [{prefix}] {msg}\n");
    }

    public static StreamWriter CreateFile(string name) {
      return File.AppendText($"./{name}_{Testing.GetTimestamp()}.log");
    }

    public static long GetTimestamp() {
      return DateTimeOffset.Now.ToUnixTimeSeconds();
    }

  }

}
