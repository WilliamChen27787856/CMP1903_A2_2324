using System;
using System.Linq;

namespace CMP1903_A2_2324 {

  public sealed class SevensOut : Game {

    /// <value>
    /// A constant value for the game's identifier.
    /// </value>
    private const string GAME_NAME = "Sevens Out";

    /// <value>
    /// A constant value for the number of dice that are inside of the game.
    /// </value>
    private const int DICE_COUNT = 2;

    /// <summary>
    /// The constructor for the class, it is used to instantiate the default values for the game
    /// </summary>
    /// <param name="againstComputer">
    /// If the game should be against a computer or not.
    /// </param>
    public SevensOut(bool againstComputer) : base(GAME_NAME, DICE_COUNT, 6, againstComputer) {
    }

    /// <summary>
    /// This method is an override of an abstract method from the <c>Game</c> class.
    /// This method implements the game requirements for the Sevens Out game rules.
    /// </summary>
    /// <returns>
    /// A boolean for if the game should continue or not, true = continue, false = exit.
    /// </returns>
    public override bool NextMove() {
      int[] rolledDice = this.RollDice();
      int total = rolledDice.Sum();

      this.PrintRolledDice(); // Print out what the player rolled.

      if (total == 7) {
        Game.ScreenPrint($"{this.GetPlayerName()} has lost...");
        return false; // Game over...
      }

      if (rolledDice[0] == rolledDice[1]) {
        Game.ScreenPrint($"{this.GetPlayerName()} rolled the same numbers, score 2x!");
        total *= 2; // Score doubles if both die landed on the same side.
      }

      this.AddScorePlayer(total); // Add the score to the player.
      this.SwitchPlayer(); // Switch to the next player.
      return true; // Continue playing.
    }

  }

}
