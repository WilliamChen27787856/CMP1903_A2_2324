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

      if (TwoOfAKind()) {
        // TODO: Ask to re-roll.
      }

      // TODO: Calculate points.
	    // - 3-of-a-kind: 3 points
	    // - 4-of-a-kind: 6 points
	    // - 5-of-a-kind: 12 points
	    // - First to a total of 20 points.

      this.AddScorePlayer(total);
      this.SwitchPlayer();
      return true; // Continue playing.
    }

    private bool TwoOfAKind() {
      Dictionary<int, Die> occured = new Dictionary<int, Die>();
      foreach (Die die in this._dice) {
        if (!occured.ContainsKey(die.Value)) {
          occured.Add(die.Value, die);
          continue;
        }

        die.Locked = true;
        occured[die.Value].Locked = true;
        return true;
      }
      return false;
    }
  }

}
