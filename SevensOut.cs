using System;
using System.Linq;

namespace CMP1903_A2_2324 {

  public class SevensOut : Game {

    private static readonly int DICE_COUNT = 2;

    public SevensOut(bool againstComputer) : base(DICE_COUNT, 6, againstComputer) {
    }

    public override bool NextMove() {
      int[] rolledDice = this.RollDie();
      int total = rolledDice.Sum();

      if (total == 7) {
        return false; // Game over...
      }

      if (rolledDice[0] == rolledDice[1]) {
        total *= 2;
      }

      Console.WriteLine(this.GetPlayerName() + ": " + total);
      this.AddScoreCurrentPlayer(total);
      this.SwitchPlayer();
      return true; // Continue playing.
    }

  }

}
