using System;
using System.Collections.Generic;
using System.Linq;

namespace CMP1903_A2_2324 {

  public class ThreeOrMore : Game {
    private static readonly int DICE_COUNT = 5;

    public ThreeOrMore(bool againstComputer) : base(DICE_COUNT, 6, againstComputer) {
    }

    public override bool NextMove() {
      int total = 0;
      int[] rolledDice = this.RollDice();
      this.PrintRolledDice();

      int twoOfAKind = TwoOfAKind();

      if (twoOfAKind != -1) {
        Game.ScreenPrint("You got a double. Would you like to re-roll the others?");
        string reRoll = Game.Choice(new string[] { "All", "Remaining", "No" });

        if (reRoll == "Remaining") {
          this.LockTwoDice(twoOfAKind);
        }

        if (reRoll != "No") {
          rolledDice = this.RollDice();
        }

        this.PrintRolledDice();
      }

      int[] occured = this.Occurances();

      if (occured.Contains(5)) {
        total += 12;
      } else if (occured.Contains(4)) {
        total += 6;
      } else if (occured.Contains(3)) {
        total += 3;
      }

      if (GetScorePlayer() >= 20) {
        return false;
      }

      Game.Pause();
      this.AddScorePlayer(total);
      this.SwitchPlayer();
      return true; // Continue playing.
    }

    private Dictionary<int, int> NumberOfAKind() {
      Dictionary<int, int> occured = new Dictionary<int, int>() {
        {1, 0}, {2, 0}, {3, 0}, {4, 0}, {5, 0}, {6, 0}
      };
      foreach (Die die in this._dice) {
        occured[die.Value] += 1;
      }
      return occured;
    }

    private int[] Occurances() {
      return this.NumberOfAKind()
        .Select<KeyValuePair<int, int>, int>(kv => kv.Value)
        .ToArray();
    }

    private int TwoOfAKind() {
      foreach (KeyValuePair<int, int> kv in NumberOfAKind()) {
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
  }
}
