using System;
using System.Linq;
using System.Collections.Generic;

namespace CMP1903_A2_2324 {

  public abstract class Game {

    public static bool DEBUG = false;

    public static void Main(string[] args) {
      Game game = PickGame();
      // TODO: Continual playing until game returns false.
      game.Play();

      // TODO: Collect statistics.
      // TODO: Get player scores and print them out.
      // TODO: Perform testing.
    }

    /*
    * All code below this point is the definition of the Game class to base the SevensOut and
    * ThreeOrMore classes on, this will create a nice interface that allows the main program logic
    * to be written without needing to do anything specific for each game to be played.
    */

    public bool AgainstComputer { get; private set; }
    public bool PlayerOneMove { get; private set; } = true;
    public int PlayerOneScore { get; protected set; } = 0;
    public int PlayerTwoScore { get; protected set; } = 0;
    protected Die[] _dice;

    public Game(int diceCount, int sidesPerDie, bool againstComputer) {
      this.AgainstComputer = againstComputer;
      this._dice = new Die[diceCount];
      for (int i = 0; i < diceCount; i++) {
        this._dice[i] = new Die(sidesPerDie);
      }
    }

    public abstract bool NextMove();

    public void Play() {
      do {
        Game.ScreenPrint("=====================");
      } while (this.NextMove());
      Game.ScreenPrint("=====================");
    }

    protected int[] RollDice() {
      return _dice.Select<Die, int>(die => die.Roll()).ToArray();
    }

    protected void SwitchPlayer() {
      this.PlayerOneMove = !this.PlayerOneMove;
      this.UnlockAllDie(); // As a precaution, ensure no die is locked for next turn.
    }

    protected void AddScorePlayer(int amount) {
      if (this.PlayerOneMove) {
        this.PlayerOneScore += amount;
        return;
      }
      this.PlayerTwoScore += amount;
    }

    protected int GetScorePlayer() {
      return this.PlayerOneMove ? PlayerOneScore : PlayerTwoScore;
    }

    protected string GetPlayerName() {
      return this.PlayerOneMove ? "Player One" : (this.AgainstComputer ? "Computer" : "Player Two");
    }

    protected void UnlockAllDie() {
      foreach (Die die in this._dice) {
        die.Locked = false;
      }
    }

    /*
    * All code below this point is utilities for console applications, these are not required for
    * GUI applications.
    */
    public static Game PickGame() {
      string gameChoice = Choice(new string[] { "Sevens Out", "Three or More" });
      switch (gameChoice) {
        case "Sevens Out":
          return new SevensOut(true);
        default:
          return new ThreeOrMore(true);
      }
    }


    /*
    * Method below here would be required to be re-implemented to work in a GUI.
    * They act as a wrapper so we do not need to replace the functions throughout
    * the rest of the program's code.
    */

    public static string ScreenPrint(string text) {
      if (DEBUG) {
        Console.WriteLine(text);
      }
      return text;
    }

    public static string UserInput(string text = "Input >") {
      ScreenPrint(text);
      return Console.ReadLine();
    }

    /*
    * These methods below would only be need to be rewritten if they were to
    * display text differently to a single text box in a GUI.
    */
    public static T Choice<T>(T[] choices) {
      Game.ScreenPrint("========================================");
      for (int i = 0; i < choices.Length; i++) {
        Game.ScreenPrint($"[{i}] {choices[i].ToString()}");
      }
      Game.ScreenPrint("========================================");
      return choices[IntChoice(0, choices.Length - 1)];
    }

    public static int IntChoice(int min, int max) {
      while (true) {
        try {
          int choice = int.Parse(Game.UserInput());
          if (choice < min || choice > max) {
            Game.ScreenPrint($"Please enter a number in the range {min} to {max} (inclusive).");
            continue;
          }
          return choice;
        } catch (Exception) {
          Game.ScreenPrint("Please enter a number.");
        }
      }
    }

  }

}
