using System;
using System.Linq;
using System.Collections.Generic;

namespace CMP1903_A2_2324 {

  public abstract class Game {

    public static void Main(string[] args) {
      Game game = PickGame();
      game.NextMove();
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

    protected sealed int[] RollDie() {
      return _dice.Select(die => die.Roll()).ToArray();
    }

    protected sealed void SwitchPlayer() {
      this.PlayerOneMove = !this.PlayerOneMove;
    }

    protected sealed void AddScoreCurrentPlayer(int amount) {
      if (this.PlayerOneMove) {
        this.PlayerOneScore += amount;
        return;
      }
      this.PlayerTwoScore += amount;
    }

    protected sealed int GetScoreCurrentPlayer() {
      return this.PlayerOneMove ? PlayerOneScore : PlayerTwoScore;
    }

    protected sealed string GetPlayerName() {
      return this.PlayerOneMove ? "Player One" : (this.AgainstComputer ? "Computer" : "Player Two");
    }

    /*
    * All code below this point is utilities for console applications, these are not required for
    * GUI applications.
    */
    public static Game PickGame() {
      string gameChoice = Choice(new string[] { "Sevens Out", "Three or More" });
      Game game;
      switch (gameChoice) {
        case "Sevens Out":
          return new SevensOut(true);
        default:
          return new SevensOut(true);
      }
    }

    public static T Choice<T>(T[] choices) {
      int i = 0;
      Console.WriteLine("========================================");
      foreach (T item in choices) {
        Console.WriteLine($"[{i}] {item.ToString()}");
        i++;
      }
      Console.WriteLine("========================================");
      return choices[IntChoice(0, choices.Length - 1)];
    }

    public static int IntChoice(int min, int max) {
      while (true) {
        try {
          Console.Write("Input > ");
          int choice = int.Parse(Console.ReadLine());
          if (choice < min || choice > max) {
            Console.WriteLine($"Please enter a number in the range {min} to {max} (inclusive).");
            continue;
          }
          return choice;
        } catch (Exception) {
          Console.WriteLine("Please enter a number.");
        }
      }
    }

  }

}
