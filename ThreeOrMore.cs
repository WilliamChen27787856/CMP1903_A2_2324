using System;
using System.Collections.Generic;
using System.Linq;

namespace CMP1903_A2_2324 {

  public sealed class ThreeOrMore : Game {

    /// <value>
    /// A constant value for the game's identifier.
    /// </value>
    private const string GAME_NAME = "Three Or More";

    /// <value>
    /// A constant value for the number of dice that are inside of the game.
    /// </value>
    private const int DICE_COUNT = 5;

    /// <summary>
    /// The constructor for the class, it is used to instantiate the default values for the game
    /// </summary>
    /// <param name="againstComputer">
    /// If the game should be against a computer or not.
    /// </param>
    public ThreeOrMore(bool againstComputer) : base(GAME_NAME, DICE_COUNT, 6, againstComputer) {
    }

    /// <summary>
    /// This method is an override of an abstract method from the <c>Game</c> class.
    /// This method implements the game requirements for the Three Or More game rules.
    /// </summary>
    /// <returns>
    /// A boolean for if the game should continue or not, true = continue, false = exit.
    /// </returns>
    public override bool NextMove() {
      int[] diceRolled = this.RollDice();
      this.PrintRolledDice(); // Print out what the player rolled.

      this.CheckTwoOfAKind(diceRolled); // Check if the player rolled a two of a kind.

      int total = ThreeOrMore.GetScoreFromValues(this.DieValues); // Find the player's score.
      this.AddScorePlayer(total); // Add the score to the player.

      if (this.GetScorePlayer() >= 20) { // If their total score is over 20, game over.
        return false;
      }

      this.SwitchPlayer(); // Switch to the next player.
      return true; // Continue playing.
    }

    /// <summary>
    /// This method is used to simplify the main code in the <c>NextMove</c> method, it contains
    /// the logic for if the player should be allowed to re-roll their dice.
    /// </summary>
    private void CheckTwoOfAKind(int[] diceRolled) {
      int twoOfAKind = TwoOfAKind(diceRolled); // Check if the player scored a two of a kind.

      if (twoOfAKind == -1) { // Player did not have any two of a kinds.
        return; // Return back to main game logic.
      }

      bool computerPlaying = this.IsPlayerComputer(); // Check if current player is the computer.
      bool doLockDie = computerPlaying; // Always perform double locking if computer playing.

      Game.ScreenPrint($"{this.GetPlayerName()} has got a double.");
      if (!computerPlaying) { // Ask non-computer players if they would like to re-roll.
        Game.ScreenPrint("Would you like to re-roll the others?");
        string reRoll = Game.Choice(new string[] { "All", "Remaining", "No" }); // Get user choice.

        if (reRoll == "No") {
          return; // No need to perform re-roll logic, exit early.
        }

        doLockDie = reRoll == "Remaining"; // Lock doubles if only rolling remaining, otherwise
                                           // don't lock dice.
      }

      if (doLockDie) {
        this.LockDice(twoOfAKind); // Lock all dice that have the same value as the double.
      }

      this.RollDice(); // Re-roll the dice.
      this.PrintRolledDice(); // Print the new dice values.
    }

    /// <summary>
    /// A method that is used to lock all dice with a specific current value.
    /// </summary>
    /// <param name="dieValue">
    /// An integer value of the number of the dice to lock.
    /// </param>
    private void LockDice(int dieValue) {
      this._dice
        .Where(die => die.Value == dieValue)
        .ToList<Die>()
        .ForEach(die => die.Locked = true);
    }

    public static int TwoOfAKind(int[] diceRolled) {
      foreach (KeyValuePair<int, int> kv in ThreeOrMore.GetFrequencies(diceRolled)) {
        if (kv.Value != 2) {
          continue;
        }

        return kv.Key;
      }
      return -1;
    }

    public static int GetScoreFromValues(int[] values) {
      int[] occured = ThreeOrMore.Occurances(ThreeOrMore.GetFrequencies(values));
      int total = ThreeOrMore.CalculateScore(occured);
      return total;
    }

    public static Dictionary<int, int> GetFrequencies(int[] values) {
      Dictionary<int, int> occured = new Dictionary<int, int>();

      foreach (int value in values) {
        if (!occured.ContainsKey(value)) {
          occured.Add(value, 0);
        }

        occured[value] += 1;
      }

      return occured;
    }

    public static int[] Occurances(Dictionary<int, int> valueFreqs) {
      return valueFreqs
        .Select<KeyValuePair<int, int>, int>(kv => kv.Value)
        .ToArray();
    }

    public static int CalculateScore(int[] diceFreq) {
      if (diceFreq.Contains(5)) {
        return 12;
      } else if (diceFreq.Contains(4)) {
        return 6;
      } else if (diceFreq.Contains(3)) {
        return 3;
      }
      return 0;
    }
  }
}
