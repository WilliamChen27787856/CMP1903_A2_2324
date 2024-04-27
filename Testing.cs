using System;
using System.Linq;
using System.Diagnostics;

namespace CMP1903_A2_2324 {

  public class Testing {

    public static void TestSevensOut() {
      Game.DEBUG = true;
      Game sevensOut = new SevensOut(true);
      sevensOut.Play(); // Auto-play Game.

      int playerOne = sevensOut.PlayerOneScore;
      int playerTwo = sevensOut.PlayerTwoScore;
      int[] diceValues = sevensOut.DieValues;
      int sum = sevensOut.DieSum;

      // Check if successfully stops before 7.
      Debug.Assert(diceValues.Sum() == 7, "Game ended with non-zero sum.");

      // Ensure total die values are different to the sum property, this should never occur.
      Debug.Assert(diceValues.Sum() == sum, "Game sum invalidated.");
      Game.DEBUG = false;
    }

    public static void TestThreeOrMore() {
      Game.DEBUG = true;

      // TODO: Ensure:
      // Scores set.
      // Scores added correctly.
      // Total >= 20 recognised.

      Game.DEBUG = false;
    }


  }

}
