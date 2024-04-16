using System;
using System.Linq;

namespace CMP1903_A2_2324 {

  public class SevensOut : Game {

    private static readonly int DICE_COUNT = 2;

    public SevensOut(bool againstComputer) : base(DICE_COUNT, 6, againstComputer) {
    }

    public override bool NextMove() {
      int[] rolledDice = this.RollDice();
      int total = rolledDice.Sum();

      int dieOne = rolledDice[0];
      int dieTwo = rolledDice[1];

      Game.ScreenPrint($"{this.GetPlayerName()} rolled {dieOne} & {dieTwo}.");

      if (total == 7) {
        Game.ScreenPrint($"{this.GetPlayerName()} has lost...");
        return false; // Game over...
      }

      if (dieOne == dieTwo) {
        total *= 2;
      }

      this.AddScorePlayer(total);
      Game.ScreenPrint($"{this.GetPlayerName()}: {total} added to total ({this.GetScorePlayer()})");

      this.SwitchPlayer();
      return true; // Continue playing.
    }

  }

}
