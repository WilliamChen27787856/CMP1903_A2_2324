using System;

namespace CMP1903_A2_2324 {

  public class ThreeOrMore {
    private static readonly int DICE_COUNT = 5;

    public ThreeOrMore(bool againstComputer) : base(DICE_COUNT, 6, againstComputer) {
    }

    public override bool NextMove() {
      int[] rolledDice = this.RollDie();



      this.AddScoreCurrentPlayer(total);
      this.SwitchPlayer();
      return true; // Continue playing.
    }
  }

}
