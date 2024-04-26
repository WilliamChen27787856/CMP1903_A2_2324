using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace CMP1903_A2_2324 {

  public abstract class Game {

    public static void Main(string[] args) {
      // TODO: Perform testing.
      while (MainMenu()) {
      }
    }


    public static Game PickGame(bool againstComputer = false) {
      Game.ScreenPrint("What game would you like to play?");
      string gameChoice = Choice(new string[] { "Sevens Out", "Three or More" });
      switch (gameChoice) {
        case "Sevens Out":
          return new SevensOut(againstComputer);
        default:
          return new ThreeOrMore(againstComputer);
      }
    }

    public static void RunGame(Game game) {
      game.Play();
      Game.Pause();
      Game.ScreenPrint($"Player One: scored {game.PlayerOneScore}.");
      Game.ScreenPrint(
          $"{(game.AgainstComputer ? "Computer" : "Player Two")}: scored {game.PlayerTwoScore}."
      );
      Statistics.INSTANCE.AddEndGameStats(game);
    }

    public static bool MainMenu() {
      string option = Choice<string>(new string[] { "Play Game", "View Statistics", "Quit" });

      if (option == "Quit") {
        return false;
      }

      if (option == "Play Game") {
        Game game = Game.PickGame(true);
        Game.RunGame(game);
      } else {
        Console.WriteLine(Statistics.INSTANCE.ToString());
      }

      return true;
    }

    /*
    * All code below this point is the definition of the Game class to base the SevensOut and
    * ThreeOrMore classes on, this will create a nice interface that allows the main program logic
    * to be written without needing to do anything specific for each game to be played.
    */

    private readonly string _gameName;
    public string GameName { get { return this._gameName; } }

    public bool AgainstComputer { get; private set; }
    public bool PlayerOneMove { get; private set; } = true;
    public int PlayerOneScore { get; protected set; } = 0;
    public int PlayerTwoScore { get; protected set; } = 0;
    protected Die[] _dice;

    public Game(string gameName, int diceCount, int sidesPerDie, bool againstComputer) {
      this._gameName = gameName;
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
        if (!this.IsPlayerComputer()) {
          Game.Pause($"{this.GetPlayerName()}'s turn, press enter to roll the dice: ");
        }
      } while (this.NextMove());
      Game.ScreenPrint("=====================");
    }

    protected int[] RollDice() {
      int[] rolledDie = _dice.Select<Die, int>(die => die.Roll()).ToArray();
      Statistics.INSTANCE.AddNewGameRoll(this, rolledDie);
      return rolledDie;
    }

    protected void PrintRolledDice() {
      StringBuilder sb = new StringBuilder();
      sb.Append($"{this.GetPlayerName()} has rolled: ");
      sb.AppendJoin<Die>(", ", this._dice);
      sb.Append(".");
      Game.ScreenPrint(sb.ToString());
    }

    protected void SwitchPlayer() {
      this.PlayerOneMove = !this.PlayerOneMove;
      this.UnlockAllDie(); // As a precaution, ensure no die is locked for next turn.
    }

    protected void AddScorePlayer(int amount) {
      if (this.PlayerOneMove) {
        this.PlayerOneScore += amount;
      } else {
        this.PlayerTwoScore += amount;
      }
      Game.ScreenPrint($"{this.GetPlayerName()}: {amount} added to total ({this.GetScorePlayer()})");
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

    protected bool IsPlayerComputer() {
      return !this.PlayerOneMove && this.AgainstComputer;
    }

    /*
    * Method below here would be required to be re-implemented to work in a GUI.
    * They act as a wrapper so we do not need to replace the functions throughout
    * the rest of the program's code.
    */

    public static string ScreenPrint(string text, bool newLine = true) {
      Console.Write(text + (newLine ? "\n" : ""));
      return text;
    }

    public static string UserInput(string text = "Input > ") {
      ScreenPrint(text, false);
      return Console.ReadLine();
    }

    public static bool Pause(string message = "") {
      Game.ScreenPrint(message, false);
      Console.ReadKey();
      return true;
    }

    /*
    * The following methods make use of the above wrapper methods and so do not need
    * re-implementing, unless for the purpose of removing parts, such as the
    * line dividers.
    */
    public static T Choice<T>(T[] choices) {
      Game.ScreenPrint("=====================");
      for (int i = 0; i < choices.Length; i++) {
        Game.ScreenPrint($"[{i}] {choices[i].ToString()}");
      }
      Game.ScreenPrint("=====================");
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
