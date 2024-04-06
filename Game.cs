using System;

namespace CMP1903_A2_2324 {

  public class Game {

    public static void Main(string[] args) {
      GameEnum game = Choice(new GameEnum[] { GameEnum.SEVENS_OUT, GameEnum.THREE_OR_MORE });
      if (game == GameEnum.SEVENS_OUT) {
        // TODO: Create Sevens Out game.
      } else if (game == GameEnum.THREE_OR_MORE) {
        // TODO: Create Three Or More game.
      } else {
        Console.WriteLine("Could not instantiate game."); // This should not happen.
        return;
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

  public enum GameEnum {
    SEVENS_OUT, THREE_OR_MORE
  }

}
