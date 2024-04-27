using System;
using System.Collections.Generic;
using System.Linq;

namespace CMP1903_A2_2324 {

  public sealed class ThreeOrMore : Game {
    private static readonly string GAME_NAME = "Three Or More";
    private static readonly int DICE_COUNT = 5;

    public ThreeOrMore(bool againstComputer) : base(GAME_NAME, DICE_COUNT, 6, againstComputer) {
    }

    public override bool NextMove() {
      this.RollDice();
      this.PrintRolledDice();

      this.CheckTwoOfAKind();

      int total = ThreeOrMore.GetScoreFromValues(this.DieValues);
      this.AddScorePlayer(total);

      if (this.GetScorePlayer() >= 20) {
        return false;
      }

      this.SwitchPlayer();
      return true; // Continue playing.
    }

    private void CheckTwoOfAKind() {
      int twoOfAKind = TwoOfAKind();

      if (twoOfAKind == -1) {
        return;
      }

      bool computerPlaying = this.IsPlayerComputer();
      bool doLockDie = computerPlaying;

      Game.ScreenPrint($"{this.GetPlayerName()} has got a double.");
      if (!computerPlaying) {
        Game.ScreenPrint("Would you like to re-roll the others?");
        string reRoll = Game.Choice(new string[] { "All", "Remaining", "No" });

        if (reRoll == "No") {
          return;
        }

        doLockDie = reRoll == "Remaining";
      }

      if (doLockDie) {
        this.LockTwoDice(twoOfAKind);
      }

      this.RollDice();
      this.PrintRolledDice();
    }

    private int TwoOfAKind() {
      foreach (KeyValuePair<int, int> kv in ThreeOrMore.GetFrequencies(this.DieValues)) {
        if (kv.Value != 2) {
          continue;
        }

        return kv.Key;
      }
      return -1;
    }

    private bool LockTwoDice(int dieValue) {
      Die[] foundDice = this._dice
        .Where(die => die.Value == dieValue)
        .ToArray();

      if (foundDice.Length < 2) {
        return false;
      }

      foundDice[0].Locked = true;
      foundDice[1].Locked = true;
      return true;
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
