using System;

namespace CMP1903_A2_2324 {

  public abstract class Game {

    public static void Main(string[] args) {
      Game game = PickGame();
      game.Play();
    }

    /*
    * All code below this point is the definition of the Game class to base the SevensOut and
    * ThreeOrMore classes on, this will create a nice interface that allows the main program logic
    * to be written without needing to do anything specific for each game to be played.
    */

    public bool AgainstComputer { get; private set; }
    public bool PlayerOneMove { get; private set; } = true;
    public int Score {get; private set; } = 0;

    public Game(int dieCount, int sidesPerDie, bool againstComputer) {
      this.AgainstComputer = againstComputer;
    }

    public abstract void Play();
    public abstract void CalculateComputerTurn();

    /*
    * All code below this point is utilities for console applications, these are not required for
    * GUI applications.
    */
    public static Game PickGame() {
      string gameChoice = Choice(new string[] { "Sevens Out", "Three or More" });
      Game game;
      switch (gameChoice) {
        case "Sevens Out":
          return new SevensOut(3, 6, true);
        default:
          return new ThreeOrMore(3, 6, true);
      }
      return null; // This cannot be happen.
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
