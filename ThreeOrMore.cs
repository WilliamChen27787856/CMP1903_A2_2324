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

      while (TwoOfAKind()) {

      }


      this.AddScoreCurrentPlayer(total);
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
