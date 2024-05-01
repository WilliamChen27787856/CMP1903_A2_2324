using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace CMP1903_A2_2324 {

  /// <summary>
  /// An abstract class that acts as both the program entry-point and a base class for the
  /// individual game objects.
  /// </summary>
  /// <remarks>
  /// This class has to be the main class/contain the program entry-point to abide by the brief
  /// only allowing 6 classes in the program.
  /// </remarks>
  public abstract class Game : IPlayable {

    /// <summary>
    /// This is the program entry point.
    /// </summary>
    public static void Main() {
      Testing.RunTests();
      while (MainMenu()) {
      }
    }

    /// <summary>
    /// This method is used to decide what game should be played.
    /// <summary>
    /// <param name="againstComputer">
    /// A boolean value to decide if the game should be against a computer or not.
    /// Defaults to false (not against computer).
    /// </param>
    /// <returns>
    /// An instance of a class that extends the <c>Game</c> class.
    /// </returns>
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

    /// <summary>
    /// This method is used to perform the basic parts of running and logging game information.
    /// </summary>
    /// <param name="game">
    /// A <c>Game</c> object to start playing.
    /// </param>
    public static void RunGame(Game game) {
      game.Play();
      Game.Pause();
      Game.ScreenPrint($"Player One: scored {game.PlayerOneScore}.");
      Game.ScreenPrint(
          $"{(game.AgainstComputer ? "Computer" : "Player Two")}: scored {game.PlayerTwoScore}."
      );
      if (!Game.DEBUG) {
        Statistics.INSTANCE.AddEndGameStats(game);
      }
    }

    /// <summary>
    /// Displays the main menu and allows for the player to choose between playing a game,
    /// viewing statistics of previous games and to quit.
    /// </summary>
    /// <returns>
    /// A boolean, false if the program should quit, true if the user has selected another option.
    /// </returns>
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

    /// <value>
    /// A string property that stores the game name as an identifier for identifying the game type.
    /// </value>
    public string GameName { get; private set; }

    /// <value>
    /// A boolean property used to store if the program if player 2 is the computer or not.
    /// </value>
    public bool AgainstComputer { get; private set; }

    /// <value>
    /// A boolean property used to store if the current move is player 1 or player 2.
    /// Player 1 = True, Player 2 = False.
    /// </value>
    public bool PlayerOneMove { get; private set; } = true;

    /// <value>
    /// An integer property to store the current score of player 1.
    /// </value>
    public int PlayerOneScore { get; protected set; } = 0;

    /// <value>
    /// An integer property to store the current score of player 2.
    /// </value>
    public int PlayerTwoScore { get; protected set; } = 0;

    /// <value>
    /// A field that stores all of the Die objects associated with the Game being played.
    /// </value>
    protected Die[] _dice;

    /// <value>
    /// An integer property that retrieves the current sum of all the values on the dice stored in
    /// <c>_dice</c>.
    /// </value>
    public int DieSum { get { return this._dice.Sum<Die>(die => die.Value); } }

    /// <value>
    /// An array of integers property that stores the current die mapped to their current int value.
    /// </value>
    public int[] DieValues { get { return this._dice.Select<Die, int>(die => die.Value).ToArray(); } }

    /// <summary>
    /// The constructor for the Game object.
    /// </summary>
    /// <param name="gameName">
    /// A string value for the game's identifier.
    /// </param>
    /// <param name="diceCount">
    /// An integer value for the number of die used in the game.
    /// </param>
    /// <param name="sidesPerDie">
    /// An integer value for the number of sides each die has.
    /// </param>
    /// <param name="againstComputer">
    /// A boolean representing if the game is against a computer or not.
    /// </param>
    /// <remarks>
    /// Most of these parameters are not used in object instantiation and are instead
    /// hard-programmed into the individual game constructors.
    /// </remarks>
    public Game(string gameName, int diceCount, int sidesPerDie, bool againstComputer) {
      this.GameName = gameName;
      this.AgainstComputer = againstComputer;
      this._dice = new Die[diceCount];
      for (int i = 0; i < diceCount; i++) {
        this._dice[i] = new Die(sidesPerDie);
      }

    }

    public abstract bool NextTurn();

    /// <summary>
    /// The standard implementation of the Play method continually calls the NextMove method until
    /// the game is over.
    /// This method is virtual so that if required games can override this default code.
    /// </summary>
    /// <remarks>
    /// This method is not overwritten by any of the current games however to allow extensibility
    /// this method is virtual.
    /// </remarks>
    public void Play() {
      do {
        Game.ScreenPrint("=====================");
        if (!this.IsPlayerComputer()) {
          Game.Pause($"{this.GetPlayerName()}'s turn, press enter to roll the dice: ");
        }
      } while (this.NextTurn());
      Game.ScreenPrint("=====================");
    }

    /// <summary>
    /// A method that allows for all of the games to roll all the dice and add the rolls to the
    /// statistics singleton.
    /// </summary>
    /// <returns>
    /// An array of all the integer values on the dice.
    /// </returns>
    protected int[] RollDice() {
      int[] rolledDie = _dice.Select<Die, int>(die => die.Roll()).ToArray();
      if (!Game.DEBUG) {
        Statistics.INSTANCE.AddNewGameRoll(this, rolledDie);
      }
      return rolledDie;
    }

    /// <summary>
    /// A method that will print out the current dice values with a message saying the player who
    /// rolled them.
    /// </summary>
    protected void PrintRolledDice() {
      StringBuilder sb = new StringBuilder();
      sb.Append($"{this.GetPlayerName()} has rolled: ");
      sb.Append(this.ToString());
      sb.Append(".");
      Game.ScreenPrint(sb.ToString());
    }

    /// <summary>
    /// A method that makes it easy to switch the players.
    /// </summary>
    /// <remarks>
    /// This method also unlocks all of the dice in the game incase the game requires dice locking.
    /// </remarks>
    protected void SwitchPlayer() {
      this.PlayerOneMove = !this.PlayerOneMove;
      this.UnlockAllDie(); // As a precaution, ensure no die is locked for next turn.
    }


    /// <summary>
    /// A method that finds the current player playing and adds an amount to their score.
    /// </summary>
    protected void AddScorePlayer(int amount) {
      if (this.PlayerOneMove) {
        this.PlayerOneScore += amount;
      } else {
        this.PlayerTwoScore += amount;
      }
      Game.ScreenPrint($"{this.GetPlayerName()}: {amount} added to total ({this.GetScorePlayer()})");
    }

    /// <summary>
    /// A method that gets the score of the currently active player.
    /// </summary>
    /// <returns>
    /// An integer value of the player's score.
    /// </returns>
    protected int GetScorePlayer() {
      return this.PlayerOneMove ? PlayerOneScore : PlayerTwoScore;
    }

    /// <summary>
    /// A method that gets the name of the currently active player.
    /// It also takes into account if player 2 is the computer or not.
    /// </summary>
    /// <returns>
    /// A string of the name given to the player.
    /// </returns>
    protected string GetPlayerName() {
      return this.PlayerOneMove ? "Player One" : (this.AgainstComputer ? "Computer" : "Player Two");
    }

    /// <summary>
    /// A method that is used to apply Locked = false to every Die object within the game.
    /// This is useful to reset state between games.
    /// </summary>
    protected void UnlockAllDie() {
      foreach (Die die in this._dice) {
        die.Locked = false;
      }
    }

    /// <summary>
    /// A method to return if the current player is a computer or not.
    /// </summary>
    /// <returns>
    /// A boolean, true if the player is a computer, false otherwise.
    /// </returns>
    protected bool IsPlayerComputer() {
      return !this.PlayerOneMove && this.AgainstComputer;
    }

    /// <summary>
    /// Overrides the default ToString method to return all of the dice values.
    /// </summary>
    /// <returns>
    /// A string containing the current value of all of the dice.
    /// </returns>
    public override string ToString() {
      return new StringBuilder().AppendJoin<Die>(", ", this._dice).ToString();
    }

    /*
    * Method below here would be required to be re-implemented to work in a GUI.
    * They act as a wrapper so we do not need to replace the functions throughout
    * the rest of the program's code.
    */

    /// <value>
    /// This static field is used to stores if the program should be placed into Debug mode,
    /// it is used to remove logging and user input questions.
    /// </value>
    /// <remarks>
    /// This is only useful for the Testing class purposes and otherwise should remain false.
    /// </remarks>
    public static bool DEBUG = false;

    /// <value>
    /// This is a static field that depends on the <c>DEBUG</c> field being true, it is used to
    /// force user-input when a question should be asked so the game can be tested headlessly.
    /// </value>
    public static string DEBUG_INPUT = "";

    /// <summary>
    /// The ScreenPrint method acts as a wrapper for displaying text to the screen.
    /// </summary>
    /// <param name="text">
    /// A string for the text to display to the screen.
    /// </param>
    /// <param name="newLine">
    /// A boolean for if there should be a newline character appended to the end of the string.
    /// </param>
    /// <returns>
    /// The string that has been added to the screen.
    /// </returns>
    /// <remarks>
    /// For the purpose of this program this just performs a <c>Console.WriteLine</c> call, but
    /// if the program was to be converted to a GUI program this could be reimplemented to either
    /// display new text elements of add text to a 'display queue' for text that needs to be put
    /// to the screen.
    /// </remarks>
    public static string ScreenPrint(string text, bool newLine = true) {
      if (DEBUG) {
        return text;
      }
      Console.Write(text + (newLine ? "\n" : ""));
      return text;
    }

    /// <summary>
    /// The ScreenPrint method acts as a wrapper for taking the user input.
    /// </summary>
    /// <param name="text">
    /// A string parameter that takes the text that should be displayed before the input is taken.
    /// </param>
    /// <returns>
    /// A string of the input the user gave.
    /// </returns>
    /// <remarks>
    /// This currently performs a <c>Console.ReadLine</c> call, but it could be replaced to take
    /// input from an input box.
    /// </remarks>
    public static string UserInput(string text = "Input > ") {
      if (DEBUG) {
        return DEBUG_INPUT;
      }
      Game.ScreenPrint(text, false);
      return Console.ReadLine();
    }

    /// <summary>
    /// This is a wrapper method that is used to pause until an input is given to continue program
    /// execution.
    /// </summary>
    /// <param name="message">
    /// A string parameter that takes the text that should be displayed.
    /// </param>
    /// <returns>
    /// A boolean value for if the game was successfully paused.
    /// </returns>
    /// <remarks>
    /// Currently this method only returns true as there is no case where the game cannot pause.
    /// However, in a GUI application there could be cases where this is not possible.
    /// </remarks>
    public static bool Pause(string message = "") {
      if (DEBUG) {
        return true;
      }
      Game.ScreenPrint(message, false);
      Console.ReadKey();
      return true;
    }

    /*
    * The following methods make use of the above wrapper methods and so do not need
    * re-implementing, unless for the purpose of removing parts, such as the
    * line dividers.
    */

    /// <summary>
    /// This is a generic method that allows for an item of any type to be chosen from an array in
    /// a program-standardised interface.
    /// </summary>
    /// <param name="choices">
    /// The array of items
    /// </param>
    /// <returns>
    /// An object of generic type T that the user selected.
    /// </returns>
    /// <remarks>
    /// This method would likely need new implementation to make it user-interface friendly,
    /// however the method signature would not need many changes.
    /// </remarks>
    public static T Choice<T>(T[] choices) {
      Game.ScreenPrint("=====================");
      for (int i = 0; i < choices.Length; i++) {
        Game.ScreenPrint($"[{i}] {choices[i].ToString()}");
      }
      Game.ScreenPrint("=====================");
      return choices[IntChoice(0, choices.Length - 1)];
    }

    /// <summary>
    /// This is a static helper method to make taking a user input of type int easier.
    /// </summary>
    /// <param name="min">
    /// An integer of the minimum value that can be entered.
    /// </param>
    /// <param name="max">
    /// An integer of the maximum value that can be entered.
    /// </param>
    /// <returns>
    /// An integer from the user input.
    /// </returns>
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
